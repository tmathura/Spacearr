using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
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
        private Mock<ILogger> _mockILogger;
        private Mock<IDisplayAlertHelper> _displayAlertHelper;
        private Mock<IComputerDriveService> _computerDriveService;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _displayAlertHelper = new Mock<IDisplayAlertHelper>();
            _computerDriveService = new Mock<IComputerDriveService>();
        }

        [TestMethod]
        public void ComputerDrivesViewModel()
        {
            {
                // Arrange
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _displayAlertHelper.Object, _computerDriveService.Object);

                // Assert
                Assert.AreEqual("Computer Drives", computerDriveDetailViewModel.Title);
            }
        }

        [TestMethod]
        public void LoadItemsCommand()
        {
            {
                const int noOfComputerDriveModels = 9;

                // Arrange
                var computerDriveModelList = ComputerDriveModelFactory.CreateComputerDriveModels(noOfComputerDriveModels);
                var taskComputerDriveModelList = Task.FromResult(computerDriveModelList);
                _computerDriveService.Setup(x => x.GetComputerDrivesAsync()).Returns(taskComputerDriveModelList);
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _displayAlertHelper.Object, _computerDriveService.Object);
                
                // Act
                computerDriveDetailViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual("Computer Drives", computerDriveDetailViewModel.Title);
                Assert.IsNotNull(computerDriveDetailViewModel.ComputerDrives);
                Assert.AreEqual(noOfComputerDriveModels, computerDriveDetailViewModel.ComputerDrives.Count);
            }
        }

        [TestMethod]
        public void LoadItemsCommand_Exception()
        {
            {
                // Arrange
                _computerDriveService.Setup(x => x.GetComputerDrivesAsync()).Throws(new Exception("GetComputerDrivesAsync took too long!"));
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _displayAlertHelper.Object, _computerDriveService.Object);
                
                // Act
                computerDriveDetailViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual("Computer Drives", computerDriveDetailViewModel.Title);
                Assert.AreEqual(0, computerDriveDetailViewModel.ComputerDrives.Count);
                _displayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", "GetComputerDrivesAsync took too long!", "OK"), Times.Once);
            }
        }
    }
}