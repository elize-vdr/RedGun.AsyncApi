// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiOAuthFlowTests
    {
        public static AsyncApiOAuthFlow BasicOAuthFlow = new AsyncApiOAuthFlow();

        public static AsyncApiOAuthFlow PartialOAuthFlow = new AsyncApiOAuthFlow
        {
            AuthorizationUrl = new Uri("http://example.com/authorization"),
            Scopes = new Dictionary<string, string>
            {
                ["scopeName3"] = "description3",
                ["scopeName4"] = "description4"
            }
        };

        public static AsyncApiOAuthFlow CompleteOAuthFlow = new AsyncApiOAuthFlow
        {
            AuthorizationUrl = new Uri("http://example.com/authorization"),
            TokenUrl = new Uri("http://example.com/token"),
            RefreshUrl = new Uri("http://example.com/refresh"),
            Scopes = new Dictionary<string, string>
            {
                ["scopeName3"] = "description3",
                ["scopeName4"] = "description4"
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiOAuthFlowTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeBasicOAuthFlowAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""scopes"": { }
}";

            // Act
            var actual = BasicOAuthFlow.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeBasicOAuthFlowAsV3YamlWorks()
        {
            // Arrange
            var expected =
                @"scopes: { }";

            // Act
            var actual = BasicOAuthFlow.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializePartialOAuthFlowAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""authorizationUrl"": ""http://example.com/authorization"",
  ""scopes"": {
    ""scopeName3"": ""description3"",
    ""scopeName4"": ""description4""
  }
}";

            // Act
            var actual = PartialOAuthFlow.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeCompleteOAuthFlowAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""authorizationUrl"": ""http://example.com/authorization"",
  ""tokenUrl"": ""http://example.com/token"",
  ""refreshUrl"": ""http://example.com/refresh"",
  ""scopes"": {
    ""scopeName3"": ""description3"",
    ""scopeName4"": ""description4""
  }
}";

            // Act
            var actual = CompleteOAuthFlow.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
