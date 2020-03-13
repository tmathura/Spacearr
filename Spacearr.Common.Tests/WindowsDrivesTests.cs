using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Tests.Factories;

namespace Spacearr.Common.Tests
{
    [TestClass]
    public class ComputersDrivesTests
    {
        [TestMethod]
        public void GetDrives()
        {
            const int noOfDriveInfos = 3;
            // Arrange
            var mockIComputersDriveInfo = new Mock<IComputerDriveInfo>();
            mockIComputersDriveInfo.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfos(noOfDriveInfos));
            var computersDrives = new ComputerDrives(mockIComputersDriveInfo.Object);

            // Act
            var drives = computersDrives.GetComputerDrives();

            // Assert
            Assert.AreEqual(noOfDriveInfos, drives.Count);
        }
    }
}