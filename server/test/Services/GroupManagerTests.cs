using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chad.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models.Common;
using Chad.Models.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Chad.Services.Tests
{
    //整个测试会在单个Scope中完成
    [TestClass]
    public class GroupManagerTests:TestBase<GroupManager>
    {
        private readonly Group[] _creations ={
            new()
            {
                Name = "TGG_Common_1",
                Parent=Group.RootParent
            },
            new()
            {
                Name = "TGG_Common_2",
                Parent=Group.RootParent
            },
            new()
            {
                Name = "TGG_Common_21",
                Parent = "TGG_Common_2"
            },
            new()
            {
                Name = "TGG_Common_22",
                Parent = "TGG_Common_2"
            },
            new()
            {
                Name = "TGG_Common_23",
                Parent = "TGG_Common_2"
            },
            new()
            {
                Name = "TGG_Common_221",
                Parent = "TGG_Common_22"
            },
        };
        
        private IServiceScope WorkScope { get; }
        private GroupManager DefaultInstance { get; }

        public GroupManagerTests()
        {
            // 构建环境
            WorkScope = Dependency.GetScope();
            DefaultInstance = WorkScope.ServiceProvider.GetRequiredService<GroupManager>();
        }

        ~GroupManagerTests()
        {
            WorkScope.Dispose();
        }


        [TestMethod]
        public void GroupManagerTest()
        {
            _ = GetInstance();
        }

        [TestMethod]
        public void GroupToSingletonStringTest()
        {
            Assert.AreEqual(Group.RootParent,
                GroupManager.GroupToSingletonString(null));
            Assert.AreEqual(Group.RootParent,
                GroupManager.GroupToSingletonString(new()));
            Assert.AreEqual(Group.RootParent,
                GroupManager.GroupToSingletonString(new() { Name = "" }));
            Assert.AreEqual(Group.RootParent,
                GroupManager.GroupToSingletonString(new() { Name = Group.RootParent }));
            var rnd = GetRandomString();
            Assert.AreEqual(rnd,
                GroupManager.GroupToSingletonString(new() { Name = rnd }));
        }

        [TestMethod]
        public void GetGroupClaimTest()
        {
            var rnd = GetRandomString();
            Assert.AreEqual(
                new Claim($"Chad.Groups.{rnd}", rnd).ToString(),
                GroupManager.GetGroupClaim(rnd).ToString());
            Assert.AreEqual(
                new Claim($"Chad.Groups.{Group.RootParent}", Group.RootParent).ToString(),
                GroupManager.GetGroupClaim(Group.RootParent).ToString());
        }
        /// <summary>
        /// 生成名称序列，如‘1_11’生成‘1_11,1_1’等
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string SeriesGen(string name)
        {
            var sb = new StringBuilder(name);
            name = name[..^1];
            while (name[^1] != '_')
            {
                sb.Append($",{name}");
                name = name[..^1];
            }

            return sb.ToString();
        }
        //声明周期测试
        //组织的森林变构时，在前端会检测合法性。
        [TestMethod]
        public async Task GroupLifeTimeTest()
        {
            //测试数据
            var creationFullNames = new[]
            {
                "TGG_Common_1",
                "TGG_Common_2",
                "TGG_Common_21,TGG_Common_2",
                "TGG_Common_22,TGG_Common_2",
                "TGG_Common_221,TGG_Common_22,TGG_Common_2",
                "TGG_Common_23,TGG_Common_2",
                Group.RootParent
            };
            var creationFullNamesU1 = new[]
            {
                "TGG_Common_1,TGG_Common_2",
                "TGG_Common_2",
                "TGG_Common_21,TGG_Common_2",
                "TGG_Common_22,TGG_Common_2",
                "TGG_Common_221,TGG_Common_22,TGG_Common_2",
                "TGG_Common_23,TGG_Common_2",
                Group.RootParent
            };
            var creationFullNamesU2 = new[]
            {
                "TGG_Common_2",
                "TGG_Common_21,TGG_Common_2",
                "TGG_Common_22,TGG_Common_2",
                "TGG_Common_221,TGG_Common_22,TGG_Common_2",
                "TGG_Common_23,TGG_Common_2",
                Group.RootParent
            };
            var creationFullNamesU3 = new[]
            {
                "TGG_Common_21",
                "TGG_Common_22",
                "TGG_Common_221,TGG_Common_22",
                "TGG_Common_23",
                Group.RootParent
            };
            var testUsers = new ManagedGeneratingUser[]
            {
                new()
                {
                    Username="TUG_Common_0",
                    Name="TUG_Common_1",
                    InitialPassword=GetRandomString(),
                    Group=Group.RootParent,
                    Role=UserRole.Student
                },
                new()
                {
                    Username="TUG_Common_1",
                    Name="TUG_Common_1",
                    InitialPassword=GetRandomString(),
                    Group="TGG_Common_1",
                    Role=UserRole.Student
                },
                new()
                {
                    Username="TUG_Common_2",
                    Name="TUG_Common_2",
                    InitialPassword=GetRandomString(),
                    Group="TGG_Common_22,TGG_Common_2",
                    Role=UserRole.Student
                },
                new()
                {
                    Username="TUG_Common_3",
                    Name="TUG_Common_3",
                    InitialPassword=GetRandomString(),
                    Group="TGG_Common_221,TGG_Common_22,TGG_Common_2",
                    Role=UserRole.Student
                },
                new()
                {
                    Username="TUG_Common_4",
                    Name="TUG_Common_4",
                    InitialPassword=GetRandomString(),
                    Group="TGG_Common_221,TGG_Common_22,TGG_Common_2",
                    Role=UserRole.Student
                }
            };
            // 创建
            //不会成功的创建
            Assert.IsFalse(await DefaultInstance.UpdateOrCreateAsync(new()
            {
                Name = "TGG_Create_NE",
                Parent = "TGG_Create_NEP"
            }));
            //成功的创建
            foreach (var creation in _creations)
            {
                Assert.IsTrue(
                    await DefaultInstance.UpdateOrCreateAsync(creation));
            }

            //添加完成性
            CollectionAssert.AreEquivalent(_creations, await DefaultInstance.GetGroupsAsync());
            //全称集合
            CollectionAssert.AreEquivalent(creationFullNames, await DefaultInstance.GetGroupNamesAsync());
            //过滤器
            var filters = _creations.Select(g => new Filter()
            { Name = $"组织: {g.Name}", Expression = $"group={g.Name}" }).ToList();
            filters.Add(new() { Name = "无组织", Expression = $"group={Group.RootParent}" });
            CollectionAssert.AreEquivalent(filters, await DefaultInstance.GetGroupFiltersAsync());
            //GroupToSeriesString
            foreach (var creation in _creations)
            {
                Assert.AreEqual(SeriesGen(creation.Name),
                    DefaultInstance.GroupToSeriesString(new Data.DbGroup
                    {
                        Name = creation.Name,
                        Parent = creation.Parent == Group.RootParent ? null : creation.Parent
                    }));
            }

            //创建测试用户，完成余下测试
            var am = WorkScope.ServiceProvider.GetRequiredService<AccountManager>();
            var db = WorkScope.ServiceProvider.GetRequiredService<ChadDb>();
            foreach (var user in testUsers)
            {
                await am.CreateAsync(user);
            }
            //GroupEqualsAsync
            foreach (var user in testUsers)
            {
                Assert.IsTrue(await GroupManager.GroupEqualsAsync(await db.Users.FindAsync(user.Username), user, am));
                Assert.IsFalse(await GroupManager.GroupEqualsAsync(await db.Users.FindAsync(user.Username), new()
                {
                    Name = "NE",
                    Role = UserRole.Student,
                    Username = "NE",
                    Group = "NE"
                }, am));
            }
            //变构
            _creations[0] = new()
            {
                Name = "TGG_Common_1",
                Parent = "TGG_Common_2"
            };
            Assert.IsTrue(await DefaultInstance.UpdateOrCreateAsync(_creations[0]));
            var scope = Dependency.GetScope();
            var sp = scope.ServiceProvider;
            am = sp.GetRequiredService<AccountManager>();
            db = sp.GetRequiredService<ChadDb>();
            CollectionAssert.Contains((await am.GetUsersAsync(group: "TGG_Common_2")).ToList()
                , await db.Users.FindAsync("TUG_Common_1"));
            //完成性
            Assert.AreEqual("TGG_Common_2",
                (await db.Groups.FindAsync("TGG_Common_1")).Parent);
            CollectionAssert.AreEquivalent(_creations, await DefaultInstance.GetGroupsAsync());
            //全称集合
            CollectionAssert.AreEquivalent(creationFullNamesU1, await DefaultInstance.GetGroupNamesAsync());
            //过滤器
            filters = _creations.Select(g => new Filter { Name = $"组织: {g.Name}", Expression = $"group={g.Name}" })
                .ToList();
            filters.Add(new() { Name = "无组织", Expression = $"group={Group.RootParent}" });
            CollectionAssert.AreEquivalent(filters, await DefaultInstance.GetGroupFiltersAsync());
            //用户响应
            CollectionAssert.Contains((await am.GetUsersAsync(group: "TGG_Common_2")).ToList()
                , await db.Users.FindAsync("TUG_Common_1"));

            scope.Dispose();

            //删除 叶节点

            await DefaultInstance.DeleteAsync("TGG_Common_1");
            scope = Dependency.GetScope();
            sp = scope.ServiceProvider;
            am = sp.GetRequiredService<AccountManager>();
            db = sp.GetRequiredService<ChadDb>();
            //完成性质
            Assert.IsNull(await db.Groups.FindAsync("TGG_Common_1"));
            CollectionAssert.AreEquivalent(_creations[1..].ToList(), await DefaultInstance.GetGroupsAsync());
            //全称集合
            CollectionAssert.AreEquivalent(creationFullNamesU2, await DefaultInstance.GetGroupNamesAsync());
            //过滤器
            filters = _creations[1..].Select(g => new Filter { Name = $"组织: {g.Name}", Expression = $"group={g.Name}" }).ToList();
            filters.Add(new() { Name = "无组织", Expression = $"group={Group.RootParent}" });
            CollectionAssert.AreEquivalent(filters, await DefaultInstance.GetGroupFiltersAsync());
            //用户响应
            CollectionAssert.Contains((await am.GetUsersAsync(group: "TGG_Common_2")).ToList()
                , await db.Users.FindAsync("TUG_Common_1"));
            scope.Dispose();
            //删除 非叶节点
            await DefaultInstance.DeleteAsync("TGG_Common_2");
            scope = Dependency.GetScope();
            sp = scope.ServiceProvider;
            am = sp.GetRequiredService<AccountManager>();
            db = sp.GetRequiredService<ChadDb>();
            //完成性质
            Assert.IsNull(await db.Groups.FindAsync("TGG_Common_2"));
            _creations[2] = new()
            {
                Name = "TGG_Common_21",
                Parent = Group.RootParent
            };
            _creations[3] = new()
            {
                Name = "TGG_Common_22",
                Parent = Group.RootParent
            };
            _creations[4] = new()
            {
                Name = "TGG_Common_23",
                Parent = Group.RootParent
            };
            CollectionAssert.AreEquivalent(_creations[2..].ToList(), await DefaultInstance.GetGroupsAsync());
            //全称集合
            CollectionAssert.AreEquivalent(creationFullNamesU3, await DefaultInstance.GetGroupNamesAsync());
            //过滤器
            filters = _creations[2..].Select(g => new Filter { Name = $"组织: {g.Name}", Expression = $"group={g.Name}" }).ToList();
            filters.Add(new() { Name = "无组织", Expression = $"group={Group.RootParent}" });
            CollectionAssert.AreEquivalent(filters, await DefaultInstance.GetGroupFiltersAsync());
            //用户响应
            CollectionAssert.Contains((await am.GetUsersAsync(group: Group.RootParent)).ToList()
                , await db.Users.FindAsync("TUG_Common_1"));
            scope.Dispose();
            //清除
            foreach (var creation in _creations[2..])
            {
                await DefaultInstance.DeleteAsync(creation.Name);
            }

            scope = Dependency.GetScope();
            sp = scope.ServiceProvider;
            am = sp.GetRequiredService<AccountManager>();
            foreach (var user in testUsers)
            {
                await am.DeleteAsync(user.Username);
                
            }

            scope.Dispose();
        }



        [TestMethod]
        public void GetDirectGroupClaimTest()
        {
            foreach (var creation in _creations)
            {
                Assert.AreEqual(new Claim(AccountManager.DirectGroupClaimType, creation.Name).ToString(),
                    GroupManager.GetDirectGroupClaim(creation.Name).ToString());
            }
        }


        [TestMethod]
        public void GroupCastTest()
        {
            foreach (var creation in _creations)
            {
                Assert.AreEqual(creation, GroupManager.GroupCast(
                    new DbGroup
                    {
                        Name = creation.Name,
                        Parent = creation.Parent == Group.RootParent ? null : creation.Parent
                    }));
            }
        }


    }
}