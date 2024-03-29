﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license.

using System;
using RedGun.AsyncApi.Expressions;
using Xunit;

namespace RedGun.AsyncApi.Tests.Expressions
{
    [Collection("DefaultSettings")]
    public class ResponseExpressionTests
    {
        [Fact]
        public void ResponseExpressionConstructorThrows()
        {
            // Arrange & Act
            Action test = () => new ResponseExpression(source: null);

            // Assert
            Assert.Throws<ArgumentNullException>("source", test);
        }

        [Fact]
        public void ResponseExpressionConstructorWorks()
        {
            // Arrange
            SourceExpression source = SourceExpression.Build("header.accept");

            // Act
            ResponseExpression response = new ResponseExpression(source);

            // Assert
            Assert.Same(source, response.Source);
            Assert.Equal("$response.header.accept", response.Expression);
            Assert.Equal("$response.header.accept", response.ToString());
        }
    }
}
