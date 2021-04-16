using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models;
using Chad.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chad.Services
{
    /// <summary>
    ///     账户管理器
    /// </summary>
    public class AccountManager
    {
        public AccountManager(
            IHttpContextAccessor httpContextAccessor,
            UserManager<DbUser> userManager,
            ChadDb db,
            ILogger<AccountManager> logger)
        {
            HttpContext = httpContextAccessor.HttpContext;
            UserManager = userManager;
            Logger = logger;
            Db = db;
        }

        public HttpContext HttpContext { get; }
        private UserManager<DbUser> UserManager { get; }
        private ChadDb Db { get; }
        private ILogger<AccountManager> Logger { get; }

        /// <summary>
        ///     通过上下文获取当前用户
        /// </summary>
        public ManagedUser? GetCurrentUser()
        {
            return HttpContext?.User is null
                ? null
                : new ManagedUser()
                {
                    Name = HttpContext.User.FindFirstValue(ClaimTypes.Name),
                    Username = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Role = HttpContext.User.FindFirstValue(ClaimTypes.Role).AsUserRole()
                };
        }

        /// <summary>
        ///     通过上下文获取当前用户
        /// </summary>
        public async Task<DbUser?> GetCurrentUserAsync()
        {
            return HttpContext?.User is null ? null : await UserManager.GetUserAsync(HttpContext.User);
        }

        /// <summary>
        ///     获取角色对应的Claim
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static Claim GetRoleClaim(UserRole role)
        {
            return new(ClaimTypes.Role, role.AsUserRoleString());
        }

        public static string GetRoleString(UserRole role)
        {
            return role switch
            {
                UserRole.Administrator => nameof(UserRole.Administrator),
                UserRole.Teacher => nameof(UserRole.Teacher),
                _ => nameof(UserRole.Student)
            };
        }

        /// <summary>
        ///     将新建的用户添加到角色。
        ///     具体地，为角色添加一个和Role相关的Claim；将角色与一个Role String关联。
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="role">角色</param>
        public async Task AddToRoleAsync(DbUser user, UserRole role)
        {
            Logger.LogInformation($"User {user.Id}({user.FriendlyName}) Added to Role {role.AsUserRoleString()}");
            await UserManager.AddClaimAsync(user, GetRoleClaim(role));
            await UserManager.AddToRoleAsync(user, GetRoleString(role));
            user.Role = role;
        }

        /// <summary>
        ///     用户格式转换
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ManagedUser UserCast(DbUser user)
        {
            return new()
            {
                Name = user.FriendlyName,
                Role = user.Role,
                Username = user.Id
            };
        }

        /// <summary>
        ///     用户格式转换
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DbUser UserCast(ManagedGeneratingUser user)
        {
            return new()
            {
                Id = user.Username,
                UserName = user.Username,
                FriendlyName = user.Name,
                Role = user.Role
            };
        }

        /// <summary>
        ///     尝试删除一个用户。
        /// </summary>
        /// <param name="userId">需要删除的账户名。</param>
        /// <returns>空，如果操作成功；错误的描述，如果操作失败。</returns>
        public async Task<string?> DeleteAsync(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) return "用户不存在";
            if (user.Role == UserRole.Administrator) return "无法删除管理员。";
            await UserManager.DeleteAsync(user);
            Logger.LogWarning($"User {user.Id}({user.FriendlyName}) Deleted.");
            return null;
        }


        /// <summary>
        ///     尝试创建一个用户
        /// </summary>
        /// <param name="user">用户创建所需的信息</param>
        /// <returns>创建成功的用户；空，如果创建失败。</returns>
        public async Task<ManagedUser?> CreateAsync(ManagedGeneratingUser user)
        {
            if (await Db.Users.FindAsync(user.Username) != null)
            {
                Logger.LogWarning($"User {user.Username} Already Existed, failed to create.");
                throw new ArgumentException($"User {user.Username} Already Existed, failed to create.");
            }

            var dbu = UserCast(user);
            var creation = await UserManager.CreateAsync(dbu, user.InitialPassword);
            if (!creation.Succeeded) return null;
            Db.Attach(dbu);
            await AddToRoleAsync(dbu, user.Role);
            await Db.SaveChangesAsync();
            Logger.LogWarning($"User {dbu.Id}({dbu.FriendlyName}) Created. Initial password: {user.InitialPassword}.");

            return new ManagedUser()
            {
                Name = user.Name,
                Username = user.Username,
                Role = user.Role
            };
        }

        /// <summary>
        ///     获取全部用户
        /// </summary>
        /// <param name="role">角色过滤器</param>
        /// <returns>用户列表</returns>
        public async ValueTask<IList<DbUser>> GetUsersAsync(UserRole? role = null)
        {
            if (role != null)
                return await UserManager.GetUsersForClaimAsync(GetRoleClaim((UserRole)role));
            return await Db.Users.ToListAsync();
        }

        /// <summary>
        ///     使用给定用户名和密码尝试登录。
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="jwtManager"></param>
        /// <returns>登录到的用户；空，如果登录失败。</returns>
        public async Task<User?> LoginAsync(string username, string password, JwtManager jwtManager)
        {
            var user = await UserManager.FindByIdAsync(username);

            if (user is null || !await UserManager.CheckPasswordAsync(user, password))
                return null;
            var signingCredentials = jwtManager.GetSigningCredentials();
            var claims = await JwtManager.GetLoginClaimsAsync(user);
            var tokenOptions = jwtManager.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new User
            {
                Username = user.Id,
                Name = user.FriendlyName,
                Role = user.Role,
                Token = token
            };
        }
    }
}