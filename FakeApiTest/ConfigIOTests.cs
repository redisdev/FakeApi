using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                ConfigIO.GetConfig("Files/DownloadFile.txt");
            });
        }

        [TestMethod]
        public void GetConfigShouldThrowExceptionWhenConfigIsNull()
        {
            var ex = Assert.ThrowsException<FileLoadException>(() =>
            {
                ConfigIO.GetConfig("Files/BadConfigFile.json");
            });

            //Assert
            Assert.AreEqual("An error occured when deserialized file Files/BadConfigFile.json", ex.Message);
        }

        [TestMethod]
        public void WriteConfig_ShouldThrowExceptionIfConfigIsNull()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                ConfigIO.WriteConfig(null, "path");
            });

            //Assert
            Assert.AreEqual("config", ex.ParamName);
        }

        [TestMethod]
        public void ShouldMergeApisConfigFiles()
        {
            //Act
            var config = ConfigIO.GetConfig("Files/Config/Api/api.cfg.json");

            //Assert
            Assert.AreEqual(4, config.Apis.Count());
        }

        [TestMethod]
        public void MergeApis_ShouldThrowExceptionWhenDirectoryNotFound()
        {
            //Arrange
            var config = new Config();
            config.ApisDirectories = new List<string> { "fakeDirectories" };

            //Assert
            var ex = Assert.ThrowsException<DirectoryNotFoundException>(() =>
            {
                ConfigIO.MergeApis(config, "configSource");
            });
            Assert.AreEqual("Directory fakeDirectories not found", ex.Message);
        }
    }
}
