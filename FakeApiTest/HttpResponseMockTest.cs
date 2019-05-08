using System.Collections.Generic;
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
    }
}
