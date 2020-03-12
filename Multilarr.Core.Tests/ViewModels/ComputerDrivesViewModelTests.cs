using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.ViewModels;
using Multilarr.Pusher.API.Interfaces.Service;

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
    }
}