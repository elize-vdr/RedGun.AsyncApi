// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiExternalDocsTests
    {
        public static AsyncApiExternalDocs BasicExDocs = new AsyncApiExternalDocs();

        public static AsyncApiExternalDocs AdvanceExDocs = new AsyncApiExternalDocs
        {
            Url = new Uri("https://example.com"),
            Description = "Find more info here"
        };

        #region AsyncApi V3

        [Theory]
        [InlineData(AsyncApiFormat.Json, "{ }")]
        [InlineData(AsyncApiFormat.Yaml, "{ }")]
        public void SerializeBasicExternalDocsAsV3Works(AsyncApiFormat format, string expected)
        {
            // Arrange & Act
            var actual = BasicExDocs.Serialize(AsyncApiSpecVersion.AsyncApi2_0, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvanceExDocsAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""description"": ""Find more info here"",
  ""url"": ""https://example.com""
}";

            // Act
            var actual = AdvanceExDocs.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvanceExDocsAsV3YamlWorks()
        {
            // Arrange
            var expected =
                @"description: Find more info here
url: https://example.com";

            // Act
            var actual = AdvanceExDocs.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        #endregion

        #region AsyncApi V2

        #endregion
    }
}
