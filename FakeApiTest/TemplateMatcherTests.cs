using System;
using FakeApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeApiTest
{
    [TestClass]
    public class TemplateMatcherTests
    {
        [TestMethod]
        [DataRow("http://localhost/api/products/{productId}", "http://localhost/api/products/2")]
        [DataRow("http://192.156.34.34/api/products/{productId}", "http://192.156.34.34/api/products/2")]
        [DataRow("http://localhost/api/products/{productId}/something/{pId}", "http://localhost/api/products/2/something/abc")]
        [DataRow("http://localhost/api/products/{productId}/something?id={0}", "http://localhost/api/products/2/something?id=4")]
        [DataRow("http://localhost/api/products/{productId}/something?id=1&p1={45}", "http://localhost/api/products/2/something?id=1&p1=45")]
        public void ShouldReturnTrueWhenUrisMatch(string template, string uri)
        {
            Assert.IsTrue(TemplateMatcher.Match(new Uri(template), new Uri(uri)));
        }

        [TestMethod]
        [DataRow("http://localhost/api/products/{productId}", "http://123.23.23.23/api/products/2")]
        [DataRow("http://192.156.34.34/api/products", "http://192.156.34.34/api/products/2")]
        [DataRow("http://192.156.34.34/api/products/{productId}", "http://192.156.34.34/api/products")]
        [DataRow("http://localhost/api/products/{productId}/xxx/{pId}", "http://localhost/api/products/2/something/abc")]
        [DataRow("http://domain/api/products/{productId}/something/{pId}", "http://localhost/api/products/2/xxx/abc")]
        public void ShouldReturnFalseWhenUrisNotMatch(string template, string uri)
        {
            Assert.IsFalse(TemplateMatcher.Match(new Uri(template), new Uri(uri)));
        }

        [TestMethod]
        public void ShouldReturnFalseIfUriIsNull()
        {
            Assert.IsFalse(TemplateMatcher.Match(new Uri("http://localhost/api"), null));
            Assert.IsFalse(TemplateMatcher.Match(null, new Uri("http://localhost/api")));
        }

        [TestMethod]
        public void ShouldReturnFalseIfSchemesAreDifferents()
        {
            //Arrange
            var templateUri = new Uri("http://localhost/api/products");
            var uri = new Uri("https://localhost/api/products");

            Assert.IsFalse(TemplateMatcher.Match(templateUri, uri));
        }

        [TestMethod]
        public void ShouldReturnFalseIfQueriesParametersCountNotEquals()
        {
            //Arrange
            var templateUri = new Uri("http://localhost/api/products?p1=1&p2=2");
            var uri = new Uri("http://localhost/api/products?p1=1");

            Assert.IsFalse(TemplateMatcher.Match(templateUri, uri));
        }

        [TestMethod]
        public void ShouldReturnFalseIfQueriesParametersNotEquals()
        {
            //Arrange
            var templateUri = new Uri("http://localhost/api/products?x={xValue}&p1=1&p2=2");
            var uri = new Uri("http://localhost/api/products?x={xValue}&p3=1&p4=2");

            Assert.IsFalse(TemplateMatcher.Match(templateUri, uri));
        }
    }
}
