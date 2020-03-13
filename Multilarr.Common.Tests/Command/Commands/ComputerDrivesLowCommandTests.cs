using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Command.Commands;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Multilarr.Common.Tests.Factories;
using Newtonsoft.Json;

namespace Multilarr.Common.Tests.Command.Commands
{
    [TestClass]
    public class ComputerDrivesLowCommandTests
    {
        private Mock<IConfiguration> _mockIConfiguration;
        private Mock<IComputerDrives> _mockIComputerDrives;
        private ComputerDrivesLowCommand _computerDrivesLowCommand;

        [TestInitialize]
        public void SetUp()
        {
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockIComputerDrives = new Mock<IComputerDrives>();
        }

        [TestMethod]
        public void Execute()
        {
            // Arrange
            var mockIConfigurationSection = new Mock<IConfigurationSection>();
            mockIConfigurationSection.Setup(a => a.Value).Returns("1");
            _mockIConfiguration.Setup(a => a.GetSection("LowComputerDriveGBValue")).Returns(mockIConfigurationSection.Object);
            _mockIComputerDrives.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfoFixed);
            _computerDrivesLowCommand = new ComputerDrivesLowCommand(_mockIConfiguration.Object, _mockIComputerDrives.Object);

            // Act
            var commandData = _computerDrivesLowCommand.Execute();
            var notificationEventArgs = JsonConvert.DeserializeObject<NotificationEventArgsModel>(commandData);

            // Assert
            Assert.IsNotNull(commandData);
            Assert.AreEqual("Computer Drives Low", notificationEventArgs.Title);
        }

        [TestMethod]
        public void Execute_ZeroComputerDrivesLow()
        {
            // Arrange
            var mockIConfigurationSection = new Mock<IConfigurationSection>();
            mockIConfigurationSection.Setup(a => a.Value).Returns("0");
            _mockIConfiguration.Setup(a => a.GetSection("LowComputerDriveGBValue")).Returns(mockIConfigurationSection.Object);
            _mockIComputerDrives.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfoFixed);
            _computerDrivesLowCommand = new ComputerDrivesLowCommand(_mockIConfiguration.Object, _mockIComputerDrives.Object);

            // Act
            var commandData = _computerDrivesLowCommand.Execute();
            var notificationEventArgs = JsonConvert.DeserializeObject<NotificationEventArgsModel>(commandData);

            // Assert
            Assert.IsNotNull(commandData);
            Assert.IsNull(notificationEventArgs);
        }
    }
}