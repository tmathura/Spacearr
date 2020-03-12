using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Core.Tests.Factories;
using Multilarr.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace Multilarr.Core.Tests.ViewModels
{
    [TestClass]
    public class LogsViewModelTests
    {
        private const string Title = "Logs";
        private Mock<ILogger> _mockILogger;
        private Mock<IDisplayAlertHelper> _mockDisplayAlertHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockDisplayAlertHelper = new Mock<IDisplayAlertHelper>();
        }

        [TestMethod]
        public void LogsViewModel()
        {
            {
                // Arrange
                var logsViewModel = new LogsViewModel(_mockILogger.Object, _mockDisplayAlertHelper.Object);

                // Assert
                Assert.AreEqual(Title, logsViewModel.Title);
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
                var logsViewModel = new LogsViewModel(_mockILogger.Object, _mockDisplayAlertHelper.Object);

                // Act
                logsViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual(Title, logsViewModel.Title);
                Assert.IsNotNull(logsViewModel.Logs);
                Assert.AreEqual(noOfLogs, logsViewModel.Logs.Count);
            }
        }

        [TestMethod]
        public void LoadItemsCommand_Exception()
        {
            {
                const string exceptionMessage = "Error on GetLogsAsync!";

                // Arrange
                _mockILogger.Setup(x => x.GetLogsAsync()).Throws(new Exception(exceptionMessage));
                var logsViewModel = new LogsViewModel(_mockILogger.Object, _mockDisplayAlertHelper.Object);

                // Act
                logsViewModel.LoadItemsCommand.Execute(null);

                // Assert
                Assert.AreEqual(Title, logsViewModel.Title);
                Assert.AreEqual(0, logsViewModel.Logs.Count);
                _mockDisplayAlertHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
            }
        }
    }
}