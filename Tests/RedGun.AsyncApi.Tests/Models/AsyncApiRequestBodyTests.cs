// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiRequestBodyTests
    {
        public static AsyncApiRequestBody AdvancedRequestBody = new AsyncApiRequestBody
        {
            Description = "description",
            Required = true,
            Content =
            {
                ["application/json"] = new AsyncApiMediaType
                {
                    Schema = new AsyncApiSchema
                    {
                        Type = "string"
                    }
                }
            }
        };

        public static AsyncApiRequestBody ReferencedRequestBody = new AsyncApiRequestBody
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.RequestBody,
                Id = "example1",
            },
            Description = "description",
            Required = true,
            Content =
            {
                ["application/json"] = new AsyncApiMediaType
                {
                    Schema = new AsyncApiSchema
                    {
                        Type = "string"
                    }
                }
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiRequestBodyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeAdvancedRequestBodyAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""description"": ""description"",
  ""content"": {
    ""application/json"": {
      ""schema"": {
        ""type"": ""string""
      }
    }
  },
  ""required"": true
}";

            // Act
            AdvancedRequestBody.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedRequestBodyAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/requestBodies/example1""
}";

            // Act
            ReferencedRequestBody.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedRequestBodyAsV3JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""description"": ""description"",
  ""content"": {
    ""application/json"": {
      ""schema"": {
        ""type"": ""string""
      }
    }
  },
  ""required"": true
}";

            // Act
            ReferencedRequestBody.SerializeAsV3WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
