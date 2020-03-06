using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Util;

namespace Multilarr.Common.Tests.Util
{
    [TestClass]
    public class DataSizeTests
    {

        [TestMethod]
        public void SizeSuffixTestKb()
        {
            // Arrange
            var size = DataSize.SizeSuffix(1024);

            // Assert
            Assert.AreEqual("1.0 KB", size);
        }

        [TestMethod]
        public void SizeSuffixTestMb()
        {
            // Arrange
            var size = DataSize.SizeSuffix(1048576);


            // Assert
            Assert.AreEqual("1.0 MB", size);
        }

        [TestMethod]
        public void SizeSuffixTestGb()
        {
            // Arrange
            var size = DataSize.SizeSuffix(1073741824);


            // Assert
            Assert.AreEqual("1.0 GB", size);
        }

        [TestMethod]
        public void SizeSuffixTestTb()
        {
            // Arrange
            var size = DataSize.SizeSuffix(1099511627776);


            // Assert
            Assert.AreEqual("1.0 TB", size);
        }

        [TestMethod]
        public void SizeSuffixTestPb()
        {
            // Arrange
            var size = DataSize.SizeSuffix(1125899906842624);


            // Assert
            Assert.AreEqual("1.0 PB", size);
        }

        [TestMethod]
        public void SizeSuffixTestEb()
        {
            // Arrange
            var size = DataSize.SizeSuffix(1152921504606846976);


            // Assert
            Assert.AreEqual("1.0 EB", size);
        }
    }
}