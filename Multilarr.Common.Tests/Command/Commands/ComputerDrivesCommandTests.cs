using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Command.Commands;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Models;
using Multilarr.Common.Tests.Factories;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Multilarr.Common.Tests.Command.Commands
{
    [TestClass]
    public class ComputerDrivesCommandTests
    {
        private Mock<IComputerDrives> _mockIComputerDrives;
        private ComputerDrivesCommand _computerDrivesCommand;

        [TestInitialize]
        public void SetUp()
        {
            _mockIComputerDrives = new Mock<IComputerDrives>();
        }

        [TestMethod]
        public void Execute()
        {
            // Arrange
            const int noOfComputerDrives = 9;
            _mockIComputerDrives.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfos(noOfComputerDrives));
            _computerDrivesCommand = new ComputerDrivesCommand(_mockIComputerDrives.Object);

            // Act
            var commandData = _computerDrivesCommand.Execute();
            var computerDrives = JsonConvert.DeserializeObject<List<ComputerDriveModel>>(commandData);

            // Assert
            Assert.AreEqual(noOfComputerDrives, computerDrives.Count);
        }

        [TestMethod]
        public void Execute_ZeroComputerDrives()
        {
            // Arrange
            _mockIComputerDrives.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfos(0));
            _computerDrivesCommand = new ComputerDrivesCommand(_mockIComputerDrives.Object);

            // Act
            var commandData = _computerDrivesCommand.Execute();
            var computerDrives = JsonConvert.DeserializeObject<List<ComputerDriveModel>>(commandData);

            // Assert
            Assert.AreEqual(0, computerDrives.Count);
        }
    }
}