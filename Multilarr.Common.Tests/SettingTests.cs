﻿using System.Collections.Generic;
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
            // Arrange
            const string appId = "The appId";
            const string key = "The key";
            const string secret = "The secret";
            const string cluster = "The cluster";

            var settingLogList = SettingLogFactory.CreateSettingLogs(5, appId, key, secret, cluster, false);
            var taskSettingLogList = Task.FromResult(settingLogList);
            _mockILogger.Setup(x => x.GetSettingLogsAsync()).Returns(taskSettingLogList);
            _setting = new Setting(_mockILogger.Object);

            // Act
            await _setting.PopulateSetting();

            // Assert
            _mockILogger.Verify(x => x.LogWarnAsync("No default setting saved."), Times.Once);
        }

        [TestMethod]
        public async Task PopulateSetting_Config()
        {
            // Arrange
            const string appId = "The appId";
            const string key = "The key";
            const string secret = "The secret";
            const string cluster = "The cluster";

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