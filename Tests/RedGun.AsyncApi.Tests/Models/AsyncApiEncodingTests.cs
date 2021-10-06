// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiEncodingTests
    {
        public static AsyncApiEncoding BasicEncoding = new AsyncApiEncoding();

        public static AsyncApiEncoding AdvanceEncoding = new AsyncApiEncoding
        {
            ContentType = "image/png, image/jpeg",
            Style = ParameterStyle.Simple,
            Explode = true,
            AllowReserved = true,
        };

        [Theory]
        [InlineData(AsyncApiFormat.Json, "{ }")]
        [InlineData(AsyncApiFormat.Yaml, "{ }")]
        public void SerializeBasicEncodingAsV3Works(AsyncApiFormat format, string expected)
        {
            // Arrange & Act
            var actual = BasicEncoding.Serialize(AsyncApiSpecVersion.AsyncApi2_0, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvanceEncodingAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""contentType"": ""image/png, image/jpeg"",
  ""style"": ""simple"",
  ""explode"": true,
  ""allowReserved"": true
}";

            // Act
            var actual = AdvanceEncoding.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvanceEncodingAsV3YamlWorks()
        {
            // Arrange
            var expected =
                @"contentType: 'image/png, image/jpeg'
style: simple
explode: true
allowReserved: true";

            // Act
            var actual = AdvanceEncoding.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
