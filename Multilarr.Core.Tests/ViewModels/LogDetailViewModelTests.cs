using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class LogDetailViewModelTests
    {
        [TestMethod]
        public void LogDetailViewModel()
        {
            var logModel = new LogModel
            {
                Id = 1
            };

            // Arrange
            var logDetailViewModel = new LogDetailViewModel(logModel);

            // Assert
            Assert.AreEqual("1", logDetailViewModel.Title);
            Assert.AreEqual(logModel, logDetailViewModel.Log);
        }
    }
}