// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System;
using RedGun.AsyncApi.Expressions;
using Xunit;

namespace RedGun.AsyncApi.Tests.Expressions
{
    [Collection("DefaultSettings")]
    public class BodyExpressionTests
    {
        [Fact]
        public void BodyExpressionThrowsWithNullPointerConstructor()
        {
            // Arrange
            Action test = () => new BodyExpression(null);

            // Act
            Assert.Throws<ArgumentNullException>("pointer", test);
        }

        [Fact]
        public void BodyExpressionWorksWithDefaultConstructor()
        {
            // Arrange & Act
            var body = new BodyExpression();

            // Assert
            Assert.Equal("body", body.Expression);
        }

        [Fact]
        public void BodyExpressionConstructorCreateCorrectBodyExpression()
        {
            // Arrange
            string expression = "#/user/uuid";
            JsonPointer pointer = new JsonPointer(expression);

            // Act
            var body = new BodyExpression(pointer);

            // Assert
            Assert.Equal("body#/user/uuid", body.Expression);
            Assert.Equal("/user/uuid", body.Fragment);
        }
    }
}
