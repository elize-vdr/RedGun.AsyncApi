// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiServerVariableTests
    {
        public static AsyncApiServerVariable BasicServerVariable = new AsyncApiServerVariable();

        public static AsyncApiServerVariable AdvancedServerVariable = new AsyncApiServerVariable
        {
            Default = "8443",
            Enum = new List<string>
            {
                "8443",
                "443"
            },
            Description = "test description"
        };

        [Theory]
        [InlineData(AsyncApiFormat.Json, "{ }")]
        [InlineData(AsyncApiFormat.Yaml, "{ }")]
        public void SerializeBasicServerVariableAsV2Works(AsyncApiFormat format, string expected)
        {
            // Arrange & Act
            var actual = BasicServerVariable.Serialize(AsyncApiSpecVersion.AsyncApi2_0, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedServerVariableAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""default"": ""8443"",
  ""description"": ""test description"",
  ""enum"": [
    ""8443"",
    ""443""
  ]
}";

            // Act
            var actual = AdvancedServerVariable.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedServerVariableAsV2YamlWorks()
        {
            // Arrange
            var expected =
                @"default: '8443'
description: test description
enum:
  - '8443'
  - '443'";

            // Act
            var actual = AdvancedServerVariable.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
