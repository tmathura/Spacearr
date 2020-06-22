using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Helpers;
using Spacearr.Core.Xamarin.Tests.Factories;
using Spacearr.Core.Xamarin.ViewModels;
using Spacearr.Pusher.API.Interfaces.Service;
using System;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class ComputerViewModelTests
    {
        private const string Title = "Computer Drives";
        private Mock<ILogger> _mockILogger;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;
        private Mock<IComputerService> _mockIComputerService;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
            _mockIComputerService = new Mock<IComputerService>();
        }

        [TestMethod]
        public void ComputerViewModel()
        {
            // Arrange
            var computerViewModel = new ComputerViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockIComputerService.Object);

            // Assert
            Assert.AreEqual(Title, computerViewModel.Title);
        }

        [TestMethod]
        public void LoadItemsCommand()
        {
            const int noOfComputerModels = 9;

            // Arrange
            _mockIComputerService.Setup(x => x.GetComputersAsync()).ReturnsAsync(ComputerModelFactory.CreateComputerModels(noOfComputerModels));
            var computerViewModel = new ComputerViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockIComputerService.Object);
            
            // Act
            computerViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, computerViewModel.Title);
            Assert.IsNotNull(computerViewModel.Computers);
            Assert.AreEqual(noOfComputerModels, computerViewModel.Computers.Count);
        }

        [TestMethod]
        public void LoadItemsCommand_Exception()
        {
            const string exceptionMessage = "GetComputersAsync took too long!";

            // Arrange
            _mockIComputerService.Setup(x => x.GetComputersAsync()).Throws(new Exception(exceptionMessage));
            var computerViewModel = new ComputerViewModel(_mockILogger.Object, _mockIDisplayAlertHelper.Object, _mockIComputerService.Object);
            
            // Act
            computerViewModel.LoadItemsCommand.Execute(null);

            // Assert
            Assert.AreEqual(Title, computerViewModel.Title);
            Assert.AreEqual(0, computerViewModel.Computers.Count);
            _mockIDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}