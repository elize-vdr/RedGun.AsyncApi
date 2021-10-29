// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiContactTests
    {
        public static AsyncApiContact BasicContact = new AsyncApiContact();

        public static AsyncApiContact AdvanceContact = new AsyncApiContact
        {
            Name = "API Support",
            Url = new Uri("http://www.example.com/support"),
            Email = "support@example.com",
            Extensions = new Dictionary<string, IAsyncApiExtension>
            {
                {"x-internal-id", new AsyncApiInteger(42)}
            }
        };

        [Theory]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Json, "{ }")]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0, AsyncApiFormat.Json, "{ }")]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Yaml, "{ }")]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0, AsyncApiFormat.Yaml, "{ }")]
        public void SerializeBasicContactWorks(
            AsyncApiSpecVersion version,
            AsyncApiFormat format,
            string expected)
        {
            // Arrange & Act
            var actual = BasicContact.Serialize(version, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        public void SerializeAdvanceContactAsJsonWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"{
  ""name"": ""API Support"",
  ""url"": ""http://www.example.com/support"",
  ""email"": ""support@example.com"",
  ""x-internal-id"": 42
}";

            // Act
            var actual = AdvanceContact.SerializeAsJson(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0)]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0)]
        public void SerializeAdvanceContactAsYamlWorks(AsyncApiSpecVersion version)
        {
            // Arrange
            var expected =
                @"name: API Support
url: http://www.example.com/support
email: support@example.com
x-internal-id: 42";

            // Act
            var actual = AdvanceContact.SerializeAsYaml(version);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
