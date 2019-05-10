using System;
using System.Collections.Generic;
using System.Linq;
using FakeApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeApiTest
{
    [TestClass]
    public class HttpResponseMockTest
    {
        [TestMethod]
        public void GetNextFile_ShouldReturnNextFileAndReset()
        {
            //Arrange
            var response = new HttpResponseMock();
            response.Files = new List<string>
            {
                "1",
                "2",
                "3"
            };

            //Act/Assert
            Assert.AreEqual("1", response.GetNextFile());
            Assert.AreEqual("2", response.GetNextFile());
            Assert.AreEqual("3", response.GetNextFile());
            Assert.AreEqual("1", response.GetNextFile());
        }

        [TestMethod]
        public void GetNextFile_ShouldThrowInvalidExceptionWhenFilesIsNull()
        {
            //Arrange
            var response = new HttpResponseMock();
            response.Files = null;

            //Act
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                response.GetNextFile();
            });
        }

        [TestMethod]
        public void GetNextFile_ShouldThrowInvalidExceptionWhenFilesIsEmpty()
        {
            //Arrange
            var response = new HttpResponseMock();
            response.Files = new List<string>();

            //Act
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                response.GetNextFile();
            });
        }
    }
}
