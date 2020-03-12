using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Tests.Factories;
using Multilarr.Core.ViewModels;
using System.Threading.Tasks;

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

        [TestMethod]
        public void LoadItemsCommand()
        {
            {
                const int noOfLogs = 7;

                // Arrange
                var logModelList = LogModelFactory.CreateLogModels(noOfLogs);
                var taskLogModelList = Task.FromResult(logModelList);
                _mockILogger.Setup(x => x.GetLogsAsync()).Returns(taskLogModelList);
                var logsViewModel = new LogsViewModel(_mockILogger.Object, _displayAlertHelper.Object);

                // Act
                logsViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual("Logs", logsViewModel.Title);
                Assert.IsNotNull(logsViewModel.Logs);
                Assert.AreEqual(noOfLogs, logsViewModel.Logs.Count);
            }
        }
    }
}