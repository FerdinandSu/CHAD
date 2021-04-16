using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chad.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models.Assessment;
using Chad.Models.Common;
using Chad.Models.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Chad.Services.Tests
{
    [TestClass]
    public class FormManagerTests : TestBase<FormManager>
    {
        private static decimal _weight = 1.0M;
        private static decimal GetWeight()
        {
            _weight *= 2;
            return _weight;
        }

        public static ManagedIndicatorSummary[] Indicators = IndicatorManagerTests.Creations.Select(
            i => new ManagedIndicatorSummary
            {
                Id = i.Id,
                CountLimit = i.CountLimit,
                Description = i.Description,
                Name = i.Name,
                Weight = GetWeight()
            }).ToArray();

        public static ManagedAssessmentForm TestForm = new()
        {
            Id = Guid.NewGuid(),
            Name = "TFF_DEFAULT",
            Status = AssessmentFormStatus.Designing,
            Description = "TFF_DEFAULT",
            Indicators = Array.Empty<ManagedIndicatorSummary>()
        };

        [TestMethod]
        public void FormManagerTest()
        {
            _ = GetInstance();
        }

        private async Task TestRefactor(ManagedAssessmentForm form, DbAssessmentForm created, FormManager fm)
        {
            Assert.IsTrue(await fm.UpdateOrCreateAsync(form, created));
            Assert.AreEqual(form, (await fm.GetManagedFormsAsync()).First());
        }

        [TestMethod]
        public async Task FormLifeTimeTest()
        {
            #region 初始化
            using var scope = Dependency.GetScope();
            var am = scope.ServiceProvider.GetRequiredService<AccountManager>();
            var im = scope.ServiceProvider.GetRequiredService<IndicatorManager>();
            var fm = scope.ServiceProvider.GetRequiredService<FormManager>();
            var db = scope.ServiceProvider.GetRequiredService<ChadDb>();
            var sys = scope.ServiceProvider.GetRequiredService<SystemManager>();
            var judgeCre = await am.CreateAsync(new()
            {
                Username = "TUF_JUDGE",
                Name = "TUF_JUDGE",
                Group = Group.RootParent,
                Role = UserRole.Teacher,
                InitialPassword = GetRandomString()
            });
            var judge = (await am.GetUsersAsync(UserRole.Teacher)).First();
            var baseCre = await am.CreateAsync(new()
            {
                Username = "TUF_BASE",
                Name = "TUF_BASE",
                Group = Group.RootParent,
                Role = UserRole.Student,
                InitialPassword = GetRandomString()
            });
            var @base = (await am.GetUsersAsync(UserRole.Student)).First();
            foreach (var creation in IndicatorManagerTests.Creations)
            {
                await im.UpdateOrCreateAsync(creation, null);
                var ind = await im.GetIndicatorByIdAsync(creation.Id);
                Assert.IsNotNull(ind);
                await im.DeployAsync(ind);
            }

            var tfSummary = new ElementSummary
            {
                Description = TestForm.Description,
                Id = TestForm.Id,
                Name = TestForm.Name
            };

            var asf = new AssessmentForm
            {
                Id = TestForm.Id,
                Description = TestForm.Description,
                Name = TestForm.Name,
                Indicators = Array.Empty<Indicator>()
            };

            var indv = new Indicator[4];
            for (int i = 0; i < 4; i++)
            {
                indv[i] = new()
                {
                    Description = Indicators[i].Description,
                    CountLimit = Indicators[i].CountLimit,
                    Fields = IndicatorManagerTests.Creations[i].Fields,
                    Name = IndicatorManagerTests.Creations[i].Name,
                    Id = IndicatorManagerTests.Creations[i].Id,
                    Values = new IndicatorValue[]
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Status = ValueStatus.Pending,
                            Values = IndicatorManagerTests.FieldsImpl
                        }
                    }
                };
            }

            var indvB = new Indicator[4];
            for (var i = 0; i < 4; i++)
            {
                indvB[i] = new()
                {
                    Description = Indicators[i].Description,
                    CountLimit = Indicators[i].CountLimit,
                    Fields = IndicatorManagerTests.Creations[i].Fields,
                    Name = IndicatorManagerTests.Creations[i].Name,
                    Id = IndicatorManagerTests.Creations[i].Id,
                    Values = new IndicatorValue[]
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Status = ValueStatus.Pending,
                            Values = IndicatorManagerTests.FieldsImpl
                        }
                    }
                };
            }

            indv = indv.OrderBy(i => i.Name).ToArray();
            indvB = indvB.OrderBy(i => i.Name).ToArray();


            var rankResult = new RankListItem[]
            {
                new()
                {
                    Group = Group.RootParent,
                    Name = @base.FriendlyName,
                    Score = 30.0M
                },
                new()
                {
                    Group = Group.RootParent,
                    Name = judge.FriendlyName,
                    Score = 24.0M
                }
            };

            #endregion
            #region 创建

            Assert.IsNull(await fm.GetFormByIdAsync(TestForm.Id));
            CollectionAssert.AreEquivalent(Array.Empty<ManagedAssessmentForm>(),
                await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Published));

            //创建
            Assert.IsTrue(await fm.UpdateOrCreateAsync(TestForm, null));
            var created = await fm.GetFormByIdAsync(TestForm.Id);
            Assert.IsNotNull(created);
            CollectionAssert.AreEquivalent(new[] { TestForm }, await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(new[] { tfSummary }, await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Published));

            Assert.AreEqual(asf, await fm.GetUserFormAsync(created));
            #endregion
            #region 变构

            await TestRefactor(TestForm with { Name = TestForm.Name + "ALT" }, created, fm);
            await TestRefactor(TestForm with { Description = TestForm.Description + "ALT" }, created, fm);
            var ntf = TestForm with { Status = AssessmentFormStatus.Published };
            Assert.IsTrue(await fm.UpdateOrCreateAsync(ntf, created));
            Assert.AreNotEqual(ntf, (await fm.GetManagedFormsAsync()).First());
            ntf = TestForm with { Indicators = Indicators };
            await TestRefactor(ntf, created, fm);
            #endregion
            #region 发布

            ntf = ntf with { Status = AssessmentFormStatus.Published };

            //3种非法操作
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Lock, created));
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Unlock, created));
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Reset, created));

            Assert.IsTrue(await fm.UpdateAsync(ActionType.Deploy, created));

            CollectionAssert.AreEquivalent(new[] { ntf }, await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(new[] { tfSummary },
                await fm.GetFormsAsync(AssessmentFormStatus.Published));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEquivalent(Array.Empty<RankListItem>(),
                await fm.GetResultsAsync(created, null));



            //发布态无法更新表本身
            Assert.IsFalse(await fm.UpdateOrCreateAsync(ntf, created));

            await fm.UpdateValuesAsync(indv.ToDictionary(i => i.Id), created, @base);
            await fm.UpdateValuesAsync(indvB.ToDictionary(i => i.Id), created, judge);



            Assert.AreEqual(asf with { Indicators = indv }, await fm.GetUserFormAsync(created, @base));

            //撤回

            ntf = ntf with { Status = AssessmentFormStatus.Designing };

            Assert.IsFalse(await fm.UpdateAsync(ActionType.Deploy, created));
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Unlock, created));
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Reset, created));

            Assert.IsTrue(await fm.UpdateAsync(ActionType.Withdraw, created));

            CollectionAssert.AreEquivalent(new[] { ntf }, await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(new[] { tfSummary },
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Published));
            CollectionAssert.AreEquivalent(Array.Empty<RankListItem>(),
                await fm.GetResultsAsync(created, null));

            //指标们不再可以查询
            Assert.AreEqual(asf, await fm.GetUserFormAsync(created));

            //但是指标值仍存在，还可以被审核; 审核结果会被后文引用并确认。
            CollectionAssert.AreNotEquivalent(Array.Empty<ManagedIndicator>(),
                (await im.GetIndicatorsAsync()));
            CollectionAssert.AreNotEquivalent(Array.Empty<IndicatorValue>(),
                (await im.GetValuesAsync(db.Indicators.First(), @base)).ToList());
            await im.AuditValuesAsync(ValueStatus.Checked,
                indv.Select(i => i.Values.First().Id).ToArray());
            await im.AuditValuesAsync(ValueStatus.Checked,
                indvB[2..].Select(i => i.Values.First().Id).ToArray());

            indv = indv.Select(ind => ind with
            {
                Values =
                ind.Values.Select(
                    iv => iv with
                    {
                        Status = ValueStatus.Checked
                    }).ToArray()
            }).ToArray();

            //撤回后可以变构
            Assert.IsTrue(await fm.UpdateOrCreateAsync(ntf, created));

            //再发布

            ntf = ntf with { Status = AssessmentFormStatus.Published };
            Assert.IsTrue(await fm.UpdateAsync(ActionType.Deploy, created));

            //指标值可再次查询

            Assert.AreEqual(asf with { Indicators = indv }, await fm.GetUserFormAsync(created, @base));

            #endregion
            #region 评估

            ntf = ntf with { Status = AssessmentFormStatus.Evaluated };

            Assert.IsTrue(await fm.UpdateAsync(ActionType.Lock, created));

            //表锁定，指标无法查询、更改
            Assert.AreEqual(asf, await fm.GetUserFormAsync(created, @base));

            CollectionAssert.AreEquivalent(new[] { ntf }, await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(new[] { tfSummary },
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Published));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));

            CollectionAssert.AreEqual(rankResult, await fm.GetResultsAsync(created, null));

            //组过滤器
            CollectionAssert.AreEqual(rankResult, await fm.GetResultsAsync(created, Group.RootParent));
            CollectionAssert.AreEqual(Array.Empty<RankListItem>(),
                await fm.GetResultsAsync(created, "NEG"));

            //指标审核结果会改变，但是不会影响已经评估了的表
            await im.AuditValuesAsync(ValueStatus.Pending,
                indvB[2..].Select(i => i.Values.First().Id).ToArray());



            CollectionAssert.AreEqual(rankResult, await fm.GetResultsAsync(created, null));

            //解锁

            ntf = ntf with { Status = AssessmentFormStatus.Published };

            Assert.IsTrue(await fm.UpdateAsync(ActionType.Unlock, created));

            //表解锁，指标可以查询、更改
            Assert.AreEqual(asf with { Indicators = indv }, await fm.GetUserFormAsync(created, @base));

            CollectionAssert.AreEquivalent(new[] { ntf }, await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(new[] { tfSummary },
                await fm.GetFormsAsync(AssessmentFormStatus.Published));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEqual(Array.Empty<RankListItem>(),
                await fm.GetResultsAsync(created, null));

            //再发布

            ntf = ntf with { Status = AssessmentFormStatus.Evaluated };

            Assert.IsTrue(await fm.UpdateAsync(ActionType.Lock, created));

            //评估结果改变
            CollectionAssert.AreEqual(rankResult[..^1], await fm.GetResultsAsync(created, null));

            #endregion
            #region 重利用
            ntf = ntf with { Status = AssessmentFormStatus.Designing };

            Assert.IsFalse(await fm.UpdateAsync(ActionType.Deploy, created));
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Lock, created));
            Assert.IsFalse(await fm.UpdateAsync(ActionType.Withdraw, created));

            Assert.IsTrue(await fm.UpdateAsync(ActionType.Reset, created));

            CollectionAssert.AreEquivalent(new[] { ntf }, await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(new[] { tfSummary },
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Published));

            //结果不再可用
            CollectionAssert.AreEquivalent(Array.Empty<RankListItem>(),
                await fm.GetResultsAsync(created, null));

            CollectionAssert.AreEqual(Array.Empty<DbAssessmentResult>(), created.Results);
            #endregion

            #region 删除

            await fm.DeleteAsync(created.Id);
            Assert.IsNull(await fm.GetFormByIdAsync(TestForm.Id));
            CollectionAssert.AreEquivalent(Array.Empty<ManagedAssessmentForm>(),
                await fm.GetManagedFormsAsync());
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Designing));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Evaluated));
            CollectionAssert.AreEquivalent(Array.Empty<ElementSummary>(),
                await fm.GetFormsAsync(AssessmentFormStatus.Published));

            //指标未被删除
            CollectionAssert.AreNotEqual(Array.Empty<DbIndicator>(),
                db.Indicators.ToArray());

            #endregion

            #region 清除

            await am.DeleteAsync("TUF_JUDGE");
            await am.DeleteAsync("TUF_BASE");

            foreach (var creation in IndicatorManagerTests.Creations)
            {
                await im.DeleteAsync(creation.Id);
            }

            #endregion

        }


    }
}