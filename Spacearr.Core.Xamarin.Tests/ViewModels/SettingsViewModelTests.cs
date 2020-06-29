using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
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
        private Mock<ISettingsPageHelper> _mockISettingsPageHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockISettingsPageHelper = new Mock<ISettingsPageHelper>();
        }

        [TestMethod]
        public void SettingsViewModel()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, It.IsAny<Page>());

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
        }

        [TestMethod]
        public void LoadItemsCommand()
        {
            const int noOfSettings = 5;

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).ReturnsAsync(SettingModelFactory.CreateSettingModels(noOfSettings));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, It.IsAny<Page>());

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
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, It.IsAny<Page>());

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.AreEqual(0, settingsViewModel.Settings.Count);
            _mockISettingsPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }

        [TestMethod]
        public void AddCommand()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, It.IsAny<Page>());

            // Act
            settingsViewModel.AddCommand.Execute(null);

            // Assert
            _mockISettingsPageHelper.Verify(x => x.CustomPushModalAsync(It.IsAny<Page>()), Times.Once);
        }

        [TestMethod]
        public void AddCommand_Exception()
        {
            const string exceptionMessage = "Error on CustomPushModalAsync!";

            // Arrange
            _mockISettingsPageHelper.Setup(x => x.CustomPushModalAsync(It.IsAny<Page>())).Throws(new Exception(exceptionMessage));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, It.IsAny<Page>());

            // Act
            settingsViewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            _mockISettingsPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}