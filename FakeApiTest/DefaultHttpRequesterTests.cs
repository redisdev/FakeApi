using System;
using System.Net;
using FakeApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeApiTest
{

    [TestClass]
    public class DefaultHttpRequesterTests
    {
        [TestMethod]
        public void ShouldThrowArgumentNullException()
        {
            //Arrange
            var defaultHttpRequester = new DefaultHttpRequester();

            //Act
            var ex = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                defaultHttpRequester.GetResponse(null);
            });

            //Assert
            Assert.AreEqual("request", ex.ParamName);
        }

        [TestMethod]
        public void ShouldThrowArgumentNullExceptionAsync()
        {
            //Arrange
            var defaultHttpRequester = new DefaultHttpRequester();

            //Act
            var ex = Assert.ThrowsException<AggregateException>(() =>
            {
                defaultHttpRequester.GetResponseAsync(null).Wait();
            });

            //Assert
            ex.Handle((Exception arg) =>
            {
                var argException = arg as ArgumentNullException;
                Assert.IsNotNull(argException);
                Assert.AreSame("request", argException.ParamName);
                return true;
            });
        }

        [TestMethod]
        public void ShouldReturnHttpWebResponse()
        {
            //Arrange
            var defaultHttpRequester = new DefaultHttpRequester();
            var webRequest = WebRequest.Create("http://dummy.restapiexample.com/api/v1/employee/38763");

            //Act
            var response = defaultHttpRequester.GetResponse(webRequest);

            //Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void ShouldReturnHttpWebResponseAsync()
        {
            //Arrange
            var defaultHttpRequester = new DefaultHttpRequester();
            var webRequest = WebRequest.Create("http://dummy.restapiexample.com/api/v1/employee/38763");

            //Act
            var response = defaultHttpRequester.GetResponseAsync(webRequest);

            //Assert
            Assert.IsNotNull(response.Result);
        }
    }
}
