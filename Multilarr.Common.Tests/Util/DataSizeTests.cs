using Microsoft.VisualStudio.TestTools.UnitTesting;
using Multilarr.Common.Util;

namespace Multilarr.Common.Tests.Util
{
    [TestClass]
    public class DataSizeTests
    {

        [DataTestMethod]
        [DataRow(1024, "1.0 KB")]
        [DataRow(1048576, "1.0 MB")]
        [DataRow(1073741824, "1.0 GB")]
        [DataRow(1099511627776, "1.0 TB")]
        [DataRow(1125899906842624, "1.0 PB")]
        [DataRow(1152921504606846976, "1.0 EB")]
        public void SizeSuffixTest(long sizeLong, string result)
        {
            // Arrange
            var size = DataSize.SizeSuffix(sizeLong);

            // Assert
            Assert.AreEqual(result, size);
        }
    }
}