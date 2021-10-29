// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiResponseTests
    {
        public static AsyncApiResponse BasicResponse = new AsyncApiResponse();

        public static AsyncApiResponse AdvancedResponse = new AsyncApiResponse
        {
            Description = "A complex object array response",
            Content =
            {
                ["text/plain"] = new AsyncApiMediaType
                {
                    Schema = new AsyncApiSchema
                    {
                        Type = "array",
                        Items = new AsyncApiSchema
                        {
                            Reference = new AsyncApiReference {Type = ReferenceType.Schema, Id = "customType"}
                        }
                    },
                    Example = new AsyncApiString("Blabla"),
                    Extensions = new Dictionary<string, IAsyncApiExtension>
                    {
                        ["myextension"] = new AsyncApiString("myextensionvalue"),
                    },
                }
            },
            Headers =
            {
                ["X-Rate-Limit-Limit"] = new AsyncApiHeader
                {
                    Description = "The number of allowed requests in the current period",
                    Schema = new AsyncApiSchema
                    {
                        Type = "integer"
                    }
                },
                ["X-Rate-Limit-Reset"] = new AsyncApiHeader
                {
                    Description = "The number of seconds left in the current period",
                    Schema = new AsyncApiSchema
                    {
                        Type = "integer"
                    }
                },
            }
        };

        public static AsyncApiResponse ReferencedResponse = new AsyncApiResponse
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Response,
                Id = "example1"
            },
            Description = "A complex object array response",
            Content =
            {
                ["text/plain"] = new AsyncApiMediaType
                {
                    Schema = new AsyncApiSchema
                    {
                        Type = "array",
                        Items = new AsyncApiSchema
                        {
                            Reference = new AsyncApiReference {Type = ReferenceType.Schema, Id = "customType"}
                        }
                    }
                }
            },
            Headers =
            {
                ["X-Rate-Limit-Limit"] = new AsyncApiHeader
                {
                    Description = "The number of allowed requests in the current period",
                    Schema = new AsyncApiSchema
                    {
                        Type = "integer"
                    }
                },
                ["X-Rate-Limit-Reset"] = new AsyncApiHeader
                {
                    Description = "The number of seconds left in the current period",
                    Schema = new AsyncApiSchema
                    {
                        Type = "integer"
                    }
                },
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiResponseTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0, AsyncApiFormat.Json)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Json)]
        //[InlineData(AsyncApiSpecVersion.AsyncApi3_0, AsyncApiFormat.Yaml)]
        [InlineData(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Yaml)]
        public void SerializeBasicResponseWorks(
            AsyncApiSpecVersion version,
            AsyncApiFormat format)
        {
            // Arrange
            var expected = format == AsyncApiFormat.Json ? @"{
  ""description"": null
}" : @"description: ";

            // Act
            var actual = BasicResponse.Serialize(version, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedResponseAsV2JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""description"": ""A complex object array response"",
  ""headers"": {
    ""X-Rate-Limit-Limit"": {
      ""description"": ""The number of allowed requests in the current period"",
      ""schema"": {
        ""type"": ""integer""
      }
    },
    ""X-Rate-Limit-Reset"": {
      ""description"": ""The number of seconds left in the current period"",
      ""schema"": {
        ""type"": ""integer""
      }
    }
  },
  ""content"": {
    ""text/plain"": {
      ""schema"": {
        ""type"": ""array"",
        ""items"": {
          ""$ref"": ""#/components/schemas/customType""
        }
      },
      ""example"": ""Blabla"",
      ""myextension"": ""myextensionvalue""
    }
  }
}";

            // Act
            var actual = AdvancedResponse.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedResponseAsV2YamlWorks()
        {
            // Arrange
            var expected =
                @"description: A complex object array response
headers:
  X-Rate-Limit-Limit:
    description: The number of allowed requests in the current period
    schema:
      type: integer
  X-Rate-Limit-Reset:
    description: The number of seconds left in the current period
    schema:
      type: integer
content:
  text/plain:
    schema:
      type: array
      items:
        $ref: '#/components/schemas/customType'
    example: Blabla
    myextension: myextensionvalue";

            // Act
            var actual = AdvancedResponse.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedResponseAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/responses/example1""
}";

            // Act
            ReferencedResponse.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedResponseAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""description"": ""A complex object array response"",
  ""headers"": {
    ""X-Rate-Limit-Limit"": {
      ""description"": ""The number of allowed requests in the current period"",
      ""schema"": {
        ""type"": ""integer""
      }
    },
    ""X-Rate-Limit-Reset"": {
      ""description"": ""The number of seconds left in the current period"",
      ""schema"": {
        ""type"": ""integer""
      }
    }
  },
  ""content"": {
    ""text/plain"": {
      ""schema"": {
        ""type"": ""array"",
        ""items"": {
          ""$ref"": ""#/components/schemas/customType""
        }
      }
    }
  }
}";

            // Act
            ReferencedResponse.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
