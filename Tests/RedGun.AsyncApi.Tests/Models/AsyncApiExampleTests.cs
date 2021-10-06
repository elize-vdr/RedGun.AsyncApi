// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Globalization;
using System.IO;
using System.Text;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiExampleTests
    {
        public static AsyncApiExample AdvancedExample = new AsyncApiExample
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
                                ["rel"] = new AsyncApiString("sampleRel1"),
                                ["bytes"] = new AsyncApiByte(new byte[] { 1, 2, 3 }),
                                ["binary"] = new AsyncApiBinary(Encoding.UTF8.GetBytes("Ñ😻😑♮Í☛oƞ♑😲☇éǋžŁ♻😟¥a´Ī♃ƠąøƩ"))
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
        };

        public static AsyncApiExample ReferencedExample = new AsyncApiExample
        {
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Example,
                Id = "example1",
            },
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
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiExampleTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeAdvancedExampleAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""value"": {
    ""versions"": [
      {
        ""status"": ""Status1"",
        ""id"": ""v1"",
        ""links"": [
          {
            ""href"": ""http://example.com/1"",
            ""rel"": ""sampleRel1"",
            ""bytes"": ""AQID"",
            ""binary"": ""Ñ😻😑♮Í☛oƞ♑😲☇éǋžŁ♻😟¥a´Ī♃ƠąøƩ""
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
}";

            // Act
            AdvancedExample.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedExampleAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""$ref"": ""#/components/examples/example1""
}";

            // Act
            ReferencedExample.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedExampleAsV3JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
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
}";

            // Act
            ReferencedExample.SerializeAsV3WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
