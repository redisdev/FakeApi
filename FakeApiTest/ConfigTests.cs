﻿using System;
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
            Assert.AreEqual(ex.Message, "No apis configured");
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
            Assert.AreEqual(ex.Message, "No apis configured");
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
            Assert.AreEqual(ex.Message, "At least one api has no response configured");
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
            Assert.AreEqual(ex.Message, "At least one api has no http method configured");
        }
    }
}