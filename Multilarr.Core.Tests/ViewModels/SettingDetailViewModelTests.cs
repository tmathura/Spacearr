using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class SettingDetailViewModelTests
    {
        private const string Title = "Computer Name";
        private SettingModel _settingModel;
        private Mock<ILogger> _mockILogger;
        private Mock<IPusherValidation> _mockIPusherValidation;
        private Mock<INavigationPopHelper> _mockINavigationPopHelper;
        private Mock<IValidationHelper> _mockIValidationHelper;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;

        [TestInitialize]
        public void SetUp()
        {
            _settingModel = new SettingModel
            {
                ComputerName = Title
            };

            _mockILogger = new Mock<ILogger>();
            _mockIPusherValidation = new Mock<IPusherValidation>();
            _mockINavigationPopHelper = new Mock<INavigationPopHelper>();
            _mockIValidationHelper = new Mock<IValidationHelper>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
        }

        [TestMethod]
        public void SettingDetailViewModel()
        {
            // Arrange
            var settingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object, _settingModel);

            // Assert
            Assert.AreEqual(Title, settingDetailViewModel.Title);
        }

        [TestMethod]
        public void UpdateCommand()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var settingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object, _settingModel);

            // Act
            settingDetailViewModel.UpdateCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingDetailViewModel.Title);
            _mockILogger.Verify(x => x.UpdateSettingAsync(_settingModel), Times.Once);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Success", "Setting saved!", "OK"), Times.Once);
            _mockINavigationPopHelper.Verify(x => x.CustomPopAsync(), Times.Once);
        }

        [TestMethod]
        public void UpdateCommand_Exception()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            var newSettingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object, _settingModel);

            // Act
            newSettingDetailViewModel.UpdateCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", "Pusher details invalid!", "OK"), Times.Once);
        }
    }
}