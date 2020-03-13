using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
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