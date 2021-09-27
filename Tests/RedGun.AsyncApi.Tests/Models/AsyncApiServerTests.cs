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
    public class AsyncApiServerTests
    {
        public static AsyncApiServer BasicServer = new AsyncApiServer
        {
            Description = "description1",
            Url = "https://example.com/server1"
        };

        public static AsyncApiServer AdvancedServer = new AsyncApiServer
        {
            Description = "description1",
            Url = "https://{username}.example.com:{port}/{basePath}",
            Variables = new Dictionary<string, AsyncApiServerVariable>
            {
                ["username"] = new AsyncApiServerVariable
                {
                    Default = "unknown",
                    Description = "variableDescription1",
                },
                ["port"] = new AsyncApiServerVariable
                {
                    Default = "8443",
                    Description = "variableDescription2",
                    Enum = new List<string>
                    {
                        "443",
                        "8443"
                    }
                },
                ["basePath"] = new AsyncApiServerVariable
                {
                    Default = "v1"
                },
            }
        };

        [Fact]
        public void SerializeBasicServerAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""url"": ""https://example.com/server1"",
  ""description"": ""description1""
}";

            // Act
            var actual = BasicServer.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedServerAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""url"": ""https://{username}.example.com:{port}/{basePath}"",
  ""description"": ""description1"",
  ""variables"": {
    ""username"": {
      ""default"": ""unknown"",
      ""description"": ""variableDescription1""
    },
    ""port"": {
      ""default"": ""8443"",
      ""description"": ""variableDescription2"",
      ""enum"": [
        ""443"",
        ""8443""
      ]
    },
    ""basePath"": {
      ""default"": ""v1""
    }
  }
}";

            // Act
            var actual = AdvancedServer.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
