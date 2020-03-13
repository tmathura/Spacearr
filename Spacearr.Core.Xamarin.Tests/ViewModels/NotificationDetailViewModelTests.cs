using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spacearr.Common.Models;
using Spacearr.Core.Xamarin.ViewModels;

namespace Spacearr.Core.Xamarin.Tests.ViewModels
{
    [TestClass]
    public class NotificationDetailViewModelTests
    {
        private const string Title = "Notification Title";

        [TestMethod]
        public void NotificationDetailViewModel()
        {
            var notificationEventArgsModel = new NotificationEventArgsModel
            {
                Title = Title
            };

            // Arrange
            var notificationDetailViewModel = new NotificationDetailViewModel(notificationEventArgsModel);

            // Assert
            Assert.AreEqual(Title, notificationDetailViewModel.Title);
            Assert.AreEqual(notificationEventArgsModel, notificationDetailViewModel.Notification);
        }
    }
}