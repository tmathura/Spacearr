using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Tests.Factories;
using Spacearr.Core.Xamarin.ViewModels;
using System;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class SettingsViewModelTests
    {
        private const string Title = "Settings";
        private Mock<ILogger> _mockILogger;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;
        private Mock<INavigationPushModalHelper> _mockINavigationPushModalHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
            _mockINavigationPushModalHelper = new Mock<INavigationPushModalHelper>();
        }

        [TestMethod]
        public void SettingsViewModel()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object, It.IsAny<Page>());

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
        }

        [TestMethod]
        public void LoadItemsCommand()
        {
            const int noOfSettings = 5;

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).ReturnsAsync(SettingModelFactory.CreateSettingModels(noOfSettings));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object, It.IsAny<Page>());

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
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object, It.IsAny<Page>());

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.AreEqual(0, settingsViewModel.Settings.Count);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }

        [TestMethod]
        public void AddCommand()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object, It.IsAny<Page>());

            // Act
            settingsViewModel.AddCommand.Execute(null);

            // Assert
            _mockINavigationPushModalHelper.Verify(x => x.CustomPushModalAsync(It.IsAny<Page>()), Times.Once);
        }

        [TestMethod]
        public void AddCommand_Exception()
        {
            const string exceptionMessage = "Error on CustomPushModalAsync!";

            // Arrange
            _mockINavigationPushModalHelper.Setup(x => x.CustomPushModalAsync(It.IsAny<Page>())).Throws(new Exception(exceptionMessage));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockINavigationPushModalHelper.Object, It.IsAny<Page>());

            // Act
            settingsViewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}