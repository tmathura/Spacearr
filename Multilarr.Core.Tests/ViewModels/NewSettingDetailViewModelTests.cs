using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class NewSettingDetailViewModelTests
    {
        private const string Title = "New Setting";
        private Mock<ILogger> _mockILogger;
        private Mock<INavigationPopModalHelper> _mockINavigationPopModalHelper;
        private Mock<IValidationHelper> _mockIValidationHelper;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockINavigationPopModalHelper = new Mock<INavigationPopModalHelper>();
            _mockIValidationHelper = new Mock<IValidationHelper>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
        }

        [TestMethod]
        public void NewSettingDetailViewModel()
        {
            {
                // Arrange
                var newSettingDetailViewModel = new NewSettingDetailViewModel(_mockILogger.Object, _mockINavigationPopModalHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object);

                // Assert
                Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            }
        }
    }
}