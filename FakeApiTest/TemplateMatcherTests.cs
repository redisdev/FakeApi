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
    }
}
