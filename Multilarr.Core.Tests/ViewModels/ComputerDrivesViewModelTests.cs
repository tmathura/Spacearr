﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private Mock<IDisplayAlertHelper> _mockDisplayAlertHelper;
        private Mock<IComputerDriveService> _mockComputerDriveService;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
            _mockComputerDriveService = new Mock<IComputerDriveService>();
        }

        [TestMethod]
        public void ComputerDrivesViewModel()
        {
            {
                // Arrange
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _mockDisplayAlertHelper.Object, _mockComputerDriveService.Object);

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
                _mockComputerDriveService.Setup(x => x.GetComputerDrivesAsync()).Returns(taskComputerDriveModelList);
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _mockDisplayAlertHelper.Object, _mockComputerDriveService.Object);
                
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
                _mockComputerDriveService.Setup(x => x.GetComputerDrivesAsync()).Throws(new Exception("GetComputerDrivesAsync took too long!"));
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(_mockILogger.Object, _mockDisplayAlertHelper.Object, _mockComputerDriveService.Object);
                
                // Act
                computerDriveDetailViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual("Computer Drives", computerDriveDetailViewModel.Title);
                Assert.AreEqual(0, computerDriveDetailViewModel.ComputerDrives.Count);
                _mockDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", "GetComputerDrivesAsync took too long!", "OK"), Times.Once);
            }
        }
    }
}