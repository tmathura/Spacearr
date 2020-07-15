using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Octokit;
using Spacearr.Common.Enums;
using Spacearr.Common.Logger.Interfaces;
using Spacearr.Common.Services.Implementations;
using Spacearr.Common.Tests.Factories;
using System;
using System.Threading.Tasks;

namespace Spacearr.Common.Tests.Services
{
    [TestClass]
    public class UpdateServiceTests
    {
        private Mock<ILogger> _mockILogger;
        private Mock<IGitHubClient> _mockIGitHubClient;
        private Version _currentVersion = new Version("1.0.0.0");
        private UpdateService _updateService;

        [TestInitialize]
        public void SetUp()
        {
            _mockILogger = new Mock<ILogger>();
            _mockIGitHubClient = new Mock<IGitHubClient>();
            _currentVersion = new Version("1.0.0.0");
        }

        [TestMethod]
        public async Task CheckForUpdateAsync_NeedUpdate()
        {
            // Arrange
            const int noOfReleases = 20;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());

            // Assert
            Assert.AreEqual(true, update);
        }

        [TestMethod]
        public async Task CheckForUpdateAsync_DoesNotNeedUpdate()
        {
            // Arrange
            const int noOfReleases = 1;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());

            // Assert
            Assert.AreEqual(false, update);
        }

        [TestMethod]
        public async Task UpdateUrlOfLastUpdateCheck_Apk()
        {
            // Arrange
            const int noOfReleases = 7;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());
            var url = await _updateService.UpdateUrlOfLastUpdateCheck(UpdateType.Android);

            // Assert
            Assert.AreEqual(true, update);
            Assert.AreEqual($"browserDownloadUrl_apk_{noOfReleases-1}", url);
        }

        [TestMethod]
        public async Task UpdateUrlOfLastUpdateCheck_WindowsService()
        {
            // Arrange
            const int noOfReleases = 9;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());
            var url = await _updateService.UpdateUrlOfLastUpdateCheck(UpdateType.WindowsService);

            // Assert
            Assert.AreEqual(true, update);
            Assert.AreEqual($"browserDownloadUrl_windowsservice_{noOfReleases-1}", url);
        }

        [TestMethod]
        public async Task UpdateUrlOfLastUpdateCheck_WorkerService()
        {
            // Arrange
            const int noOfReleases = 11;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());
            var url = await _updateService.UpdateUrlOfLastUpdateCheck(UpdateType.WorkerService);

            // Assert
            Assert.AreEqual(true, update);
            Assert.AreEqual($"browserDownloadUrl_workerservice_{noOfReleases-1}", url);
        }

        [TestMethod]
        public async Task UpdateUrlOfLastUpdateCheck_Uwp()
        {
            // Arrange
            const int noOfReleases = 15;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());
            var url = await _updateService.UpdateUrlOfLastUpdateCheck(UpdateType.Uwp);

            // Assert
            Assert.AreEqual(true, update);
            Assert.AreEqual($"browserDownloadUrl_uwp_{noOfReleases-1}", url);
        }

        [TestMethod]
        public async Task UpdateUrlOfLastUpdateCheck_DidNotRunCheckForUpdateAsyncFirst()
        {
            // Arrange
            const int noOfReleases = 15;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);
            
            // Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _updateService.UpdateUrlOfLastUpdateCheck(UpdateType.Android));
        }

        [TestMethod]
        public async Task LatestTagName()
        {
            // Arrange
            const int noOfReleases = 3;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);

            // Act
            var update = await _updateService.CheckForUpdateAsync(_currentVersion.ToString());

            // Assert
            Assert.AreEqual(true, update);
            Assert.AreEqual($"1.0.0.{noOfReleases-1}", _updateService.LatestTagName);
        }

        [TestMethod]
        public void LatestTagName_DidNotRunCheckForUpdateAsyncFirst()
        {
            // Arrange
            const int noOfReleases = 3;
            var releases = OctokitRelease.CreateOctokitReleases(noOfReleases);
            _mockIGitHubClient.Setup(x => x.Repository.Release.GetAll("tmathura", "Spacearr")).ReturnsAsync(releases);
            _updateService = new UpdateService(_mockILogger.Object, _mockIGitHubClient.Object);
            
            // Assert
            Assert.ThrowsException<Exception>(() => _updateService.LatestTagName);
        }
    }
}