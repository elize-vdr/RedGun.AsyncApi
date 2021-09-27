// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using RedGun.AsyncApi.Expressions;
using Xunit;

namespace RedGun.AsyncApi.Tests.Expressions
{
    [Collection("DefaultSettings")]
    public class StatusCodeExpressionTests
    {
        [Fact]
        public void StatusCodeExpressionReturnsCorrectExpression()
        {
            // Arrange & Act
            var statusCode = new StatusCodeExpression();

            // Assert
            Assert.Equal("$statusCode", statusCode.Expression);
        }
    }
}
