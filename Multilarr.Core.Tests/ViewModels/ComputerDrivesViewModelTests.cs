using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Tests.Factories;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces.Service;
using System.Threading.Tasks;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class ComputerDrivesViewModelTests
    {
        private Mock<IDisplayAlertHelper> _displayAlertHelper;
        private Mock<IComputerDriveService> _computerDriveService;

        [TestInitialize]
        public void SetUp()
        {
            _displayAlertHelper = new Mock<IDisplayAlertHelper>();
            _computerDriveService = new Mock<IComputerDriveService>();
        }

        [TestMethod]
        public void ComputerDrivesViewModel()
        {
            {
                // Arrange
                var logger = new Mock<ILogger>();
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(logger.Object, _displayAlertHelper.Object, _computerDriveService.Object);

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
                var logger = new Mock<ILogger>();
                var computerDriveModelList = ComputerDriveModelFactory.CreateComputerDriveModels(noOfComputerDriveModels);
                var taskComputerDriveModelList = Task.FromResult(computerDriveModelList);
                _computerDriveService.Setup(x => x.GetComputerDrivesAsync()).Returns(taskComputerDriveModelList);
                var computerDriveDetailViewModel = new ComputerDrivesViewModel(logger.Object, _displayAlertHelper.Object, _computerDriveService.Object);
                
                // Act
                computerDriveDetailViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual("Computer Drives", computerDriveDetailViewModel.Title);
                Assert.IsNotNull(computerDriveDetailViewModel.ComputerDrives);
                Assert.AreEqual(noOfComputerDriveModels, computerDriveDetailViewModel.ComputerDrives.Count);
            }
        }
    }
}