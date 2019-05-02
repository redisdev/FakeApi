using System;
using System.Collections.Generic;
using FakeApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeApiTest
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void ShouldThrowExceptionIfApisIsNull()
        {
            //Arrange
            var config = new Config();

            //Act
            var ex = Assert.ThrowsException<Exception>(() =>
            {
                config.Validate();
            });

            //Assert
            Assert.AreEqual("No apis configured", ex.Message);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfAnyApiUrlIsNullOrEmpty()
        {
            //Arrange
            var config = new Config();
            config.DefaultMethod = "GET";
            var apis = new List<ApiConfig>();
            apis.Add(new ApiConfig
            {
                Url = string.Empty
            });
            config.Apis = apis;

            //Act
            var ex = Assert.ThrowsException<Exception>(() =>
            {
                config.Validate();
            });

            //Assert
            Assert.AreEqual("At least one api has no url configured", ex.Message);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfUrlParameterIsNull()
        {
            //Arrange
            var config = new Config();

            //Act
            var ex = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                config.GetResponseMock(null, "GET");
            });

            //Assert
            Assert.AreEqual("url", ex.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfMethodParameterIsNull()
        {
            //Arrange
            var config = new Config();

            //Act
            var ex = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                config.GetResponseMock("http://localhost/api/get-users", null);
            });

            //Assert
            Assert.AreEqual("method", ex.ParamName);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfApisIsEmpty()
        {
            //Arrange
            var config = new Config();
            config.Apis = new List<ApiConfig>();

            //Act
            var ex = Assert.ThrowsException<Exception>(() =>
            {
                config.Validate();
            });

            //Assert
            Assert.AreEqual("No apis configured", ex.Message);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfAtLeastOneApiApiHasNoResponseAndDefaultResponseIsNull()
        {
            //Arrange
            var config = new Config();
            config.DefaultMethod = "GET";
            var apis = new List<ApiConfig>();
            apis.Add(new ApiConfig
            {
                Url = "http://localhost/api/users"
            });
            config.Apis = apis;

            //Act
            var ex = Assert.ThrowsException<Exception>(() =>
            {
                config.Validate();
            });

            //Assert
            Assert.AreEqual("At least one api has no response configured", ex.Message);
        }

        [TestMethod]
        public void ShouldThrowExceptionIfAtLeastOneApiApiHasNoMethodAndDefaultMethodIsNull()
        {
            //Arrange
            var config = new Config();
            var apis = new List<ApiConfig>();
            apis.Add(new ApiConfig
            {
                Url = "http://localhost/api/users"
            });
            config.Apis = apis;

            //Act
            var ex = Assert.ThrowsException<Exception>(() =>
            {
                config.Validate();
            });

            //Assert
            Assert.AreEqual("At least one api has no http method configured", ex.Message);
        }

        [TestMethod]
        public void GetResponseMock_ShouldThrowExceptionIfApiIsNull()
        {
            //Arrange
            var config = new Config();

            //Act
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                config.GetResponseMock("http://localhost", "method");
            });

            //Assert
            Assert.AreEqual("Try to get fake response for url http://localhost but apis config not loaded", ex.Message);
        }
    }
}
