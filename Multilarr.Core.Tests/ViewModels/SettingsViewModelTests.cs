using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Helpers;
using Multilarr.Core.Tests.Factories;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class SettingsViewModelTests
    {
        private const string Title = "Settings";
        private Mock<ILogger> _mockILogger;
        private Mock<IPusherValidation> _mockIPusherValidation;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;
        private Mock<INavigationPushModalHelper> _mockINavigationPushModalHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIPusherValidation = new Mock<IPusherValidation>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
            _mockINavigationPushModalHelper = new Mock<INavigationPushModalHelper>();
        }

        [TestMethod]
        public void SettingsViewModel()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
        }

        [TestMethod]
        public void LoadItemsCommand()
        {
            const int noOfSettings = 5;

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).ReturnsAsync(SettingModelFactory.CreateSettingModels(noOfSettings));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object);

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.IsNotNull(settingsViewModel.Settings);
            Assert.AreEqual(noOfSettings, settingsViewModel.Settings.Count);
        }

        [TestMethod]
        public void LoadItemsCommand_Exception()
        {
            const string exceptionMessage = "Error on GetSettingsAsync!";

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).Throws(new Exception(exceptionMessage));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object);

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.AreEqual(0, settingsViewModel.Settings.Count);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}