using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Helpers;
using Multilarr.Core.Tests.Factories;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces.Service;
using System;
using System.Threading.Tasks;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class ComputerDrivesViewModelTests
    {
        private const string Title = "Computer Drives";
        private Mock<ILogger> _mockILogger;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;
        private Mock<IComputerDriveService> _mockIComputerDriveService;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
            _mockIComputerDriveService = new Mock<IComputerDriveService>();
        }

        [TestMethod]
        public void ComputerDrivesViewModel()
        {
            // Arrange
            var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockIComputerDriveService.Object);

            // Assert
            Assert.AreEqual(Title, computerDriveDetailViewModel.Title);
        }

        [TestMethod]
        public void LoadItemsCommand()
        {
            const int noOfComputerDriveModels = 9;

            // Arrange
            _mockIComputerDriveService.Setup(x => x.GetComputerDrivesAsync()).ReturnsAsync(ComputerDriveModelFactory.CreateComputerDriveModels(noOfComputerDriveModels));
            var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockIComputerDriveService.Object);
            
            // Act
            computerDriveDetailViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, computerDriveDetailViewModel.Title);
            Assert.IsNotNull(computerDriveDetailViewModel.ComputerDrives);
            Assert.AreEqual(noOfComputerDriveModels, computerDriveDetailViewModel.ComputerDrives.Count);
        }

        [TestMethod]
        public void LoadItemsCommand_Exception()
        {
            const string exceptionMessage = "GetComputerDrivesAsync took too long!";

            // Arrange
            _mockIComputerDriveService.Setup(x => x.GetComputerDrivesAsync()).Throws(new Exception(exceptionMessage));
            var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockIComputerDriveService.Object);
            
            // Act
            computerDriveDetailViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, computerDriveDetailViewModel.Title);
            Assert.AreEqual(0, computerDriveDetailViewModel.ComputerDrives.Count);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}