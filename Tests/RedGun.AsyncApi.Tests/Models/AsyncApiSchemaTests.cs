// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiSchemaTests
    {
        public static AsyncApiSchema BasicSchema = new AsyncApiSchema();

        public static AsyncApiSchema AdvancedSchemaNumber = new AsyncApiSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = 42,
            ExclusiveMinimum = true,
            Minimum = 10,
            Default = new AsyncApiInteger(15),
            Type = "integer",

            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocs
            {
                Url = new Uri("http://example.com/externalDocs")
            }
        };

        public static AsyncApiSchema AdvancedSchemaObject = new AsyncApiSchema
        {
            Title = "title1",
            Properties = new Dictionary<string, AsyncApiSchema>
            {
                ["property1"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = "integer"
                        },
                        ["property3"] = new AsyncApiSchema
                        {
                            Type = "string",
                            MaxLength = 15
                        }
                    },
                },
                ["property4"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property5"] = new AsyncApiSchema
                        {
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["property6"] = new AsyncApiSchema
                                {
                                    Type = "boolean"
                                }
                            }
                        },
                        ["property7"] = new AsyncApiSchema
                        {
                            Type = "string",
                            MinLength = 2
                        }
                    },
                },
            },
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocs
            {
                Url = new Uri("http://example.com/externalDocs")
            }
        };

        public static AsyncApiSchema AdvancedSchemaWithAllOf = new AsyncApiSchema
        {
            Title = "title1",
            AllOf = new List<AsyncApiSchema>
            {
                new AsyncApiSchema
                {
                    Title = "title2",
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property1"] = new AsyncApiSchema
                        {
                            Type = "integer"
                        },
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = "string",
                            MaxLength = 15
                        }
                    },
                },
                new AsyncApiSchema
                {
                    Title = "title3",
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property3"] = new AsyncApiSchema
                        {
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["property4"] = new AsyncApiSchema
                                {
                                    Type = "boolean"
                                }
                            }
                        },
                        ["property5"] = new AsyncApiSchema
                        {
                            Type = "string",
                            MinLength = 2
                        }
                    },
                    Nullable = true
                },
            },
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocs
            {
                Url = new Uri("http://example.com/externalDocs")
            }
        };

        public static AsyncApiSchema ReferencedSchema = new AsyncApiSchema
        {
            Title = "title1",
            MultipleOf = 3,
            Maximum = 42,
            ExclusiveMinimum = true,
            Minimum = 10,
            Default = new AsyncApiInteger(15),
            Type = "integer",

            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocs
            {
                Url = new Uri("http://example.com/externalDocs")
            },

            Reference = new AsyncApiReference
            {
                Type = ReferenceType.Schema,
                Id = "schemaObject1"
            }
        };

        public static AsyncApiSchema AdvancedSchemaWithRequiredPropertiesObject = new AsyncApiSchema
        {
            Title = "title1",
            Required = new HashSet<string>() { "property1" },
            Properties = new Dictionary<string, AsyncApiSchema>
            {
                ["property1"] = new AsyncApiSchema
                {
                    Required = new HashSet<string>() { "property3" },
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = "integer"
                        },
                        ["property3"] = new AsyncApiSchema
                        {
                            Type = "string",
                            MaxLength = 15,
                            ReadOnly = true
                        }
                    },
                    ReadOnly = true,
                },
                ["property4"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property5"] = new AsyncApiSchema
                        {
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["property6"] = new AsyncApiSchema
                                {
                                    Type = "boolean"
                                }
                            }
                        },
                        ["property7"] = new AsyncApiSchema
                        {
                            Type = "string",
                            MinLength = 2
                        }
                    },
                    ReadOnly = true,
                },
            },
            Nullable = true,
            ExternalDocs = new AsyncApiExternalDocs
            {
                Url = new Uri("http://example.com/externalDocs")
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiSchemaTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeBasicSchemaAsV2JsonWorks()
        {
            // Arrange
            var expected = @"{ }";

            // Act
            var actual = BasicSchema.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedSchemaNumberAsV2JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""multipleOf"": 3,
  ""maximum"": 42,
  ""minimum"": 10,
  ""exclusiveMinimum"": true,
  ""type"": ""integer"",
  ""default"": 15,
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaNumber.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedSchemaObjectAsV2JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""properties"": {
    ""property1"": {
      ""properties"": {
        ""property2"": {
          ""type"": ""integer""
        },
        ""property3"": {
          ""maxLength"": 15,
          ""type"": ""string""
        }
      }
    },
    ""property4"": {
      ""properties"": {
        ""property5"": {
          ""properties"": {
            ""property6"": {
              ""type"": ""boolean""
            }
          }
        },
        ""property7"": {
          ""minLength"": 2,
          ""type"": ""string""
        }
      }
    }
  },
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaObject.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedSchemaWithAllOfAsV2JsonWorks()
        {
            // Arrange
            var expected = @"{
  ""title"": ""title1"",
  ""allOf"": [
    {
      ""title"": ""title2"",
      ""properties"": {
        ""property1"": {
          ""type"": ""integer""
        },
        ""property2"": {
          ""maxLength"": 15,
          ""type"": ""string""
        }
      }
    },
    {
      ""title"": ""title3"",
      ""properties"": {
        ""property3"": {
          ""properties"": {
            ""property4"": {
              ""type"": ""boolean""
            }
          }
        },
        ""property5"": {
          ""minLength"": 2,
          ""type"": ""string""
        }
      },
      ""nullable"": true
    }
  ],
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            var actual = AdvancedSchemaWithAllOf.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedSchemaAsV2WithoutReferenceJsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);

            var expected = @"{
  ""title"": ""title1"",
  ""multipleOf"": 3,
  ""maximum"": 42,
  ""minimum"": 10,
  ""exclusiveMinimum"": true,
  ""type"": ""integer"",
  ""default"": 15,
  ""nullable"": true,
  ""externalDocs"": {
    ""url"": ""http://example.com/externalDocs""
  }
}";

            // Act
            ReferencedSchema.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedSchemaAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);

            var expected = @"{
  ""$ref"": ""#/components/schemas/schemaObject1""
}";

            // Act
            ReferencedSchema.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
