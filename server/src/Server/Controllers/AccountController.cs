using System;
using System.Linq;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models;
using Chad.Models.Common;
using Chad.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChadApi.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ChadController
    {
        public AccountController(
            UserManager<DbUser> userManager,
            JwtManager jwtManager,
            SystemManager systemManager,
            AccountManager accountManager,
            SignInManager<DbUser> signInManager)
        {
            AccountManager = accountManager;
            UserManager = userManager;
            JwtManager = jwtManager;
            SystemManager = systemManager;
            SignInManager = signInManager;
        }

        private UserManager<DbUser> UserManager { get; }
        private JwtManager JwtManager { get; }
        private SystemManager SystemManager { get; }
        private SignInManager<DbUser> SignInManager { get; }
        private AccountManager AccountManager { get; }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginArguments arguments)
        {
            // 初次/重置后，登陆自动初始化系统
            if (!SystemManager.Initialized) await SystemManager.InitializeAsync();

            var user = await AccountManager.LoginAsync(arguments.Username, arguments.Password, JwtManager);

            if (user == null)
                return ApplicationError("用户名或密码错误。");

            await SignInManager.SignInAsync(await UserManager.FindByIdAsync(arguments.Username), arguments.RememberMe);
            return Json(user);
        }


        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] ManagedGeneratingUser user)
        {
            try
            {
                var u = await AccountManager.CreateAsync(user);
                if (u == null)
                    return ApplicationError("用户创建失败。");
                return Json(u);
            }
            catch (ArgumentException ex)
            {
                return ApplicationError(ex.Message);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return NoContent();
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpGet("managed")]
        public async Task<ActionResult<ManagedUser[]>> GetManagedUsers()
        {
            return Json((await AccountManager.GetUsersAsync())
                .Select(u => new ManagedUser
                {
                    Name = u.FriendlyName,
                    Username = u.Id,
                    Role = u.Role
                }));
        }

        /// <summary>
        ///     根据输入的筛选，获取用户。role和group只能有其中之一有效。
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<UserSummary[]>> GetUsers(
            [FromQuery] UserRole? role = null
        )
        {
            return Json((await AccountManager.GetUsersAsync(role))
                .Select(u => new UserSummary {Name = u.FriendlyName, Username = u.Id}));

            //return ManagedUser[]
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(
            [FromRoute] string id
        )
        {
            var result = await AccountManager.DeleteAsync(id);
            if (result == null)
                return NoContent();
            return ApplicationError(result);
        }

        [HttpPost("chpwd")]
        public async Task<IActionResult> Chpwd(
            [FromBody] string[] password)
        {
            var dbu = await AccountManager.GetCurrentUserAsync(); //UserManager.GetUserAsync(User);
            if (dbu == null) return Unauthorized();
            if (await UserManager.CheckPasswordAsync(dbu, password[0]))
            {
                if ((await UserManager.ChangePasswordAsync(dbu, password[0], password[1])).Succeeded)
                {
                    await SignInManager.SignOutAsync();
                    return NoContent();
                }

                return ApplicationError("新密码不合要求: 4位以上。");
            }

            return ApplicationError("旧密码输入错误。");
        }

        public record LoginArguments
        {
            public string Username { get; set; }

            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}