using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Command.Implementations.Commands;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Models;
using Spacearr.Common.Tests.Factories;

namespace Spacearr.Common.Tests.Command.Commands
{
    [TestClass]
    public class SaveFirebasePushNotificationTokenCommandTests
    {
        private Mock<ILogger> _mockILogger;
        private SaveFirebasePushNotificationTokenCommand _saveFirebasePushNotificationTokenCommand;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
        }

        [TestMethod]
        public void Execute_Save()
        {
            // Arrange
            var token = "token";
            var deviceId = Guid.NewGuid();
            _mockILogger.Setup(x => x.GetFirebasePushNotificationDeviceAsync(deviceId));
            var firebasePushNotificationDevice = new FirebasePushNotificationDevice { DeviceId = deviceId, Token = token };
            _saveFirebasePushNotificationTokenCommand = new SaveFirebasePushNotificationTokenCommand(_mockILogger.Object, firebasePushNotificationDevice);

            // Act
            var commandData = _saveFirebasePushNotificationTokenCommand.Execute();

            // Assert
            Assert.IsNotNull(commandData);
            _mockILogger.Verify(c => c.SaveFirebasePushNotificationDeviceAsync(deviceId, token), Times.Once());
        }

        [TestMethod]
        public void Execute_Update()
        {
            // Arrange
            var token = "token";
            var deviceId = Guid.NewGuid();
            var firebasePushNotificationDeviceModel = FirebasePushNotificationDeviceModelFactory.CreateFirebasePushNotificationDeviceModels(1).FirstOrDefault();
            firebasePushNotificationDeviceModel.DeviceId = deviceId;
            _mockILogger.Setup(x => x.GetFirebasePushNotificationDeviceAsync(deviceId)).ReturnsAsync(firebasePushNotificationDeviceModel);
            var firebasePushNotificationDevice = new FirebasePushNotificationDevice { DeviceId = deviceId, Token = token };
            _saveFirebasePushNotificationTokenCommand = new SaveFirebasePushNotificationTokenCommand(_mockILogger.Object, firebasePushNotificationDevice);

            // Act
            var commandData = _saveFirebasePushNotificationTokenCommand.Execute();

            // Assert
            Assert.IsNotNull(commandData);
            firebasePushNotificationDeviceModel.Token = token;
            _mockILogger.Verify(c => c.UpdateFirebasePushNotificationDeviceAsync(firebasePushNotificationDeviceModel), Times.Once());
        }
    }
}