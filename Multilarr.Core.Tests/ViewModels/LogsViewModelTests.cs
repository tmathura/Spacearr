using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.ViewModels;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass()]
    public class LogsViewModelTests
    {
        private Mock<ILogger> _mockILogger;
        private Mock<IDisplayAlertHelper> _displayAlertHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _displayAlertHelper = new Mock<IDisplayAlertHelper>();
        }

        [TestMethod]
        public void LogsViewModel()
        {
            {
                // Arrange
                var logsViewModel = new LogsViewModel(_mockILogger.Object, _displayAlertHelper.Object);

                // Assert
                Assert.AreEqual("Logs", logsViewModel.Title);
            }
        }
    }
}