using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Models;
using Multilarr.Common.Tests.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Multilarr.Common.Tests.Models
{
    [TestClass]
    public class SettingLogTests
    {
        private ICollection<ValidationResult> _results;

        [TestMethod]
        public void Validate()
        {
            // Arrange
            var model = new SettingLog
            {
                ComputerName = "Computer Name",
                PusherAppId = "Pusher App Id",
                PusherKey = "PusherKey",
                PusherSecret = "Pusher Secret",
                PusherCluster = "Pusher Cluster"
            };

            // Act
            var passed = TestModel.Validate(model, out _results);

            // Assert
            Assert.IsTrue(passed);
        }

        [DataTestMethod]
        [DataRow(null, null, null, null, null)]
        [DataRow("Name", null, null, null, null)]
        [DataRow(null, "App Id", null, null, null)]
        [DataRow(null, null, "Key", null, null)]
        [DataRow(null, null, null, "Secret", null)]
        [DataRow(null, null, null, null, "Cluster")]
        [DataRow(null, "App Id", "Key", "Secret", "Cluster")]
        [DataRow(null, null, "Key", "Secret", "Cluster")]
        [DataRow(null, null, null, null, "Cluster")]
        [DataRow("Computer Name", "App Id", "Key", "Secret", null)]
        [DataRow("Computer Name", "App Id", "Key", null, null)]
        [DataRow("Computer Name", null, null, null, null)]
        public void Validate_Invalid(string computerName, string appId, string key, string secret, string cluster)
        {
            // Arrange
            var model = new SettingLog
            {
                ComputerName = computerName,
                PusherAppId = appId,
                PusherKey = key,
                PusherSecret = secret,
                PusherCluster = cluster
            };

            // Act
            var passed = TestModel.Validate(model, out _results);

            // Assert
            Assert.IsFalse(passed);
        }

        [TestMethod]
        public void Validate_InvalidDescription()
        {
            // Arrange
            var errorMessages = new List<string>
            {
                "Please specify the Computer Name.",
                "The Pusher App Id field is required.",
                "The Pusher Key field is required.",
                "The Pusher Secret field is required.",
                "The Pusher Cluster field is required."
            };
            var model = new SettingLog();

            // Act
            var passed = TestModel.Validate(model, out _results);

            // Assert
            Assert.IsFalse(passed);
            Assert.AreEqual(errorMessages.Count, _results.Count);

            foreach (var validationResult in _results)
            {
                Assert.IsTrue(errorMessages.Contains(validationResult.ToString()));
            }
        }
    }
}