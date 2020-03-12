using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class ComputerDriveDetailViewModelTests
    {
        [TestMethod]
        public void ComputerDriveDetailViewModel()
        {
            var computerDriveModel = new ComputerDriveModel
            {
                Name = "Computer Name",
                VolumeLabel = "VolumeL abel"
            };

            // Arrange
            var computerDriveDetailViewModel = new ComputerDriveDetailViewModel(computerDriveModel);

            // Assert
            Assert.AreEqual($"{computerDriveModel.Name} {computerDriveModel.VolumeLabel}", computerDriveDetailViewModel.Title);
            Assert.AreEqual(computerDriveModel, computerDriveDetailViewModel.ComputerDriveModel);
        }
    }
}