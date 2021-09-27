// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using RedGun.AsyncApi.Expressions;
using Xunit;

namespace RedGun.AsyncApi.Tests.Expressions
{
    [Collection("DefaultSettings")]
    public class UrlExpressionTests
    {
        [Fact]
        public void UrlExpressionReturnsCorrectExpression()
        {
            // Arrange & Act
            var url = new UrlExpression();

            // Assert
            Assert.Equal("$url", url.Expression);
        }
    }
}
