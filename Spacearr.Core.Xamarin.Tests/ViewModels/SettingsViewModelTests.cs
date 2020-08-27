using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.Tests.Factories;
using Spacearr.Core.Xamarin.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Spacearr.Common.Models;
using Spacearr.Pusher.API.Services.Interfaces;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class SettingsViewModelTests
    {
        private const string Title = "Settings";
        private Mock<ILogger> _mockILogger;
        private Mock<ISettingsPageHelper> _mockISettingsPageHelper;
        private Mock<IGetWorkerServiceVersionService> _mockIGetWorkerServiceVersionService;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockISettingsPageHelper = new Mock<ISettingsPageHelper>();
            _mockIGetWorkerServiceVersionService = new Mock<IGetWorkerServiceVersionService>();
        }

        [TestMethod]
        public void SettingsViewModel()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
        }

        [TestMethod]
        public async Task LoadItemsCommand()
        {
            const int noOfSettings = 5;

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).ReturnsAsync(SettingModelFactory.CreateSettingModels(noOfSettings));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            await Task.Delay(1500);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.IsNotNull(settingsViewModel.Settings);
            Assert.AreEqual(noOfSettings, settingsViewModel.Settings.Count);
        }

        [TestMethod]
        public async Task LoadItemsCommand_CheckVersion()
        {
            const int noOfSettings = 1;
            var version = new Version(1, 0, 0, 5);
            var settingModel = SettingModelFactory.CreateSettingModels(noOfSettings);
            var workerServiceVersionModel = new WorkerServiceVersionModel { Version = version };

            // Arrange
            _mockIGetWorkerServiceVersionService.Setup(x => x.GetWorkerServiceVersionServiceAsync(settingModel.FirstOrDefault())).ReturnsAsync(workerServiceVersionModel);
            _mockILogger.Setup(x => x.GetSettingsAsync()).ReturnsAsync(settingModel);
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            await Task.Delay(1500);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.IsNotNull(settingsViewModel.Settings);
            Assert.AreEqual(noOfSettings, settingsViewModel.Settings.Count);
            Assert.AreEqual(version.ToString(), settingsViewModel.Settings[0].Version);
        }

        [TestMethod]
        public async Task LoadItemsCommand_CheckVersionUnavailable()
        {
            const int noOfSettings = 1;
            var settingModel = SettingModelFactory.CreateSettingModels(noOfSettings);

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).ReturnsAsync(settingModel);
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            await Task.Delay(1500);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.IsNotNull(settingsViewModel.Settings);
            Assert.AreEqual(noOfSettings, settingsViewModel.Settings.Count);
            Assert.AreEqual("UNAVAILABLE", settingsViewModel.Settings[0].Version);
        }

        [TestMethod]
        public async Task LoadItemsCommand_Exception()
        {
            const string exceptionMessage = "Error on GetSettingsAsync!";

            // Arrange
            _mockILogger.Setup(x => x.GetSettingsAsync()).Throws(new Exception(exceptionMessage));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Act
            settingsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            Assert.AreEqual(0, settingsViewModel.Settings.Count);
            await Task.Delay(1500);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
            _mockISettingsPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }

        [TestMethod]
        public void AddCommand()
        {
            // Arrange
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Act
            settingsViewModel.AddCommand.Execute(null);

            // Assert
            _mockISettingsPageHelper.Verify(x => x.CustomPushAsyncToNewSetting(), Times.Once);
        }

        [TestMethod]
        public void AddCommand_Exception()
        {
            const string exceptionMessage = "Error on CustomPushModalAsync!";

            // Arrange
            _mockISettingsPageHelper.Setup(x => x.CustomPushAsyncToNewSetting()).Throws(new Exception(exceptionMessage));
            var settingsViewModel = new SettingsViewModel(_mockILogger.Object, _mockISettingsPageHelper.Object, _mockIGetWorkerServiceVersionService.Object);

            // Act
            settingsViewModel.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingsViewModel.Title);
            _mockISettingsPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}