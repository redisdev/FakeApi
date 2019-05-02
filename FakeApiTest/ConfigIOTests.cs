using System.IO;
using FakeApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FakeApiTest
{
    [TestClass]
    public class ConfigIOTests
    {
        [TestMethod]
        public void GetConfigShouldThrowExceptionIfFileDoesNotExist()
        {
            var ex = Assert.ThrowsException<FileLoadException>(() =>
            {
                ConfigIO.GetConfig("fakePath");
            });

            //Assert
            Assert.AreEqual("File fakePath not found", ex.Message);
        }

        [TestMethod]
        public void GetConfigShouldThrowExceptionWhenConfigFileIsNotValid()
        {
            Assert.ThrowsException<JsonReaderException>(() =>
            {
                ConfigIO.GetConfig("DownloadFile.txt");
            });
        }

        [TestMethod]
        public void GetConfigShouldThrowExceptionWhenConfigIsNull()
        {
            var ex = Assert.ThrowsException<FileLoadException>(() =>
            {
                ConfigIO.GetConfig("BadConfigFile.json");
            });

            Assert.AreEqual("An error occured when deserialized file content", ex.Message);
        }
    }
}
