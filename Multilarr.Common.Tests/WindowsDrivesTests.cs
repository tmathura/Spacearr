using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Tests.Factories;

namespace Multilarr.Common.Tests
{
    [TestClass]
    public class ComputersDrivesTests
    {
        private const int NoOfDriveInfos = 3;
        private Mock<IComputerDriveInfo> _mockIComputersDriveInfo;
        private ComputerDrives _computersDrives;

        [TestInitialize]
        public void SetUp()
        {
            _mockIComputersDriveInfo = new Mock<IComputerDriveInfo>();
            _mockIComputersDriveInfo.Setup(x => x.GetComputerDrives()).Returns(ComputerDriveInfoFactory.CreateComputerDriveInfos(NoOfDriveInfos));
            _computersDrives = new ComputerDrives(_mockIComputersDriveInfo.Object);
        }

        [TestMethod]
        public void GetDrives()
        {
            // Act
            var drives = _computersDrives.GetComputerDrives();

            // Assert
            Assert.AreEqual(NoOfDriveInfos, drives.Count);
        }
    }
}