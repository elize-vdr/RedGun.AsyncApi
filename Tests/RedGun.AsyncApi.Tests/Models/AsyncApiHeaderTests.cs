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
    public class AsyncApiHeaderTests
    {
        public static AsyncApiHeader AdvancedHeader = new AsyncApiHeader
        {
            Description = "sampleHeader",
            Schema = new AsyncApiSchema
            {
                Type = "integer",
                Format = "int32"
            }
        };

        public static AsyncApiHeader ReferencedHeader = new AsyncApiHeader
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Header,
                Id = "example1",
            },
            Description = "sampleHeader",
            Schema = new AsyncApiSchema
            {
                Type = "integer",
                Format = "int32"
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiHeaderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /* TODO: There are only V2 tests here and no OpenApi V3 which is our AsyncApi 2 equivalent, so can this file be deleted??
        [Fact]
        public void SerializeAdvancedHeaderAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""description"": ""sampleHeader"",
  ""schema"": {
    ""type"": ""integer"",
    ""format"": ""int32""
  }
}";

            // Act
            AdvancedHeader.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedHeaderAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/headers/example1""
}";

            // Act
            ReferencedHeader.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedHeaderAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""description"": ""sampleHeader"",
  ""schema"": {
    ""type"": ""integer"",
    ""format"": ""int32""
  }
}";

            // Act
            ReferencedHeader.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
        */
    }
}
