// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiSchemaTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiSchema/";

        [Fact]
        public void ParsePrimitiveSchemaShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "primitiveSchema.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var schema = AsyncApiV2Deserializer.LoadSchema(node);

                // Assert
                diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

                schema.Should().BeEquivalentTo(
                    new AsyncApiSchema
                    {
                        Type = "string",
                        Format = "email"
                    });
            }
        }

        [Fact]
        public void ParsePrimitiveSchemaFragmentShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "primitiveSchema.yaml")))
            {
                var reader = new AsyncApiStreamReader();
                var diagnostic = new AsyncApiDiagnostic();

                // Act
                var schema = reader.ReadFragment<AsyncApiSchema>(stream, AsyncApiSpecVersion.AsyncApi2_0, out diagnostic);

                // Assert
                diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

                schema.Should().BeEquivalentTo(
                    new AsyncApiSchema
                    {
                        Type = "string",
                        Format = "email"
                    });
            }
        }

        [Fact]
        public void ParsePrimitiveStringSchemaFragmentShouldSucceed()
        {
            var input = @"
{ ""type"": ""integer"",
""format"": ""int64"",
""default"": 88
}
";
            var reader = new AsyncApiStringReader();
            var diagnostic = new AsyncApiDiagnostic();

            // Act
            var schema = reader.ReadFragment<AsyncApiSchema>(input, AsyncApiSpecVersion.AsyncApi2_0, out diagnostic);

            // Assert
            diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

            schema.Should().BeEquivalentTo(
                new AsyncApiSchema
                {
                    Type = "integer",
                    Format = "int64",
                    Default = new AsyncApiLong(88)
                });
        }

        [Fact]
        public void ParseExampleStringFragmentShouldSucceed()
        {
            var input = @"
{ 
  ""foo"": ""bar"",
  ""baz"": [ 1,2]
}";
            var reader = new AsyncApiStringReader();
            var diagnostic = new AsyncApiDiagnostic();

            // Act
            var AsyncApiAny = reader.ReadFragment<IAsyncApiAny>(input, AsyncApiSpecVersion.AsyncApi2_0, out diagnostic);

            // Assert
            diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

            AsyncApiAny.Should().BeEquivalentTo(
                new AsyncApiObject
                {
                    ["foo"] = new AsyncApiString("bar"),
                    ["baz"] = new AsyncApiArray() {
                        new AsyncApiInteger(1),
                        new AsyncApiInteger(2)
                    }
                });
        }

        [Fact]
        public void ParseEnumFragmentShouldSucceed()
        {
            var input = @"
[ 
  ""foo"",
  ""baz""
]";
            var reader = new AsyncApiStringReader();
            var diagnostic = new AsyncApiDiagnostic();

            // Act
            var AsyncApiAny = reader.ReadFragment<IAsyncApiAny>(input, AsyncApiSpecVersion.AsyncApi2_0, out diagnostic);

            // Assert
            diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

            AsyncApiAny.Should().BeEquivalentTo(
                new AsyncApiArray
                {
                    new AsyncApiString("foo"),
                    new AsyncApiString("baz")
                });
        }

        [Fact]
        public void ParseSimpleSchemaShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "simpleSchema.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var schema = AsyncApiV2Deserializer.LoadSchema(node);

                // Assert
                diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

                schema.Should().BeEquivalentTo(
                    new AsyncApiSchema
                    {
                        Type = "object",
                        Required =
                        {
                            "name"
                        },
                        Properties =
                        {
                            ["name"] = new AsyncApiSchema
                            {
                                Type = "string"
                            },
                            ["address"] = new AsyncApiSchema
                            {
                                Type = "string"
                            },
                            ["age"] = new AsyncApiSchema
                            {
                                Type = "integer",
                                Format = "int32",
                                Minimum = 0
                            }
                        },
                        AdditionalPropertiesAllowed = false
                    });
            }
        }

        [Fact]
        public void ParsePathFragmentShouldSucceed()
        {
            var input = @"
summary: externally referenced path item
get:
  responses:
    '200':
      description: Ok
";
            var reader = new AsyncApiStringReader();
            var diagnostic = new AsyncApiDiagnostic();

            // Act
            var AsyncApiAny = reader.ReadFragment<AsyncApiPathItem>(input, AsyncApiSpecVersion.AsyncApi2_0, out diagnostic);

            // Assert
            diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

            AsyncApiAny.Should().BeEquivalentTo(
                new AsyncApiPathItem
                {
                    Summary = "externally referenced path item",
                    Operations = new Dictionary<OperationType, AsyncApiOperation>
                    {
                        [OperationType.Get] = new AsyncApiOperation()
                        {
                            Responses = new AsyncApiResponses
                            {
                                ["200"] = new AsyncApiResponse
                                {
                                    Description = "Ok"
                                }
                            }
                        }
                    }
                });
        }

        [Fact]
        public void ParseDictionarySchemaShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "dictionarySchema.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var schema = AsyncApiV2Deserializer.LoadSchema(node);

                // Assert
                diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

                schema.Should().BeEquivalentTo(
                    new AsyncApiSchema
                    {
                        Type = "object",
                        AdditionalProperties = new AsyncApiSchema
                        {
                            Type = "string"
                        }
                    });
            }
        }

        [Fact]
        public void ParseBasicSchemaWithExampleShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicSchemaWithExample.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var schema = AsyncApiV2Deserializer.LoadSchema(node);

                // Assert
                diagnostic.Should().BeEquivalentTo(new AsyncApiDiagnostic());

                schema.Should().BeEquivalentTo(
                    new AsyncApiSchema
                    {
                        Type = "object",
                        Properties =
                        {
                            ["id"] = new AsyncApiSchema
                            {
                                Type = "integer",
                                Format = "int64"
                            },
                            ["name"] = new AsyncApiSchema
                            {
                                Type = "string"
                            }
                        },
                        Required =
                        {
                            "name"
                        },
                        Example = new AsyncApiObject
                        {
                            ["name"] = new AsyncApiString("Puma"),
                            ["id"] = new AsyncApiLong(1)
                        }
                    });
            }
        }

        [Fact]
        public void ParseBasicSchemaWithReferenceShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicSchemaWithReference.yaml")))
            {
                // Act
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                // Assert
                var components = AsyncApiDoc.Components;

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });

                components.Should().BeEquivalentTo(
                    new AsyncApiComponents
                    {
                        Schemas =
                        {
                            ["ErrorModel"] = new AsyncApiSchema
                            {
                                Type = "object",
                                Properties =
                                {
                                    ["code"] = new AsyncApiSchema
                                    {
                                        Type = "integer",
                                        Minimum = 100,
                                        Maximum = 600
                                    },
                                    ["message"] = new AsyncApiSchema
                                    {
                                        Type = "string"
                                    }
                                },
                                Reference = new AsyncApiReference
                                {
                                    Type = ReferenceType.Schema,
                                    Id = "ErrorModel"
                                },
                                Required =
                                {
                                    "message",
                                    "code"
                                }
                            },
                            ["ExtendedErrorModel"] = new AsyncApiSchema
                            {
                                Reference = new AsyncApiReference
                                {
                                    Type = ReferenceType.Schema,
                                    Id = "ExtendedErrorModel"
                                },
                                AllOf =
                                {
                                    new AsyncApiSchema
                                    {
                                        Reference = new AsyncApiReference
                                        {
                                            Type = ReferenceType.Schema,
                                            Id = "ErrorModel"
                                        },
                                        // Schema should be dereferenced in our model, so all the properties
                                        // from the ErrorModel above should be propagated here.
                                        Type = "object",
                                        Properties =
                                        {
                                            ["code"] = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Minimum = 100,
                                                Maximum = 600
                                            },
                                            ["message"] = new AsyncApiSchema
                                            {
                                                Type = "string"
                                            }
                                        },
                                        Required =
                                        {
                                            "message",
                                            "code"
                                        }
                                    },
                                    new AsyncApiSchema
                                    {
                                        Type = "object",
                                        Required = {"rootCause"},
                                        Properties =
                                        {
                                            ["rootCause"] = new AsyncApiSchema
                                            {
                                                Type = "string"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
            }
        }

        [Fact]
        public void ParseAdvancedSchemaWithReferenceShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "advancedSchemaWithReference.yaml")))
            {
                // Act
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                // Assert
                var components = AsyncApiDoc.Components;

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });

                components.Should().BeEquivalentTo(
                    new AsyncApiComponents
                    {
                        Schemas =
                        {
                            ["Pet"] = new AsyncApiSchema
                            {
                                Type = "object",
                                Discriminator = new AsyncApiDiscriminator
                                {
                                    PropertyName = "petType"
                                },
                                Properties =
                                {
                                    ["name"] = new AsyncApiSchema
                                    {
                                        Type = "string"
                                    },
                                    ["petType"] = new AsyncApiSchema
                                    {
                                        Type = "string"
                                    }
                                },
                                Required =
                                {
                                    "name",
                                    "petType"
                                },
                                Reference = new AsyncApiReference()
                                {
                                    Id= "Pet",
                                    Type = ReferenceType.Schema
                                }
                            },
                            ["Cat"] = new AsyncApiSchema
                            {
                                Description = "A representation of a cat",
                                AllOf =
                                {
                                    new AsyncApiSchema
                                    {
                                        Reference = new AsyncApiReference
                                        {
                                            Type = ReferenceType.Schema,
                                            Id = "Pet"
                                        },
                                        // Schema should be dereferenced in our model, so all the properties
                                        // from the Pet above should be propagated here.
                                        Type = "object",
                                        Discriminator = new AsyncApiDiscriminator
                                        {
                                            PropertyName = "petType"
                                        },
                                        Properties =
                                        {
                                            ["name"] = new AsyncApiSchema
                                            {
                                                Type = "string"
                                            },
                                            ["petType"] = new AsyncApiSchema
                                            {
                                                Type = "string"
                                            }
                                        },
                                        Required =
                                        {
                                            "name",
                                            "petType"
                                        }
                                    },
                                    new AsyncApiSchema
                                    {
                                        Type = "object",
                                        Required = {"huntingSkill"},
                                        Properties =
                                        {
                                            ["huntingSkill"] = new AsyncApiSchema
                                            {
                                                Type = "string",
                                                Description = "The measured skill for hunting",
                                                Enum =
                                                {
                                                    new AsyncApiString("clueless"),
                                                    new AsyncApiString("lazy"),
                                                    new AsyncApiString("adventurous"),
                                                    new AsyncApiString("aggressive")
                                                }
                                            }
                                        }
                                    }
                                },
                                Reference = new AsyncApiReference()
                                {
                                    Id= "Cat",
                                    Type = ReferenceType.Schema
                                }
                            },
                            ["Dog"] = new AsyncApiSchema
                            {
                                Description = "A representation of a dog",
                                AllOf =
                                {
                                    new AsyncApiSchema
                                    {
                                        Reference = new AsyncApiReference
                                        {
                                            Type = ReferenceType.Schema,
                                            Id = "Pet"
                                        },
                                        // Schema should be dereferenced in our model, so all the properties
                                        // from the Pet above should be propagated here.
                                        Type = "object",
                                        Discriminator = new AsyncApiDiscriminator
                                        {
                                            PropertyName = "petType"
                                        },
                                        Properties =
                                        {
                                            ["name"] = new AsyncApiSchema
                                            {
                                                Type = "string"
                                            },
                                            ["petType"] = new AsyncApiSchema
                                            {
                                                Type = "string"
                                            }
                                        },
                                        Required =
                                        {
                                            "name",
                                            "petType"
                                        }
                                    },
                                    new AsyncApiSchema
                                    {
                                        Type = "object",
                                        Required = {"packSize"},
                                        Properties =
                                        {
                                            ["packSize"] = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int32",
                                                Description = "the size of the pack the dog is from",
                                                Default = new AsyncApiInteger(0),
                                                Minimum = 0
                                            }
                                        }
                                    }
                                },
                                Reference = new AsyncApiReference()
                                {
                                    Id= "Dog",
                                    Type = ReferenceType.Schema
                                }
                            }
                        }
                    });
            }
        }


        [Fact]
        public void ParseSelfReferencingSchemaShouldNotStackOverflow()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "selfReferencingSchema.yaml")))
            {
                // Act
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                // Assert
                var components = AsyncApiDoc.Components;

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });

                var schemaExtension = new AsyncApiSchema()
                {
                    AllOf = { new AsyncApiSchema()
                    {
                        Title = "schemaExtension",
                        Type = "object",
                        Properties = {
                                        ["description"] = new AsyncApiSchema() { Type = "string", Nullable = true},
                                        ["targetTypes"] = new AsyncApiSchema() {
                                            Type = "array",
                                            Items = new AsyncApiSchema() {
                                                Type = "string"
                                            }
                                        },
                                        ["status"] = new AsyncApiSchema() { Type = "string"},
                                        ["owner"] = new AsyncApiSchema() { Type = "string"},
                                        ["child"] = null
                                    }
                        }
                    },
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "microsoft.graph.schemaExtension"
                    }
                };

                schemaExtension.AllOf[0].Properties["child"] = schemaExtension;

                components.Schemas["microsoft.graph.schemaExtension"].Should().BeEquivalentTo(components.Schemas["microsoft.graph.schemaExtension"].AllOf[0].Properties["child"]);
            }
        }
    }
}
