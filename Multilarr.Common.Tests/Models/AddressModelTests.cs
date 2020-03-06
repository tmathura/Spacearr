using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Models;
using Multilarr.Common.Tests.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Multilarr.Common.Tests.Models
{
    [TestClass]
    public class AddressModelTests
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
        public void Validate_InvalidComputerName(string computerName, string appId, string key, string secret, string cluster)
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
    }
}