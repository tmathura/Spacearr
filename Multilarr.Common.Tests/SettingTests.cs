using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Tests.Factories;
using System.Threading.Tasks;

namespace Multilarr.Common.Tests
{
    [TestClass]
    public class SettingTests
    {
        private Mock<ILogger> _mockILogger;
        private Setting _setting;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
        }

        [TestMethod]
        public async Task PopulateSetting()
        {
            // Arrange
            const string appId = "The appId";
            const string key = "The key";
            const string secret = "The secret";
            const string cluster = "The cluster";

            var settingLogList = SettingLogFactory.Default(appId, key, secret, cluster);
            var taskSettingLogList = Task.FromResult(settingLogList);
            _mockILogger.Setup(x => x.GetSettingLogsAsync()).Returns(taskSettingLogList);
            _setting = new Setting(_mockILogger.Object);

            // Act
            await _setting.PopulateSetting();

            // Assert
            Assert.AreEqual(appId, _setting.AppId);
            Assert.AreEqual(key, _setting.Key);
            Assert.AreEqual(secret, _setting.Secret);
            Assert.AreEqual(cluster, _setting.Cluster);
        }
    }
}