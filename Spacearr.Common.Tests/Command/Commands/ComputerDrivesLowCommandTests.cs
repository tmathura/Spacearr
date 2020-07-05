using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Command.Commands;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Common.Tests.Factories;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Common.Tests.Command.Commands
{
    [TestClass]
    public class ComputerDrivesLowCommandTests
    {
        private Mock<IConfiguration> _mockIConfiguration;
        private Mock<ILogger> _mockILogger;
        private Mock<IComputerDrives> _mockIComputerDrives;
        private Mock<ISendFirebasePushNotification> _mockISendFirebasePushNotification;
        private ComputerDrivesLowCommand _computerDrivesLowCommand;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockIComputerDrives = new Mock<IComputerDrives>();
            _mockISendFirebasePushNotification = new Mock<ISendFirebasePushNotification>();
        }

        [TestMethod]
        public void Execute()
        {
            // Arrange
            const int noOfComputerDriveInfos = 7;
            _mockILogger.Setup(x => x.GetFirebasePushNotificationDevicesAsync()).ReturnsAsync(FirebasePushNotificationDeviceModelFactory.CreateFirebasePushNotificationDeviceModels(2));
            var mockIConfigurationSection = new Mock<IConfigurationSection>();
            mockIConfigurationSection.Setup(a => a.Value).Returns("1");
            _mockIConfiguration.Setup(a => a.GetSection("LowComputerDriveGBValue")).Returns(mockIConfigurationSection.Object);
            _mockIComputerDrives.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfos(noOfComputerDriveInfos, DriveType.Fixed));
            _mockISendFirebasePushNotification.Setup(x => x.SendNotificationMultipleDevices(It.IsAny<List<string>>(), It.IsAny<string>(),
                It.IsAny<string>()));
            _computerDrivesLowCommand = new ComputerDrivesLowCommand(_mockIConfiguration.Object, _mockILogger.Object, _mockIComputerDrives.Object, _mockISendFirebasePushNotification.Object);

            // Act
            var commandData = _computerDrivesLowCommand.Execute();

            // Assert
            Assert.IsNotNull(commandData);
            Assert.AreEqual(noOfComputerDriveInfos, _mockISendFirebasePushNotification.Invocations.Count);
        }

        [TestMethod]
        public void Execute_ZeroComputerDrivesLow()
        {
            // Arrange
            var mockIConfigurationSection = new Mock<IConfigurationSection>();
            mockIConfigurationSection.Setup(a => a.Value).Returns("0");
            _mockIConfiguration.Setup(a => a.GetSection("LowComputerDriveGBValue")).Returns(mockIConfigurationSection.Object);
            _mockIComputerDrives.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfoFixed);
            _computerDrivesLowCommand = new ComputerDrivesLowCommand(_mockIConfiguration.Object, _mockILogger.Object, _mockIComputerDrives.Object, _mockISendFirebasePushNotification.Object);

            // Act
            var commandData = _computerDrivesLowCommand.Execute();

            // Assert
            Assert.IsNotNull(commandData);
            Assert.AreEqual(0, _mockISendFirebasePushNotification.Invocations.Count);
        }
    }
}