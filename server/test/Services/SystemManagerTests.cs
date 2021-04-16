using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chad.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chad.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Chad.Services.Tests
{
    [TestClass]
    public class SystemManagerTests:TestBase<SystemManager>
    {
        [TestMethod]
        public void SystemManagerTest()
        {
            _ = GetInstance();
        }

        [TestMethod]
        public async Task SystemLifeTimeTest()
        {
            //当前系统是可用状态，因此从新周期开始测试

            #region 环境准备
            using var scope = Dependency.GetScope();
            var am = scope.ServiceProvider.GetRequiredService<AccountManager>();
            var im = scope.ServiceProvider.GetRequiredService<IndicatorManager>();
            var db = scope.ServiceProvider.GetRequiredService<ChadDb>();
            var sys = scope.ServiceProvider.GetRequiredService<SystemManager>();


            #endregion

            #region 新周期

            var lastCyc = sys.SystemTime;
            await sys.NextCycleAsync();

            Assert.AreEqual(lastCyc + 1, sys.SystemTime);

            #endregion

            #region 重置
            Assert.IsTrue(sys.Initialized);
            await sys.ResetAsync();
            Assert.IsFalse(sys.Initialized);
            #endregion

            #region 初始化


            await sys.InitializeAsync();
            Assert.AreEqual(0, sys.SystemTime);
            Assert.IsTrue(sys.Initialized);

            #endregion


        }


    }
}