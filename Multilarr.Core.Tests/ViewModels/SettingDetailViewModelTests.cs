using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.Helpers;
using Multilarr.Core.ViewModels;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class SettingDetailViewModelTests
    {
        private const string Title = "Computer Name";
        private SettingModel _settingModel;
        private Mock<ILogger> _mockILogger;
        private Mock<INavigationPopHelper> _mockINavigationPopHelper;
        private Mock<IValidationHelper> _mockIValidationHelper;
        private Mock<IDisplayAlertHelper> _mockIDisplayAlertHelper;

        [TestInitialize]
        public void SetUp()
        {
            _settingModel = new SettingModel
            {
                ComputerName = Title
            };

            _mockILogger = new Mock<ILogger>();
            _mockINavigationPopHelper = new Mock<INavigationPopHelper>();
            _mockIValidationHelper = new Mock<IValidationHelper>();
            _mockIDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
        }

        [TestMethod]
        public void SettingDetailViewModel()
        {
            {
                // Arrange
                var newSettingDetailViewModel = new SettingDetailViewModel(_mockILogger.Object, _mockINavigationPopHelper.Object, _mockIValidationHelper.Object, _mockIDisplayAlertHelper.Object, _settingModel);

                // Assert
                Assert.AreEqual(Title, newSettingDetailViewModel.Title);
            }
        }
    }
}