using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces;
using System;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class NewSettingDetailViewModelTests
    {
        private const string Title = "New Setting";
        private Mock<ILogger> _mockILogger;
        private Mock<IPusherValidation> _mockIPusherValidation;
        private Mock<INewSettingPageHelper> _mockINewSettingPageHelper;
        private Mock<IValidationHelper> _mockIValidationHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIPusherValidation = new Mock<IPusherValidation>();
            _mockINewSettingPageHelper = new Mock<INewSettingPageHelper>();
            _mockIValidationHelper = new Mock<IValidationHelper>();
        }

        [TestMethod]
        public void NewSettingDetailViewModel()
        {
            // Arrange
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINewSettingPageHelper.Object, _mockIValidationHelper.Object);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
        }

        [TestMethod]
        public void SaveCommand()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINewSettingPageHelper.Object, _mockIValidationHelper.Object);

            // Act
            newSettingDetailViewModel.SaveCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockILogger.Verify(x => x.LogSettingAsync(newSettingDetailViewModel.SettingModel), Times.Once);
            _mockINewSettingPageHelper.Verify(x => x.CustomDisplayAlert("Success", "Setting saved!", "OK"), Times.Once);
            _mockINewSettingPageHelper.Verify(x => x.CustomPopAsync(), Times.Once);
        }

        [TestMethod]
        public void SaveCommand_ExceptionTestCase1()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINewSettingPageHelper.Object, _mockIValidationHelper.Object);

            // Act
            newSettingDetailViewModel.SaveCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockINewSettingPageHelper.Verify(x => x.CustomDisplayAlert("Error", "Pusher details invalid!", "OK"), Times.Once);
        }

        [TestMethod]
        public void SaveCommand_ExceptionTestCase2()
        {
            const string exceptionMessage = "Error on ValidationHelper!";

            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Throws(new Exception(exceptionMessage));
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINewSettingPageHelper.Object, _mockIValidationHelper.Object);

            // Act
            newSettingDetailViewModel.SaveCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockINewSettingPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}