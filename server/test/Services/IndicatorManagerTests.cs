using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chad.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models.Assessment;
using Chad.Models.Audit;
using Chad.Models.Common;
using Chad.Models.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Chad.Services.Tests
{
    [TestClass]
    public class IndicatorManagerTests : TestBase<IndicatorManager>
    {
        [TestMethod]
        public void IndicatorManagerTest()
        {
            _ = GetInstance();
        }

        public static string[] FieldsImpl { get; } =
        {
            "1970-1-1",
            "1",
            "1",
            "S1",
            "TEXT"
        };
        public static IndicatorField[] Fields { get; } =
        {
            new()
            {
                Name = "TFI_LT_0",
                Description = "TFI_LT_0",
                Type = IndicatorFieldType.Date,
                Value = ""
            },
            new()
            {
                Name = "TFI_LT_1",
                Description = "TFI_LT_1",
                Type = IndicatorFieldType.Number,
                Value = "0"
            },
            new()
            {
                Name = "TFI_LT_2",
                Description = "TFI_LT_2",
                Type = IndicatorFieldType.Switch,
                Value = "0"
            },
            new()
            {
                Name = "TFI_LT_3",
                Description = "TFI_LT_3",
                Type = IndicatorFieldType.Selections,
                Value = "S1|S2|S3"
            },
            new()
            {
                Name = "TFI_LT_4",
                Description = "TFI_LT_4",
                Type = IndicatorFieldType.Text,
                Value = "DEFAULT_TEXT"
            },
        };

        public static ManagedIndicator[] Creations { get; } =
        {
            new()
            {
                Name="TEI_LT_0",
                Description="TEI_LT_0",
                CountLimit=0,
                DefaultTtl=1,
                Id=Guid.NewGuid(),
                Published=false,
                Type=IndicatorType.Event,
                Fields=Array.Empty<IndicatorField>()
            },
            new()
            {
                Name="TEI_LT_1",
                Description="TEI_LT_1",
                CountLimit=1,
                DefaultTtl=0,
                Id=Guid.NewGuid(),
                Published=false,
                Type=IndicatorType.Event,
                Fields=Array.Empty<IndicatorField>()
            },
            new()
            {
                Name = "TPI_LT_0",
                Description = "TPI_LT_0",
                CountLimit = 0,
                DefaultTtl = 0,
                Id = Guid.NewGuid(),
                Published = false,
                Type = IndicatorType.Property,
                Fields = Array.Empty<IndicatorField>()
            },
            new()
            {
                Name = "TPI_LT_1",
                Description = "TPI_LT_1",
                CountLimit = 1,
                DefaultTtl = 0,
                Id = Guid.NewGuid(),
                Published = false,
                Type = IndicatorType.Property,
                Fields = Array.Empty<IndicatorField>()
            }
        };

        [TestMethod]
        public async Task IndicatorLifeTimeTest()
        {
            // 初始化
            using var scope = Dependency.GetScope();
            var am = scope.ServiceProvider.GetRequiredService<AccountManager>();
            var im = scope.ServiceProvider.GetRequiredService<IndicatorManager>();
            var db = scope.ServiceProvider.GetRequiredService<ChadDb>();
            var sys = scope.ServiceProvider.GetRequiredService<SystemManager>();
            var judgeCre = await am.CreateAsync(new()
            {
                Username = "TUI_JUDGE",
                Name = "TUI_JUDGE",
                Group = Group.RootParent,
                Role = UserRole.Teacher,
                InitialPassword = GetRandomString()
            });
            var judge = (await am.GetUsersAsync(UserRole.Teacher)).First();
            var baseCre = await am.CreateAsync(new()
            {
                Username = "TUI_BASE",
                Name = "TUI_BASE",
                Group = Group.RootParent,
                Role = UserRole.Student,
                InitialPassword = GetRandomString()
            });
            var @base = (await am.GetUsersAsync(UserRole.Student)).First();
            var baseIv = new IndicatorValue[]
            {
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                }
            };
            var baseIvA = new IndicatorValue[]
            {
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[0].Id,
                    Status = ValueStatus.Checked
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[1].Id,
                    Status = ValueStatus.Checked
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[2].Id,
                    Status = ValueStatus.Rejected
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[3].Id,
                    Status = ValueStatus.Rejected
                }
            };
            var baseIvB = new IndicatorValue[]
            {
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[0].Id,
                    Status = ValueStatus.Checked
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[1].Id,
                    Status = ValueStatus.Checked
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[2].Id,
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = baseIv[3].Id,
                    Status = ValueStatus.Pending
                }
            };
            var judgeIv = new IndicatorValue[]
            {
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                },
                new()
                {
                    Values = FieldsImpl,
                    Id = Guid.NewGuid(),
                    Status = ValueStatus.Pending
                }
            };


            foreach (var creation in Creations)
            {
                #region 创建
                CollectionAssert.AreEquivalent(await im.GetIndicatorsAsync(),
                    Array.Empty<ManagedIndicator>());
                Assert.IsNull(await im.GetIndicatorByIdAsync(creation.Id));
                Assert.IsTrue(await im.UpdateOrCreateAsync(creation, null));
                var created = await im.GetIndicatorByIdAsync(creation.Id);
                Assert.IsNotNull(created);

                CollectionAssert.Contains(await im.GetIndicatorsAsync(), creation);
                CollectionAssert.AreEquivalent(await im.GetAuditIndicatorsAsync(AuditMode.Pending),
                    Array.Empty<ManagedIndicator>());
                CollectionAssert.AreEquivalent(await im.GetAuditIndicatorsAsync(AuditMode.Finished),
                    Array.Empty<ManagedIndicator>());
                Assert.IsNull(await im.GetUserIndicatorAsync(created, @base));
                Assert.IsNull(await im.GetUserIndicatorAsync(created, judge));
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                    () => im.GetAuditIndicatorAsync(AuditMode.Pending, created, true,judge));
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                    () => im.GetAuditIndicatorAsync(AuditMode.Finished, created, true, judge));
                #endregion
                #region 变构

                Assert.IsTrue(await im.UpdateOrCreateAsync(creation with { Name = creation.Name + "ALT" }, created));
                Assert.AreEqual(creation.Name + "ALT", created.Name);
                Assert.IsTrue(await im.UpdateOrCreateAsync(creation with { Description = creation.Description + "ALT" }, created));
                Assert.AreEqual(creation.Description + "ALT", created.Description);
                Assert.IsTrue(await im.UpdateOrCreateAsync(creation with { DefaultTtl = (ushort)(creation.DefaultTtl + 1) }, created));
                Assert.AreEqual((ushort)(creation.DefaultTtl + 1), created.DefaultTtl);
                Assert.IsTrue(await im.UpdateOrCreateAsync(creation with { CountLimit = (byte)(creation.CountLimit + 1) }, created));
                Assert.AreEqual((byte)(creation.CountLimit + 1), created.CountLimit);
                Assert.IsTrue(await im.UpdateOrCreateAsync(creation with
                {
                    Type = creation.Type switch
                    {
                        IndicatorType.Event => IndicatorType.Property,
                        _ => IndicatorType.Event
                    }
                }, created));
                Assert.AreEqual(creation.Type switch
                {
                    IndicatorType.Event => IndicatorType.Property,
                    _ => IndicatorType.Event
                }, created.Type);
                Assert.IsTrue(await im.UpdateOrCreateAsync(creation with { Type = creation.Type }, created));
                var newCreation = creation with { Fields = Fields };
                Assert.IsTrue(await im.UpdateOrCreateAsync(newCreation, created));
                CollectionAssert.AreEqual(Fields, created.Fields.Select(f => new IndicatorField
                {
                    Description = f.Description,
                    Name = f.Name,
                    Type = f.Type,
                    Value = f.Value
                }).ToArray());
                Assert.AreEqual(newCreation, (await im.GetIndicatorsAsync()).First());
                #endregion
                #region 发布
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                    im.UpdateValuesAsync(baseIv, created, @base));
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                    im.UpdateValuesAsync(judgeIv, created, judge));
                //发布
                Assert.IsTrue(await im.DeployAsync(created));
                //审核集合仍然为空
                CollectionAssert.AreEquivalent(await im.GetAuditIndicatorsAsync(AuditMode.Pending, judge),
                    Array.Empty<ManagedIndicator>());
                CollectionAssert.AreEquivalent(await im.GetAuditIndicatorsAsync(AuditMode.Finished, judge),
                    Array.Empty<ManagedIndicator>());
                Assert.IsNotNull(await im.GetUserIndicatorAsync(created, @base));
                Assert.IsNotNull(await im.GetUserIndicatorAsync(created, judge));
                Assert.IsNotNull(await im.GetAuditIndicatorAsync(AuditMode.Pending, created, true, judge));
                Assert.IsNotNull(await im.GetAuditIndicatorAsync(AuditMode.Finished, created, true, judge));
                //发布后无法变构
                Assert.IsFalse(await im.UpdateOrCreateAsync(creation with { Name = creation.Name + "ALT" }, created));
                #endregion
                #region 填写
                await im.UpdateValuesAsync(baseIv, created, @base);
                await im.UpdateValuesAsync(judgeIv, created, judge);
                var baseInd = await im.GetUserIndicatorAsync(created, @base);
                Assert.IsNotNull(baseInd);
                var judgeInd = await im.GetUserIndicatorAsync(created, judge);
                Assert.IsNotNull(judgeInd);

                CollectionAssert.AreEquivalent(baseIv,
                    baseInd.Values);
                CollectionAssert.AreEquivalent(judgeIv,
                    judgeInd.Values);
                switch (created.Type)
                {
                    case IndicatorType.Event:
                        foreach (var value in baseIv)
                        {
                            Assert.AreEqual(sys.SystemTime,
                                (await db.Events.FirstAsync(e => e.Id == value.Id))
                                .EffectDomainId);
                        }
                        break;
                    case IndicatorType.Property:
                        foreach (var value in baseIv)
                        {
                            Assert.AreEqual(created.DefaultTtl,
                                (await db.Properties.FirstAsync(e => e.Id == value.Id))
                                .Ttl);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //审核的指标值
                var judgeAuInd = await im.GetAuditIndicatorAsync(AuditMode.Finished, created, true, judge);
                //没有审核完毕的值
                Assert.IsNotNull(judgeAuInd);
                CollectionAssert.AreEquivalent(Array.Empty<AuditIndicatorValue>(), judgeAuInd.Values);
                CollectionAssert.AreEquivalent(Array.Empty<AuditIndicator>(),
                    await im.GetAuditIndicatorsAsync(AuditMode.Finished, judge));
                //只有未审核的值，都是来自base的
                judgeAuInd = await im.GetAuditIndicatorAsync(AuditMode.Pending, created, true, judge);
                Assert.IsNotNull(judgeAuInd);
                CollectionAssert.AreEquivalent(baseIv.Select(
                    iv => new AuditIndicatorValue
                    {
                        Id = iv.Id,
                        Status = iv.Status,
                        Values = iv.Values,
                        OwnerExpression = $"{@base.FriendlyName}(无组织)",
                    }).ToArray(), judgeAuInd.Values);
                CollectionAssert.Contains(await im.GetAuditIndicatorsAsync(AuditMode.Pending, judge), judgeAuInd);
                //审核，前两个被通过、后两个被拒绝
                await im.AuditValuesAsync(ValueStatus.Checked, new[]
                {
                    baseIv[0].Id,
                    baseIv[1].Id
                });
                await im.AuditValuesAsync(ValueStatus.Rejected, new[]
                {
                    baseIv[2].Id,
                    baseIv[3].Id
                });
                baseInd = await im.GetUserIndicatorAsync(created, @base);
                Assert.IsNotNull(baseInd);
                CollectionAssert.AreEquivalent(baseIvA, baseInd.Values);
                //没用未审核的值
                judgeAuInd = await im.GetAuditIndicatorAsync(AuditMode.Pending, created, true, judge);
                Assert.IsNotNull(judgeAuInd);
                CollectionAssert.AreEquivalent(Array.Empty<AuditIndicatorValue>(), judgeAuInd.Values);
                CollectionAssert.AreEquivalent(Array.Empty<AuditIndicator>(),
                    await im.GetAuditIndicatorsAsync(AuditMode.Pending, judge));
                //只有审核完毕的值，都是来自base的
                judgeAuInd = await im.GetAuditIndicatorAsync(AuditMode.Finished, created, true, judge);
                Assert.IsNotNull(judgeAuInd);
                CollectionAssert.AreEquivalent(baseIvA.Select(
                    iv => new AuditIndicatorValue
                    {
                        Id = iv.Id,
                        Status = iv.Status,
                        Values = iv.Values,
                        OwnerExpression = $"{@base.FriendlyName}(无组织)",
                    }).ToArray(), judgeAuInd.Values);
                CollectionAssert.Contains(await im.GetAuditIndicatorsAsync(AuditMode.Finished, judge), judgeAuInd);
                //用户修改值
                await im.UpdateValuesAsync(baseIvB, created, @base);
                await im.AuditValuesAsync(ValueStatus.Pending, new[]
                {
                    baseIv[0].Id,
                    baseIv[1].Id
                });
                baseInd = await im.GetUserIndicatorAsync(created, @base);
                Assert.IsNotNull(baseInd);
                CollectionAssert.AreEquivalent(baseIv, baseInd.Values);
                //没有审核完毕的值
                judgeAuInd = await im.GetAuditIndicatorAsync(AuditMode.Finished, created, true, judge);
                Assert.IsNotNull(judgeAuInd);
                CollectionAssert.AreEquivalent(Array.Empty<AuditIndicatorValue>(), judgeAuInd.Values);
                CollectionAssert.AreEquivalent(Array.Empty<AuditIndicator>(),
                    await im.GetAuditIndicatorsAsync(AuditMode.Finished, judge));
                //只有未审核的值，都是来自base的
                judgeAuInd = await im.GetAuditIndicatorAsync(AuditMode.Pending, created, true, judge);
                Assert.IsNotNull(judgeAuInd);
                CollectionAssert.AreEquivalent(baseIv.Select(
                    iv => new AuditIndicatorValue
                    {
                        Id = iv.Id,
                        Status = iv.Status,
                        Values = iv.Values,
                        OwnerExpression = $"{@base.FriendlyName}(无组织)",
                    }).ToArray(), judgeAuInd.Values);
                CollectionAssert.Contains(await im.GetAuditIndicatorsAsync(AuditMode.Pending, judge), judgeAuInd);
                #endregion
                #region 删除
                await im.DeleteAsync(creation.Id);
                Assert.IsNull(await im.GetIndicatorByIdAsync(creation.Id));
                CollectionAssert.AreEquivalent(await im.GetIndicatorsAsync(),
                    Array.Empty<ManagedIndicator>());
                #endregion
            }

            //清除

            await am.DeleteAsync("TUI_JUDGE");
            await am.DeleteAsync("TUI_BASE");
        }


    }
}