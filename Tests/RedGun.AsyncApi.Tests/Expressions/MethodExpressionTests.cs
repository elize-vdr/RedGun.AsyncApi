// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using RedGun.AsyncApi.Expressions;
using Xunit;

namespace RedGun.AsyncApi.Tests.Expressions
{
    [Collection("DefaultSettings")]
    public class MethodExpressionTests
    {
        [Fact]
        public void MethodExpressionReturnsCorrectExpression()
        {
            // Arrange & Act
            var method = new MethodExpression();

            // Assert
            Assert.Equal("$method", method.Expression);
        }
    }
}
