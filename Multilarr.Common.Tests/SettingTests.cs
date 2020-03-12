using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Tests.Factories;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
            const string appId = "The appId";
            const string key = "The key";
            const string secret = "The secret";
            const string cluster = "The cluster";

            // Arrange
            var settingList = SettingModelFactory.Default(appId, key, secret, cluster);
            var taskSettingList = Task.FromResult(settingList);
            _mockILogger.Setup(x => x.GetSettingsAsync()).Returns(taskSettingList);
            _setting = new Setting(_mockILogger.Object);

            // Act
            await _setting.PopulateSetting();

            // Assert
            Assert.AreEqual(appId, _setting.AppId);
            Assert.AreEqual(key, _setting.Key);
            Assert.AreEqual(secret, _setting.Secret);
            Assert.AreEqual(cluster, _setting.Cluster);
        }

        [TestMethod]
        public async Task PopulateSetting_NoSetting()
        {
            // Arrange
            _setting = new Setting(_mockILogger.Object);

            // Act
            await _setting.PopulateSetting();

            // Assert
            _mockILogger.Verify(x => x.LogWarnAsync("No settings saved."), Times.Once);
        }

        [TestMethod]
        public async Task PopulateSetting_NoDefaultSetting()
        {
            const string appId = "The appId";
            const string key = "The key";
            const string secret = "The secret";
            const string cluster = "The cluster";

            // Arrange
            var settingList = SettingModelFactory.CreateSettingModels(5, appId, key, secret, cluster, false);
            var taskSettingList = Task.FromResult(settingList);
            _mockILogger.Setup(x => x.GetSettingsAsync()).Returns(taskSettingList);
            _setting = new Setting(_mockILogger.Object);

            // Act
            await _setting.PopulateSetting();

            // Assert
            _mockILogger.Verify(x => x.LogWarnAsync("No default setting saved."), Times.Once);
        }

        [TestMethod]
        public async Task PopulateSetting_Config()
        {
            const string appId = "The appId";
            const string key = "The key";
            const string secret = "The secret";
            const string cluster = "The cluster";

            // Arrange
            var settingDictionary = new Dictionary<string, string>
            {
                { "PusherAppId", appId },
                { "PusherKey", key },
                { "PusherSecret", secret },
                { "PusherCluster", cluster }
            };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settingDictionary).Build();

            _setting = new Setting(_mockILogger.Object, configuration);

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