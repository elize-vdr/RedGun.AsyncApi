// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System;
using RedGun.AsyncApi.Expressions;
using Xunit;

namespace RedGun.AsyncApi.Tests.Expressions
{
    [Collection("DefaultSettings")]
    public class HeaderExpressionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("        ")]
        public void HeaderExpressionConstructorThrows(string token)
        {
            // Arrange
            Action test = () => new HeaderExpression(token);

            // Act
            Assert.Throws<ArgumentException>("token", test);
        }

        [Fact]
        public void BodyExpressionWorksWithConstructor()
        {
            // Arrange
            string expression = "accept";

            // Act
            var header = new HeaderExpression(expression);

            // Assert
            Assert.Equal("header.accept", header.Expression);
            Assert.Equal("accept", header.Token);
        }
    }
}
