﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces;
using System;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class NewSettingDetailViewModelTests
    {
        private const string Title = "New Setting";
        private Mock<ILogger> _mockILogger;
        private Mock<IPusherValidation> _mockIPusherValidation;
        private Mock<INavigationPopModalHelper> _mockINavigationPopModalHelper;
        private Mock<IValidationHelper> _mockIValidationHelper;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIPusherValidation = new Mock<IPusherValidation>();
            _mockINavigationPopModalHelper = new Mock<INavigationPopModalHelper>();
            _mockIValidationHelper = new Mock<IValidationHelper>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
        }

        [TestMethod]
        public void NewSettingDetailViewModel()
        {
            // Arrange
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
        }

        [TestMethod]
        public void SaveCommand()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

            // Act
            newSettingDetailViewModel.SaveCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockILogger.Verify(x => x.LogSettingAsync(newSettingDetailViewModel.SettingModel), Times.Once);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Success", "Setting saved!", "OK"), Times.Once);
            _mockINavigationPopModalHelper.Verify(x => x.CustomPopModalAsync(), Times.Once);
        }

        [TestMethod]
        public void SaveCommand_ExceptionTestCase1()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

            // Act
            newSettingDetailViewModel.SaveCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", "Pusher details invalid!", "OK"), Times.Once);
        }

        [TestMethod]
        public void SaveCommand_ExceptionTestCase2()
        {
            const string exceptionMessage = "Error on ValidationHelper!";

            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Throws(new Exception(exceptionMessage));
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

            // Act
            newSettingDetailViewModel.SaveCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }

        [TestMethod]
        public void CancelCommand()
        {
            // Arrange
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

            // Act
            newSettingDetailViewModel.CancelCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockINavigationPopModalHelper.Verify(x => x.CustomPopModalAsync(), Times.Once);
        }

        [TestMethod]
        public void CancelCommand_Exception()
        {
            const string exceptionMessage = "Error on CustomPopModalAsync!";

            // Arrange
            _mockINavigationPopModalHelper.Setup(x => x.CustomPopModalAsync()).Throws(new Exception(exceptionMessage));
            var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

            // Act
            newSettingDetailViewModel.CancelCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}