// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Expressions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiCallbackTests
    {
        public static AsyncApiCallback AdvancedCallback = new AsyncApiCallback
        {
            PathItems =
            {
                [RuntimeExpression.Build("$request.body#/url")]
                = new AsyncApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Post] =
                        new AsyncApiOperation
                        {
                            RequestBody = new AsyncApiRequestBody
                            {
                                Content =
                                {
                                    ["application/json"] = new AsyncApiMediaType
                                    {
                                        Schema = new AsyncApiSchema
                                        {
                                            Type = "object"
                                        }
                                    }
                                }
                            },
                            Responses = new AsyncApiResponses
                            {
                                ["200"] = new AsyncApiResponse
                                {
                                    Description = "Success"
                                }
                            }
                        }
                    }
                }
            }
        };

        public static AsyncApiCallback ReferencedCallback = new AsyncApiCallback
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Callback,
                Id = "simpleHook",
            },
            PathItems =
            {
                [RuntimeExpression.Build("$request.body#/url")]
                = new AsyncApiPathItem
                {
                    Operations =
                    {
                        [OperationType.Post] =
                        new AsyncApiOperation
                        {
                            RequestBody = new AsyncApiRequestBody
                            {
                                Content =
                                {
                                    ["application/json"] = new AsyncApiMediaType
                                    {
                                        Schema = new AsyncApiSchema
                                        {
                                            Type = "object"
                                        }
                                    }
                                }
                            },
                            Responses = new AsyncApiResponses
                            {
                                ["200"] = new AsyncApiResponse
                                {
                                    Description = "Success"
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiCallbackTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeAdvancedCallbackAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$request.body#/url"": {
    ""post"": {
      ""requestBody"": {
        ""content"": {
          ""application/json"": {
            ""schema"": {
              ""type"": ""object""
            }
          }
        }
      },
      ""responses"": {
        ""200"": {
          ""description"": ""Success""
        }
      }
    }
  }
}";

            // Act
            AdvancedCallback.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedCallbackAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/callbacks/simpleHook""
}";

            // Act
            ReferencedCallback.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedCallbackAsV3JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$request.body#/url"": {
    ""post"": {
      ""requestBody"": {
        ""content"": {
          ""application/json"": {
            ""schema"": {
              ""type"": ""object""
            }
          }
        }
      },
      ""responses"": {
        ""200"": {
          ""description"": ""Success""
        }
      }
    }
  }
}";

            // Act
            ReferencedCallback.SerializeAsV3WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
