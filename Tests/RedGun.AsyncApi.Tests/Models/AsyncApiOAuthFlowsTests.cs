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
    public class AsyncApiOAuthFlowsTests
    {
        public static AsyncApiOAuthFlows BasicOAuthFlows = new AsyncApiOAuthFlows();

        public static AsyncApiOAuthFlows OAuthFlowsWithSingleFlow = new AsyncApiOAuthFlows
        {
            Implicit = new AsyncApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://example.com/authorization"),
                Scopes = new Dictionary<string, string>
                {
                    ["scopeName1"] = "description1",
                    ["scopeName2"] = "description2"
                }
            }
        };

        public static AsyncApiOAuthFlows OAuthFlowsWithMultipleFlows = new AsyncApiOAuthFlows
        {
            Implicit = new AsyncApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://example.com/authorization"),
                Scopes = new Dictionary<string, string>
                {
                    ["scopeName1"] = "description1",
                    ["scopeName2"] = "description2"
                }
            },
            Password = new AsyncApiOAuthFlow
            {
                TokenUrl = new Uri("http://example.com/token"),
                RefreshUrl = new Uri("http://example.com/refresh"),
                Scopes = new Dictionary<string, string>
                {
                    ["scopeName3"] = "description3",
                    ["scopeName4"] = "description4"
                }
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiOAuthFlowsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeBasicOAuthFlowsAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{ }";

            // Act
            var actual = BasicOAuthFlows.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeBasicOAuthFlowsAsV2YamlWorks()
        {
            // Arrange
            var expected =
                @"{ }";

            // Act
            var actual = BasicOAuthFlows.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeOAuthFlowsWithSingleFlowAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""implicit"": {
    ""authorizationUrl"": ""http://example.com/authorization"",
    ""scopes"": {
      ""scopeName1"": ""description1"",
      ""scopeName2"": ""description2""
    }
  }
}";

            // Act
            var actual = OAuthFlowsWithSingleFlow.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeOAuthFlowsWithMultipleFlowsAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""implicit"": {
    ""authorizationUrl"": ""http://example.com/authorization"",
    ""scopes"": {
      ""scopeName1"": ""description1"",
      ""scopeName2"": ""description2""
    }
  },
  ""password"": {
    ""tokenUrl"": ""http://example.com/token"",
    ""refreshUrl"": ""http://example.com/refresh"",
    ""scopes"": {
      ""scopeName3"": ""description3"",
      ""scopeName4"": ""description4""
    }
  }
}";

            // Act
            var actual = OAuthFlowsWithMultipleFlows.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
