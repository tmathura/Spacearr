using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.Tests.Factories;
using Spacearr.Core.Xamarin.ViewModels;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class ComputerDriveViewModelTests
    {
        [TestMethod]
        public void ComputerDriveViewModel()
        {
            const int noOfComputerDriveModels = 5;

            var computerModel = new ComputerModel
            {
                Name = "Computer Name",
                ComputerDrives = ComputerDriveModelFactory.CreateComputerDriveModels(noOfComputerDriveModels)
            };

            // Arrange
            var computerDriveViewModel = new ComputerDriveViewModel(computerModel);

            // Assert
            Assert.AreEqual(computerModel.Name, computerDriveViewModel.Title);
            Assert.AreEqual(computerModel, computerDriveViewModel.ComputerDriveModel);
        }
    }
}