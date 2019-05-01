using System.IO;
using FakeApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeApiTest
{
    [TestClass]
    public class ConfigIOTests
    {
        [TestMethod]
        public void GetConfigShouldThrowExceptionIfFileDoesNotExist()
        {
            Assert.ThrowsException<FileLoadException>(() =>
            {
                ConfigIO.GetConfig("fakePath");
            });
        }
    }
}
