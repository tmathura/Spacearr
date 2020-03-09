using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Models;

namespace Multilarr.Common.Tests.Models
{
    [TestClass]
    public class ComputerDriveTests
    {

        [TestMethod]
        public void Validate_TestCase1()
        {
            // Arrange
            var model = new ComputerDrive
            {
                TotalSize = 1073741824,
                TotalFreeSpace = 1073741824
            };

            // Assert
            Assert.AreEqual(0, model.TotalUsedSpace);
            Assert.AreEqual(0, model.ProgressBarValue);
            Assert.AreEqual("1.00 GB", model.TotalSizeString);
            Assert.AreEqual("1.00 GB", model.TotalFreeSpaceString);
        }

        [TestMethod]
        public void Validate_TestCase2()
        {
            // Arrange
            var model = new ComputerDrive
            {
                TotalSize = 1073741824,
                TotalFreeSpace = 536870912
            };

            // Assert
            Assert.AreEqual(536870912, model.TotalUsedSpace);
            Assert.AreEqual(0.5, model.ProgressBarValue);
            Assert.AreEqual("1.00 GB", model.TotalSizeString);
            Assert.AreEqual("512.00 MB", model.TotalFreeSpaceString);
        }
    }
}