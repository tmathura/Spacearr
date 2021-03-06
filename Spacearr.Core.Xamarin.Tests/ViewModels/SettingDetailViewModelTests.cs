﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Helpers.Interfaces;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Validator.Interfaces;
using System;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class SettingDetailViewModelTests
    {
        private const string Title = "Computer Name";
        private SettingModel _settingModel;
        private Mock<ILogger> _mockILogger;
        private Mock<IPusherValidation> _mockIPusherValidation;
        private Mock<ISettingDetailPageHelper> _mockISettingDetailPageHelper;
        private Mock<IValidationHelper> _mockIValidationHelper;

        [TestInitialize]
        public void SetUp()
        {
            _settingModel = new SettingModel
            {
                ComputerName = Title
            };

            _mockILogger = new Mock<ILogger>();
            _mockIPusherValidation = new Mock<IPusherValidation>();
            _mockISettingDetailPageHelper = new Mock<ISettingDetailPageHelper>();
            _mockIValidationHelper = new Mock<IValidationHelper>();
        }

        [TestMethod]
        public void SettingDetailViewModel()
        {
            // Arrange
            var settingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockISettingDetailPageHelper.Object, _mockIValidationHelper.Object, _settingModel);

            // Assert
            Assert.AreEqual(Title, settingDetailViewModel.Title);
        }

        [TestMethod]
        public void UpdateCommand()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var settingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockISettingDetailPageHelper.Object, _mockIValidationHelper.Object, _settingModel);

            // Act
            settingDetailViewModel.UpdateCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingDetailViewModel.Title);
            _mockILogger.Verify(x => x.UpdateSettingAsync(_settingModel), Times.Once);
            _mockISettingDetailPageHelper.Verify(x => x.CustomDisplayAlert("Success", "Setting saved!", "OK"), Times.Once);
            _mockISettingDetailPageHelper.Verify(x => x.CustomPopAsync(), Times.Once);
        }

        [TestMethod]
        public void UpdateCommand_ExceptionTestCase1()
        {
            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Returns(true);
            _mockIPusherValidation.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            var newSettingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockISettingDetailPageHelper.Object, _mockIValidationHelper.Object, _settingModel);

            // Act
            newSettingDetailViewModel.UpdateCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockISettingDetailPageHelper.Verify(x => x.CustomDisplayAlert("Error", "Pusher details invalid!", "OK"), Times.Once);
        }

        [TestMethod]
        public void UpdateCommand_ExceptionTestCase2()
        {
            const string exceptionMessage = "Error on ValidationHelper!";

            // Arrange
            _mockIValidationHelper.Setup(x => x.IsFormValid(It.IsAny<object>())).Throws(new Exception(exceptionMessage));
            var newSettingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockISettingDetailPageHelper.Object, _mockIValidationHelper.Object, _settingModel);

            // Act
            newSettingDetailViewModel.UpdateCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockISettingDetailPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }

        [TestMethod]
        public void DeleteCommand()
        {
            // Arrange
            var settingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockISettingDetailPageHelper.Object, _mockIValidationHelper.Object, _settingModel);

            // Act
            settingDetailViewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, settingDetailViewModel.Title);
            _mockILogger.Verify(x => x.DeleteLogAsync(_settingModel), Times.Once);
            _mockISettingDetailPageHelper.Verify(x => x.CustomDisplayAlert("Success", "Setting Deleted!", "OK"), Times.Once);
            _mockISettingDetailPageHelper.Verify(x => x.CustomPopAsync(), Times.Once);
        }

        [TestMethod]
        public void DeleteCommand_Exception()
        {
            const string exceptionMessage = "Error on DeleteLogAsync!";

            // Arrange
            _mockILogger.Setup(x => x.DeleteLogAsync(_settingModel)).Throws(new Exception(exceptionMessage));
            var newSettingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockIPusherValidation.Object, _mockISettingDetailPageHelper.Object, _mockIValidationHelper.Object, _settingModel);

            // Act
            newSettingDetailViewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            _mockISettingDetailPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}