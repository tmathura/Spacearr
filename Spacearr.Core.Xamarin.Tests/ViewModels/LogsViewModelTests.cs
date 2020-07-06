using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Spacearr.Common.Interfaces.Logger;
using Spacearr.Core.Xamarin.Interfaces.Helpers;
using Spacearr.Core.Xamarin.Tests.Factories;
using Spacearr.Core.Xamarin.ViewModels;
using System;
using System.Threading.Tasks;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class LogsViewModelTests
    {
        private const string Title = "Logs";
        private Mock<ILogger> _mockILogger;
        private Mock<ILogsPageHelper> _mockILogsPageHelper;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockILogsPageHelper = new Mock<ILogsPageHelper>();
        }

        [TestMethod]
        public void LogsViewModel()
        {
            // Arrange
            var logsViewModel = new LogsViewModel(_mockILogger.Object, _mockILogsPageHelper.Object);

            // Assert
            Assert.AreEqual(Title, logsViewModel.Title);
        }

        [TestMethod]
        public async Task LoadItemsCommand()
        {
            const int noOfLogs = 7;

            // Arrange
            _mockILogger.Setup(x => x.GetLogsAsync()).ReturnsAsync(LogModelFactory.CreateLogModels(noOfLogs));
            var logsViewModel = new LogsViewModel(_mockILogger.Object, _mockILogsPageHelper.Object);

            // Act
            logsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            await Task.Delay(1010);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
            Assert.AreEqual(Title, logsViewModel.Title);
            Assert.IsNotNull(logsViewModel.Logs);
            Assert.AreEqual(noOfLogs, logsViewModel.Logs.Count);
        }

        [TestMethod]
        public async Task LoadItemsCommand_Exception()
        {
            const string exceptionMessage = "Error on GetLogsAsync!";

            // Arrange
            _mockILogger.Setup(x => x.GetLogsAsync()).Throws(new Exception(exceptionMessage));
            var logsViewModel = new LogsViewModel(_mockILogger.Object, _mockILogsPageHelper.Object);

            // Act
            logsViewModel.LoadItemsCommand.Execute(null);

            // Assert
            await Task.Delay(1000);  //Need this when getting data locally on the device otherwise keeps showing loading icon on Android.
            Assert.AreEqual(Title, logsViewModel.Title);
            Assert.AreEqual(0, logsViewModel.Logs.Count);
            _mockILogsPageHelper.Verify(x => x.CustomDisplayAlert("Error", exceptionMessage, "OK"), Times.Once);
        }
    }
}