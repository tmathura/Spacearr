using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spacearr.Common.Models;
using System.Collections.Generic;

namespace Spacearr.Common.Tests.Models
{
    [TestClass]
    public class ComputerModelTests
    {

        [TestMethod]
        public void Validate_TestCase1()
        {
            // Arrange
            var model = new ComputerModel
            {
                ComputerDrives = new List<ComputerDriveModel>
                {
                    new ComputerDriveModel
                    {
                        TotalSize = 1073741824,
                        TotalFreeSpace = 1073741824
                    }
                }
            };

            // Assert
            Assert.AreEqual(0, model.ProgressBarValue);
            Assert.AreEqual(true, model.Online);
            Assert.AreEqual(false, model.HasError);
        }

        [TestMethod]
        public void Validate_TestCase2()
        {
            // Arrange
            var model = new ComputerModel
            {
                ComputerDrives = new List<ComputerDriveModel>
                {
                    new ComputerDriveModel
                    {
                        TotalSize = 1073741824,
                        TotalFreeSpace = 536870912
                    }
                }
            };

            // Assert
            Assert.AreEqual(0.5, model.ProgressBarValue);
            Assert.AreEqual(true, model.Online);
            Assert.AreEqual(false, model.HasError);
        }

        [TestMethod]
        public void Validate_TestCase3()
        {
            // Arrange
            var model = new ComputerModel
            {
                Error = "There is an error."
            };

            // Assert
            Assert.AreEqual(0, model.ProgressBarValue);
            Assert.AreEqual(false, model.Online);
            Assert.AreEqual(true, model.HasError);
        }
    }
}