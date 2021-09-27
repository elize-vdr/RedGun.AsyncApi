// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiMediaTypeTests
    {
        public static AsyncApiMediaType BasicMediaType = new AsyncApiMediaType();

        public static AsyncApiMediaType AdvanceMediaType = new AsyncApiMediaType
        {
            Example = new AsyncApiInteger(42),
            Encoding = new Dictionary<string, AsyncApiEncoding>
            {
                {"testEncoding", AsyncApiEncodingTests.AdvanceEncoding}
            }
        };

        public static AsyncApiMediaType MediaTypeWithObjectExample = new AsyncApiMediaType
        {
            Example = new AsyncApiObject
            {
                ["versions"] = new AsyncApiArray
                {
                    new AsyncApiObject
                    {
                        ["status"] = new AsyncApiString("Status1"),
                        ["id"] = new AsyncApiString("v1"),
                        ["links"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["href"] = new AsyncApiString("http://example.com/1"),
                                ["rel"] = new AsyncApiString("sampleRel1")
                            }
                        }
                    },

                    new AsyncApiObject
                    {
                        ["status"] = new AsyncApiString("Status2"),
                        ["id"] = new AsyncApiString("v2"),
                        ["links"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["href"] = new AsyncApiString("http://example.com/2"),
                                ["rel"] = new AsyncApiString("sampleRel2")
                            }
                        }
                    }
                }
            },
            Encoding = new Dictionary<string, AsyncApiEncoding>
            {
                {"testEncoding", AsyncApiEncodingTests.AdvanceEncoding}
            }
        };

        public static AsyncApiMediaType MediaTypeWithXmlExample = new AsyncApiMediaType
        {
            Example = new AsyncApiString("<xml>123</xml>"),
            Encoding = new Dictionary<string, AsyncApiEncoding>
            {
                {"testEncoding", AsyncApiEncodingTests.AdvanceEncoding}
            }
        };

        public static AsyncApiMediaType MediaTypeWithObjectExamples = new AsyncApiMediaType
        {
            Examples = {
                ["object1"] = new AsyncApiExample
                {
                    Value = new AsyncApiObject
                    {
                        ["versions"] = new AsyncApiArray
                        {
                            new AsyncApiObject
                            {
                                ["status"] = new AsyncApiString("Status1"),
                                ["id"] = new AsyncApiString("v1"),
                                ["links"] = new AsyncApiArray
                                {
                                    new AsyncApiObject
                                    {
                                        ["href"] = new AsyncApiString("http://example.com/1"),
                                        ["rel"] = new AsyncApiString("sampleRel1")
                                    }
                                }
                            },

                            new AsyncApiObject
                            {
                                ["status"] = new AsyncApiString("Status2"),
                                ["id"] = new AsyncApiString("v2"),
                                ["links"] = new AsyncApiArray
                                {
                                    new AsyncApiObject
                                    {
                                        ["href"] = new AsyncApiString("http://example.com/2"),
                                        ["rel"] = new AsyncApiString("sampleRel2")
                                    }
                                }
                            }
                        }
                    }
                }
            },
            Encoding = new Dictionary<string, AsyncApiEncoding>
            {
                {"testEncoding", AsyncApiEncodingTests.AdvanceEncoding}
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiMediaTypeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(AsyncApiFormat.Json, "{ }")]
        [InlineData(AsyncApiFormat.Yaml, "{ }")]
        public void SerializeBasicMediaTypeAsV3Works(AsyncApiFormat format, string expected)
        {
            // Arrange & Act
            var actual = BasicMediaType.Serialize(AsyncApiSpecVersion.AsyncApi2_0, format);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvanceMediaTypeAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""example"": 42,
  ""encoding"": {
    ""testEncoding"": {
      ""contentType"": ""image/png, image/jpeg"",
      ""style"": ""simple"",
      ""explode"": true,
      ""allowReserved"": true
    }
  }
}";

            // Act
            var actual = AdvanceMediaType.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvanceMediaTypeAsV3YamlWorks()
        {
            // Arrange
            var expected =
                @"example: 42
encoding:
  testEncoding:
    contentType: 'image/png, image/jpeg'
    style: simple
    explode: true
    allowReserved: true";

            // Act
            var actual = AdvanceMediaType.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeMediaTypeWithObjectExampleAsV3YamlWorks()
        {
            // Arrange
            var expected =
                @"example:
  versions:
    - status: Status1
      id: v1
      links:
        - href: http://example.com/1
          rel: sampleRel1
    - status: Status2
      id: v2
      links:
        - href: http://example.com/2
          rel: sampleRel2
encoding:
  testEncoding:
    contentType: 'image/png, image/jpeg'
    style: simple
    explode: true
    allowReserved: true";

            // Act
            var actual = MediaTypeWithObjectExample.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeMediaTypeWithObjectExampleAsV3JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""example"": {
    ""versions"": [
      {
        ""status"": ""Status1"",
        ""id"": ""v1"",
        ""links"": [
          {
            ""href"": ""http://example.com/1"",
            ""rel"": ""sampleRel1""
          }
        ]
      },
      {
        ""status"": ""Status2"",
        ""id"": ""v2"",
        ""links"": [
          {
            ""href"": ""http://example.com/2"",
            ""rel"": ""sampleRel2""
          }
        ]
      }
    ]
  },
  ""encoding"": {
    ""testEncoding"": {
      ""contentType"": ""image/png, image/jpeg"",
      ""style"": ""simple"",
      ""explode"": true,
      ""allowReserved"": true
    }
  }
}";

            // Act
            var actual = MediaTypeWithObjectExample.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeMediaTypeWithXmlExampleAsV3YamlWorks()
        {
            // Arrange
            var expected =
                @"example: <xml>123</xml>
encoding:
  testEncoding:
    contentType: 'image/png, image/jpeg'
    style: simple
    explode: true
    allowReserved: true";

            // Act
            var actual = MediaTypeWithXmlExample.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeMediaTypeWithXmlExampleAsV3JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""example"": ""<xml>123</xml>"",
  ""encoding"": {
    ""testEncoding"": {
      ""contentType"": ""image/png, image/jpeg"",
      ""style"": ""simple"",
      ""explode"": true,
      ""allowReserved"": true
    }
  }
}";

            // Act
            var actual = MediaTypeWithXmlExample.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeMediaTypeWithObjectExamplesAsV3YamlWorks()
        {
            // Arrange
            var expected = @"examples:
  object1:
    value:
      versions:
        - status: Status1
          id: v1
          links:
            - href: http://example.com/1
              rel: sampleRel1
        - status: Status2
          id: v2
          links:
            - href: http://example.com/2
              rel: sampleRel2
encoding:
  testEncoding:
    contentType: 'image/png, image/jpeg'
    style: simple
    explode: true
    allowReserved: true";

            // Act
            var actual = MediaTypeWithObjectExamples.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);
            _output.WriteLine(actual);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeMediaTypeWithObjectExamplesAsV3JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""examples"": {
    ""object1"": {
      ""value"": {
        ""versions"": [
          {
            ""status"": ""Status1"",
            ""id"": ""v1"",
            ""links"": [
              {
                ""href"": ""http://example.com/1"",
                ""rel"": ""sampleRel1""
              }
            ]
          },
          {
            ""status"": ""Status2"",
            ""id"": ""v2"",
            ""links"": [
              {
                ""href"": ""http://example.com/2"",
                ""rel"": ""sampleRel2""
              }
            ]
          }
        ]
      }
    }
  },
  ""encoding"": {
    ""testEncoding"": {
      ""contentType"": ""image/png, image/jpeg"",
      ""style"": ""simple"",
      ""explode"": true,
      ""allowReserved"": true
    }
  }
}";

            // Act
            var actual = MediaTypeWithObjectExamples.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);
            _output.WriteLine(actual);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
