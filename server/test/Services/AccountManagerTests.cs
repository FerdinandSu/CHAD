using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chad.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models.Common;
using Chad.Models.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Chad.Services.Tests
{
    [TestClass]
    public class AccountManagerTests:TestBase<AccountManager>
    {

        [TestMethod]
        public void GetRoleClaimTest()
        {
            Assert.AreEqual(AccountManager.GetRoleClaim(UserRole.Administrator).ToString(),
                new Claim(ClaimTypes.Role, nameof(UserRole.Administrator)).ToString());
            Assert.AreEqual(AccountManager.GetRoleClaim(UserRole.Student).ToString(),
                new Claim(ClaimTypes.Role, nameof(UserRole.Student)).ToString());
            Assert.AreEqual(AccountManager.GetRoleClaim(UserRole.Teacher).ToString(),
                new Claim(ClaimTypes.Role, nameof(UserRole.Teacher)).ToString());
        }

        [TestMethod]
        public void GetRoleStringTest()
        {
            Assert.AreEqual(AccountManager.GetRoleString(UserRole.Administrator),
                 nameof(UserRole.Administrator));
            Assert.AreEqual(AccountManager.GetRoleString(UserRole.Student),
                 nameof(UserRole.Student));
            Assert.AreEqual(AccountManager.GetRoleString(UserRole.Teacher),
                nameof(UserRole.Teacher));
        }

        /// <summary>
        /// 一个用户的生命周期测试
        /// 测试：Create, AddToRole, MoveToRole, AddToGroup, RemoveFromGroup, Update, Reset和Delete
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task UserLifeTimeTest()
        {
            // 准备环境
            using var scope = Dependency.GetScope();
            var groupManager = scope.ServiceProvider.GetRequiredService<GroupManager>();
            var db = scope.ServiceProvider.GetRequiredService<ChadDb>();
            var um = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
            var jwt = scope.ServiceProvider.GetRequiredService<JwtManager>();
            var manager = scope.ServiceProvider.GetRequiredService<AccountManager>();

            var group1 = "TGA_LifeTime_1";
            var group2 = "TGA_LifeTime_2";
            var group2fn = "TGA_LifeTime_2,TGA_LifeTime_1";

            await groupManager.UpdateOrCreateAsync(new()
            {
                Name = group1
            });

            await groupManager.UpdateOrCreateAsync(new()
            {
                Name = group2,
                Parent = group1
            });

            //测试数据

            var creations = new ManagedGeneratingUser[]
            {
                new()
                {
                    Name = "TUA_LifeTime_0",
                    Username = "TUA_LifeTime_0",
                    Group = "",
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Student
                },

                new()
                {
                    Name = "TUA_LifeTime_1",
                    Username = "TUA_LifeTime_1",
                    Group = Group.RootParent,
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Student
                },

                new()
                {
                    Name = "TUA_LifeTime_2",
                    Username = "TUA_LifeTime_2",
                    Group = group1,
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Teacher
                },

                new()
                {
                    Name = "TUA_LifeTime_3",
                    Username = "TUA_LifeTime_3",
                    Group = group2fn,
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Student
                },
                new()
                {
                    Name = "TUA_LifeTime_SU",
                    Username = "TUA_LifeTime_SU",
                    Group = group2fn,
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Administrator
                }

            };

            // 测试

            foreach (var creation in creations)
            {
                //Create,AddToRole,AddToGroup
                var created = await manager.CreateAsync(creation);
                Assert.IsNotNull(created);
                await Assert.ThrowsExceptionAsync<ArgumentException>(
                    async () => await manager.CreateAsync(creation));
                var dbu = await db.Users.FindAsync(creation.Username);
                // 创建完成度
                Assert.AreEqual(await manager.UserCast(AccountManager.UserCast(creation)), created);
                Assert.AreEqual(await manager.UserCast(dbu), created);
                // 角色
                await TestUserInRole(dbu, creation.Role, um, manager);
                // 组
                await TestUserInGroup(dbu, creation.Group, um, manager, db);
                //密码系统
                Assert.IsNotNull(await manager.LoginAsync(creation.Name, creation.InitialPassword, jwt));
                //修改密码
                var npd = GetRandomString();
                await um.ChangePasswordAsync(dbu, creation.InitialPassword, npd);
                Assert.IsNull(await manager.LoginAsync(creation.Name, creation.InitialPassword, jwt));
                Assert.IsNotNull(await manager.LoginAsync(creation.Name, npd, jwt));
                //重置
                Assert.IsNotNull(await manager.ResetAsync(creation.Username + "##"));
                Assert.IsNull(await manager.ResetAsync(creation.Username));
                Assert.IsNotNull(await manager.LoginAsync(creation.Name, creation.InitialPassword, jwt));
                Assert.IsNull(await manager.LoginAsync(creation.Name, npd, jwt));
                //信息更新

                //修改角色
                Assert.IsNotNull(await manager.UpdateAsync(created with { Role = UserRole.Administrator }));
                UserRole altered;
                switch (created.Role)
                {
                    case UserRole.Student:
                        Assert.IsNull(await manager.UpdateAsync(created with { Role = UserRole.Teacher }));
                        altered = UserRole.Teacher;
                        break;
                    case UserRole.Teacher:
                        Assert.IsNull(await manager.UpdateAsync(created with { Role = UserRole.Student }));
                        altered = UserRole.Student;
                        break;
                    case UserRole.Administrator:
                        Assert.IsNotNull(await manager.UpdateAsync(created with { Role = UserRole.Student }));
                        altered = UserRole.Administrator;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                await TestUserInRole(dbu, altered, um, manager);
                //修改组
                var newGroup = created.Group switch
                {
                    "TGA_LifeTime_1" => group2fn,
                    "TGA_LifeTime_2,TGA_LifeTime_1" => "",
                    _ => group2fn
                };
                if (creation.Role == UserRole.Administrator)
                {
                    Assert.IsNotNull(await manager.UpdateAsync(created with { Group = newGroup }));
                }
                else
                {
                    Assert.IsNull(await manager.UpdateAsync(created with { Group = newGroup }));
                    await TestUserInGroup(dbu, newGroup, um, manager, db);
                }


                //修改姓名
                var newName = GetRandomString();
                if (creation.Role == UserRole.Administrator)
                {
                    Assert.IsNotNull(await manager.UpdateAsync(created with { Name = newName }));
                }
                else
                {
                    Assert.IsNull(await manager.UpdateAsync(created with { Name = newName }));
                    Assert.AreEqual(newName, dbu.FriendlyName);
                }

                //只修改用户名的部分，因为该行为不会出现在正常的数据包中。
                //如果出现，则会引发更新另一个用户(如果新用户名对应的用户存在)，或是用户未找到异常。
                Assert.IsNotNull(await manager.UpdateAsync(created with { Username=created.Username+"##"}));

                //删除
                if (creation.Role == UserRole.Administrator)
                {
                    Assert.IsNotNull(await manager.DeleteAsync(creation.Username));
                }
                else
                {
                    Assert.IsNull(await manager.DeleteAsync(creation.Username));
                    Assert.IsNull(await db.Users.FindAsync(creation.Username));
                    Assert.IsNotNull(await manager.DeleteAsync(creation.Username));
                }

            }

            // 清除环境
            foreach (var user in creations.Where(u => u.Role == UserRole.Administrator))
            {
                await um.DeleteAsync(await um.FindByNameAsync(user.Name));
            }

            db.Groups.Remove(await db.Groups.FindAsync(group1));
            db.Groups.Remove(await db.Groups.FindAsync(group2));
            await db.SaveChangesAsync();
        }

        private static async Task TestUserInRole(DbUser dbu, UserRole role,
    UserManager<DbUser> um, AccountManager manager)
        {
            CollectionAssert.Contains(
                (await um.GetUsersForClaimAsync(AccountManager.GetRoleClaim(role))).ToList(),dbu);
            CollectionAssert.Contains((await manager.GetUsersAsync(role)).ToList(),dbu);
        }
        private static async Task TestUserInGroup(DbUser dbu, string? group,
            UserManager<DbUser> um, AccountManager manager, ChadDb db)
        {
            var group1 = "TGA_LifeTime_1";
            var group2 = "TGA_LifeTime_2";
            var group2fn = "TGA_LifeTime_2,TGA_LifeTime_1";

            if (group == group1)
            {
                CollectionAssert.Contains((await manager.GetUsersAsync()).ToList(),dbu);
                CollectionAssert.Contains((await manager.GetUsersAsync(group: group1)).ToList(),dbu);
                CollectionAssert.DoesNotContain((await manager.GetUsersAsync(group: group2)).ToList(),dbu);
                CollectionAssert.Contains(
                    (await um.GetUsersForClaimAsync(GroupManager.GetDirectGroupClaim(group1))).ToList(),dbu);
                CollectionAssert.Contains(
                    (await um.GetUsersForClaimAsync(GroupManager.GetGroupClaim(group1))).ToList(),dbu);
                CollectionAssert.DoesNotContain(
                    (await um.GetUsersForClaimAsync(GroupManager.GetGroupClaim(group2))).ToList(),dbu);
                Assert.AreEqual(GroupManager.GroupCast(await db.Groups.FindAsync(group1)),
                    GroupManager.GroupCast(await manager.GetGroupOfUserAsync(dbu)
                                           ?? throw new NullReferenceException()));
            }
            else if (group == group2fn)
            {
                CollectionAssert.Contains((await manager.GetUsersAsync()).ToList(),dbu);
                CollectionAssert.Contains((await manager.GetUsersAsync(group: group1)).ToList(),dbu);
                CollectionAssert.Contains((await manager.GetUsersAsync(group: group2)).ToList(),dbu);
                CollectionAssert.DoesNotContain(
                    (await um.GetUsersForClaimAsync(GroupManager.GetDirectGroupClaim(group1))).ToList(),dbu);
                CollectionAssert.Contains(
                    (await um.GetUsersForClaimAsync(GroupManager.GetDirectGroupClaim(group2))).ToList(),dbu);
                CollectionAssert.Contains(
                    (await um.GetUsersForClaimAsync(GroupManager.GetGroupClaim(group1))).ToList(),dbu);
                CollectionAssert.Contains(
                    (await um.GetUsersForClaimAsync(GroupManager.GetGroupClaim(group2))).ToList(),dbu);
                Assert.AreEqual(GroupManager.GroupCast(await db.Groups.FindAsync(group2)),
                    GroupManager.GroupCast(await manager.GetGroupOfUserAsync(dbu)
                                           ?? throw new NullReferenceException()));
            }
            else
            {
                CollectionAssert.Contains((await manager.GetUsersAsync()).ToList(),dbu);
                CollectionAssert.Contains((await manager.GetUsersAsync(group: Group.RootParent)).ToList(),dbu);
                CollectionAssert.DoesNotContain((await manager.GetUsersAsync(group: group1)).ToList(),dbu);
                CollectionAssert.DoesNotContain((await manager.GetUsersAsync(group: group2)).ToList(),dbu);
                CollectionAssert.DoesNotContain(
                    (await um.GetUsersForClaimAsync(GroupManager.GetDirectGroupClaim(group1))).ToList(),dbu);
                CollectionAssert.DoesNotContain(
                    (await um.GetUsersForClaimAsync(GroupManager.GetDirectGroupClaim(group2))).ToList(),dbu);
                CollectionAssert.DoesNotContain(
                    (await um.GetUsersForClaimAsync(GroupManager.GetGroupClaim(group1))).ToList(),dbu);
                CollectionAssert.DoesNotContain(
                    (await um.GetUsersForClaimAsync(GroupManager.GetGroupClaim(group2))).ToList(),dbu);
                Assert.IsNull(await manager.GetGroupOfUserAsync(dbu));
            }
        }


        // 测试构造函数
        [TestMethod]
        public void AccountManagerTest()
        {
            _ = GetInstance();
            _ = AccountManager.RoleFilters;
        }


        //两种UserCase互为逆操作
        [TestMethod]
        public async Task UserCastTest()
        {
            using var scope = Dependency.GetScope();
            var groupManager = scope.ServiceProvider.GetRequiredService<GroupManager>();
            var manager = scope.ServiceProvider.GetRequiredService<AccountManager>();

            await groupManager.UpdateOrCreateAsync(new()
            {
                Name = "TGA_UserCast_1"
            });

            await groupManager.UpdateOrCreateAsync(new()
            {
                Name = "TGA_UserCast_2",
                Parent = "TGA_UserCast_1"
            });

            var creations = new ManagedGeneratingUser[]
            {
                new()
                {
                    Name = "TUA_UserCast_0",
                    Username = "TUA_UserCast_0",
                    Group = "",
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Student
                },

                new()
                {
                    Name = "TUA_UserCast_1",
                    Username = "TUA_UserCast_1",
                    Group = Group.RootParent,
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Student
                },

                new()
                {
                    Name = "TUA_UserCast_2",
                    Username = "TUA_UserCast_2",
                    Group = "TGA_UserCast_1",
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Teacher
                },

                new()
                {
                    Name = "TUA_UserCast_3",
                    Username = "TUA_UserCast_3",
                    Group = "TGA_UserCast_2,TGA_UserCast_1",
                    InitialPassword = GetRandomString(),
                    Role = UserRole.Student
                }
            };

            //测试

            foreach (var creation in creations)
            {
                var created = await manager.CreateAsync(creation);
                if (created is null)
                {
                    Assert.Fail();
                    return;
                }
                Assert.AreEqual(created, await manager.UserCast(AccountManager.UserCast(creation)));
            }

            //清除

            foreach (var creation in creations)
            {
                await manager.DeleteAsync(creation.Username);
            }

            await groupManager.DeleteAsync("TGA_UserCast_2");
            await groupManager.DeleteAsync("TGA_UserCast_1");
        }

        [TestMethod]
        public async Task GetCurrentUserAsyncTest()
        {
            //用户来自请求上下文，非空情况无法测试。
            //但是如果出错，程序能够捕捉到异常。
            Assert.IsNull(await GetInstance().GetCurrentUserAsync());
        }

    }
}