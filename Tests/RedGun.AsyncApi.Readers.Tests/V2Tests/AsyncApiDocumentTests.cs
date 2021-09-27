// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Newtonsoft.Json;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Validations;
using RedGun.AsyncApi.Validations.Rules;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiDocumentTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiDocument/";

        private readonly ITestOutputHelper _output;

        public AsyncApiDocumentTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ParseDocumentFromInlineStringShouldSucceed()
        {
            var AsyncApiDoc = new AsyncApiStringReader().Read(
                @"
AsyncApi : 3.0.0
info:
    title: Simple Document
    version: 0.9.1
paths: {}",
                out var context);

            AsyncApiDoc.Should().BeEquivalentTo(
                new AsyncApiDocument
                {
                    Info = new AsyncApiInfo
                    {
                        Title = "Simple Document",
                        Version = "0.9.1"
                    },
                    Paths = new AsyncApiPaths()
                });

            context.Should().BeEquivalentTo(
                new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("hi-IN")]
        // The equivalent of English 1,000.36 in French and Danish is 1.000,36
        [InlineData("fr-FR")]
        [InlineData("da-DK")]
        public void ParseDocumentWithDifferentCultureShouldSucceed(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            var AsyncApiDoc = new AsyncApiStringReader().Read(
                @"
AsyncApi : 3.0.0
info:
    title: Simple Document
    version: 0.9.1
components:
  schemas:
    sampleSchema:
      type: object
      properties:
        sampleProperty:
          type: double
          minimum: 100.54
          maximum: 60000000.35
          exclusiveMaximum: true
          exclusiveMinimum: false
paths: {}",
                out var context);

            AsyncApiDoc.Should().BeEquivalentTo(
                new AsyncApiDocument
                {
                    Info = new AsyncApiInfo
                    {
                        Title = "Simple Document",
                        Version = "0.9.1"
                    },
                    Components = new AsyncApiComponents()
                    {
                        Schemas =
                        {
                            ["sampleSchema"] = new AsyncApiSchema()
                            {
                                Type = "object",
                                Properties =
                                {
                                    ["sampleProperty"] = new AsyncApiSchema()
                                    {
                                        Type = "double",
                                        Minimum = (decimal)100.54,
                                        Maximum = (decimal)60000000.35,
                                        ExclusiveMaximum = true,
                                        ExclusiveMinimum = false
                                    }
                                },
                                Reference = new AsyncApiReference()
                                {
                                    Id = "sampleSchema",
                                    Type = ReferenceType.Schema
                                }
                            }
                        }
                    },
                    Paths = new AsyncApiPaths()
                });

            context.Should().BeEquivalentTo(
                new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });
        }

        [Fact]
        public void ParseBasicDocumentWithMultipleServersShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicDocumentWithMultipleServers.yaml")))
            {
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });

                AsyncApiDoc.Should().BeEquivalentTo(
                    new AsyncApiDocument
                    {
                        Info = new AsyncApiInfo
                        {
                            Title = "The API",
                            Version = "0.9.1",
                        },
                        Servers =
                        {
                            new AsyncApiServer
                            {
                                Url = new Uri("http://www.example.org/api").ToString(),
                                Description = "The http endpoint"
                            },
                            new AsyncApiServer
                            {
                                Url = new Uri("https://www.example.org/api").ToString(),
                                Description = "The https endpoint"
                            }
                        },
                        Paths = new AsyncApiPaths()
                    });
            }
        }

        [Fact]
        public void ParseBrokenMinimalDocumentShouldYieldExpectedDiagnostic()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "brokenMinimalDocument.yaml")))
            {
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                AsyncApiDoc.Should().BeEquivalentTo(
                    new AsyncApiDocument
                    {
                        Info = new AsyncApiInfo
                        {
                            Version = "0.9"
                        },
                        Paths = new AsyncApiPaths()
                    });

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic
                    {
                        Errors =
                        {
                            new AsyncApiValidatorError(nameof(AsyncApiInfoRules.InfoRequiredFields),"#/info/title", "The field 'title' in 'info' object is REQUIRED.")
                        },
                        SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0
                    });
            }
        }

        [Fact]
        public void ParseMinimalDocumentShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "minimalDocument.yaml")))
            {
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                AsyncApiDoc.Should().BeEquivalentTo(
                    new AsyncApiDocument
                    {
                        Info = new AsyncApiInfo
                        {
                            Title = "Simple Document",
                            Version = "0.9.1"
                        },
                        Paths = new AsyncApiPaths()
                    });

                diagnostic.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });
            }
        }

        [Fact]
        public void ParseStandardPetStoreDocumentShouldSucceed()
        {
            AsyncApiDiagnostic context;
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "petStore.yaml")))
            {
                var actual = new AsyncApiStreamReader().Read(stream, out context);

                var components = new AsyncApiComponents
                {
                    Schemas = new Dictionary<string, AsyncApiSchema>
                    {
                        ["pet"] = new AsyncApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string>
                            {
                                "id",
                                "name"
                            },
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["id"] = new AsyncApiSchema
                                {
                                    Type = "integer",
                                    Format = "int64"
                                },
                                ["name"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                                ["tag"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                            },
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "pet"
                            }
                        },
                        ["newPet"] = new AsyncApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string>
                            {
                                "name"
                            },
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["id"] = new AsyncApiSchema
                                {
                                    Type = "integer",
                                    Format = "int64"
                                },
                                ["name"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                                ["tag"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                            },
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "newPet"
                            }
                        },
                        ["errorModel"] = new AsyncApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string>
                            {
                                "code",
                                "message"
                            },
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["code"] = new AsyncApiSchema
                                {
                                    Type = "integer",
                                    Format = "int32"
                                },
                                ["message"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                }
                            },
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "errorModel"
                            }
                        },
                    }
                };

                // Create a clone of the schema to avoid modifying things in components.
                var petSchema =
                    JsonConvert.DeserializeObject<AsyncApiSchema>(
                        JsonConvert.SerializeObject(components.Schemas["pet"]));
                petSchema.Reference = new AsyncApiReference
                {
                    Id = "pet",
                    Type = ReferenceType.Schema
                };

                var newPetSchema =
                    JsonConvert.DeserializeObject<AsyncApiSchema>(
                        JsonConvert.SerializeObject(components.Schemas["newPet"]));
                newPetSchema.Reference = new AsyncApiReference
                {
                    Id = "newPet",
                    Type = ReferenceType.Schema
                };

                var errorModelSchema =
                    JsonConvert.DeserializeObject<AsyncApiSchema>(
                        JsonConvert.SerializeObject(components.Schemas["errorModel"]));
                errorModelSchema.Reference = new AsyncApiReference
                {
                    Id = "errorModel",
                    Type = ReferenceType.Schema
                };

                var expected = new AsyncApiDocument
                {
                    Info = new AsyncApiInfo
                    {
                        Version = "1.0.0",
                        Title = "Swagger Petstore (Simple)",
                        Description =
                            "A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification",
                        TermsOfService = new Uri("http://helloreverb.com/terms/"),
                        Contact = new AsyncApiContact
                        {
                            Name = "Swagger API team",
                            Email = "foo@example.com",
                            Url = new Uri("http://swagger.io")
                        },
                        License = new AsyncApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("http://opensource.org/licenses/MIT")
                        }
                    },
                    Servers = new List<AsyncApiServer>
                    {
                        new AsyncApiServer
                        {
                            Url = "http://petstore.swagger.io/api"
                        }
                    },
                    Paths = new AsyncApiPaths
                    {
                        ["/pets"] = new AsyncApiPathItem
                        {
                            Operations = new Dictionary<OperationType, AsyncApiOperation>
                            {
                                [OperationType.Get] = new AsyncApiOperation
                                {
                                    Description = "Returns all pets from the system that the user has access to",
                                    OperationId = "findPets",
                                    Parameters = new List<AsyncApiParameter>
                                    {
                                        new AsyncApiParameter
                                        {
                                            Name = "tags",
                                            In = ParameterLocation.Query,
                                            Description = "tags to filter by",
                                            Required = false,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "array",
                                                Items = new AsyncApiSchema
                                                {
                                                    Type = "string"
                                                }
                                            }
                                        },
                                        new AsyncApiParameter
                                        {
                                            Name = "limit",
                                            In = ParameterLocation.Query,
                                            Description = "maximum number of results to return",
                                            Required = false,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int32"
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["200"] = new AsyncApiResponse
                                        {
                                            Description = "pet response",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema
                                                    {
                                                        Type = "array",
                                                        Items = petSchema
                                                    }
                                                },
                                                ["application/xml"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema
                                                    {
                                                        Type = "array",
                                                        Items = petSchema
                                                    }
                                                }
                                            }
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                },
                                [OperationType.Post] = new AsyncApiOperation
                                {
                                    Description = "Creates a new pet in the store.  Duplicates are allowed",
                                    OperationId = "addPet",
                                    RequestBody = new AsyncApiRequestBody
                                    {
                                        Description = "Pet to add to the store",
                                        Required = true,
                                        Content = new Dictionary<string, AsyncApiMediaType>
                                        {
                                            ["application/json"] = new AsyncApiMediaType
                                            {
                                                Schema = newPetSchema
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["200"] = new AsyncApiResponse
                                        {
                                            Description = "pet response",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = petSchema
                                                },
                                            }
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        ["/pets/{id}"] = new AsyncApiPathItem
                        {
                            Operations = new Dictionary<OperationType, AsyncApiOperation>
                            {
                                [OperationType.Get] = new AsyncApiOperation
                                {
                                    Description =
                                        "Returns a user based on a single ID, if the user does not have access to the pet",
                                    OperationId = "findPetById",
                                    Parameters = new List<AsyncApiParameter>
                                    {
                                        new AsyncApiParameter
                                        {
                                            Name = "id",
                                            In = ParameterLocation.Path,
                                            Description = "ID of pet to fetch",
                                            Required = true,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int64"
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["200"] = new AsyncApiResponse
                                        {
                                            Description = "pet response",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = petSchema
                                                },
                                                ["application/xml"] = new AsyncApiMediaType
                                                {
                                                    Schema = petSchema
                                                }
                                            }
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                },
                                [OperationType.Delete] = new AsyncApiOperation
                                {
                                    Description = "deletes a single pet based on the ID supplied",
                                    OperationId = "deletePet",
                                    Parameters = new List<AsyncApiParameter>
                                    {
                                        new AsyncApiParameter
                                        {
                                            Name = "id",
                                            In = ParameterLocation.Path,
                                            Description = "ID of pet to delete",
                                            Required = true,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int64"
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["204"] = new AsyncApiResponse
                                        {
                                            Description = "pet deleted"
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Components = components
                };

                actual.Should().BeEquivalentTo(expected);
            }

            context.Should().BeEquivalentTo(
                new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });
        }

        [Fact]
        public void ParseModifiedPetStoreDocumentWithTagAndSecurityShouldSucceed()
        {
            AsyncApiDiagnostic context;
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "petStoreWithTagAndSecurity.yaml")))
            {
                var actual = new AsyncApiStreamReader().Read(stream, out context);

                var components = new AsyncApiComponents
                {
                    Schemas = new Dictionary<string, AsyncApiSchema>
                    {
                        ["pet"] = new AsyncApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string>
                            {
                                "id",
                                "name"
                            },
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["id"] = new AsyncApiSchema
                                {
                                    Type = "integer",
                                    Format = "int64"
                                },
                                ["name"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                                ["tag"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                            },
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "pet"
                            }
                        },
                        ["newPet"] = new AsyncApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string>
                            {
                                "name"
                            },
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["id"] = new AsyncApiSchema
                                {
                                    Type = "integer",
                                    Format = "int64"
                                },
                                ["name"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                                ["tag"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                },
                            },
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "newPet"
                            }
                        },
                        ["errorModel"] = new AsyncApiSchema
                        {
                            Type = "object",
                            Required = new HashSet<string>
                            {
                                "code",
                                "message"
                            },
                            Properties = new Dictionary<string, AsyncApiSchema>
                            {
                                ["code"] = new AsyncApiSchema
                                {
                                    Type = "integer",
                                    Format = "int32"
                                },
                                ["message"] = new AsyncApiSchema
                                {
                                    Type = "string"
                                }
                            },
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "errorModel"
                            }
                        },
                    },
                    SecuritySchemes = new Dictionary<string, AsyncApiSecurityScheme>
                    {
                        ["securitySchemeName1"] = new AsyncApiSecurityScheme
                        {
                            Type = SecuritySchemeType.ApiKey,
                            Name = "apiKeyName1",
                            In = ParameterLocation.Header,
                            Reference = new AsyncApiReference
                            {
                                Id = "securitySchemeName1",
                                Type = ReferenceType.SecurityScheme
                            }

                        },
                        ["securitySchemeName2"] = new AsyncApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OpenIdConnect,
                            OpenIdConnectUrl = new Uri("http://example.com"),
                            Reference = new AsyncApiReference
                            {
                                Id = "securitySchemeName2",
                                Type = ReferenceType.SecurityScheme
                            }
                        }
                    }
                };

                // Create a clone of the schema to avoid modifying things in components.
                var petSchema =
                    JsonConvert.DeserializeObject<AsyncApiSchema>(
                        JsonConvert.SerializeObject(components.Schemas["pet"]));
                petSchema.Reference = new AsyncApiReference
                {
                    Id = "pet",
                    Type = ReferenceType.Schema
                };

                var newPetSchema =
                    JsonConvert.DeserializeObject<AsyncApiSchema>(
                        JsonConvert.SerializeObject(components.Schemas["newPet"]));
                newPetSchema.Reference = new AsyncApiReference
                {
                    Id = "newPet",
                    Type = ReferenceType.Schema
                };

                var errorModelSchema =
                    JsonConvert.DeserializeObject<AsyncApiSchema>(
                        JsonConvert.SerializeObject(components.Schemas["errorModel"]));
                errorModelSchema.Reference = new AsyncApiReference
                {
                    Id = "errorModel",
                    Type = ReferenceType.Schema
                };

                var tag1 = new AsyncApiTag
                {
                    Name = "tagName1",
                    Description = "tagDescription1",
                    Reference = new AsyncApiReference
                    {
                        Id = "tagName1",
                        Type = ReferenceType.Tag
                    }
                };


                var tag2 = new AsyncApiTag
                {
                    Name = "tagName2"
                };

                var securityScheme1 = JsonConvert.DeserializeObject<AsyncApiSecurityScheme>(
                    JsonConvert.SerializeObject(components.SecuritySchemes["securitySchemeName1"]));
                securityScheme1.Reference = new AsyncApiReference
                {
                    Id = "securitySchemeName1",
                    Type = ReferenceType.SecurityScheme
                };

                var securityScheme2 = JsonConvert.DeserializeObject<AsyncApiSecurityScheme>(
                    JsonConvert.SerializeObject(components.SecuritySchemes["securitySchemeName2"]));
                securityScheme2.Reference = new AsyncApiReference
                {
                    Id = "securitySchemeName2",
                    Type = ReferenceType.SecurityScheme
                };

                var expected = new AsyncApiDocument
                {
                    Info = new AsyncApiInfo
                    {
                        Version = "1.0.0",
                        Title = "Swagger Petstore (Simple)",
                        Description =
                            "A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification",
                        TermsOfService = new Uri("http://helloreverb.com/terms/"),
                        Contact = new AsyncApiContact
                        {
                            Name = "Swagger API team",
                            Email = "foo@example.com",
                            Url = new Uri("http://swagger.io")
                        },
                        License = new AsyncApiLicense
                        {
                            Name = "MIT",
                            Url = new Uri("http://opensource.org/licenses/MIT")
                        }
                    },
                    Servers = new List<AsyncApiServer>
                    {
                        new AsyncApiServer
                        {
                            Url = "http://petstore.swagger.io/api"
                        }
                    },
                    Paths = new AsyncApiPaths
                    {
                        ["/pets"] = new AsyncApiPathItem
                        {
                            Operations = new Dictionary<OperationType, AsyncApiOperation>
                            {
                                [OperationType.Get] = new AsyncApiOperation
                                {
                                    Tags = new List<AsyncApiTag>
                                    {
                                        tag1,
                                        tag2
                                    },
                                    Description = "Returns all pets from the system that the user has access to",
                                    OperationId = "findPets",
                                    Parameters = new List<AsyncApiParameter>
                                    {
                                        new AsyncApiParameter
                                        {
                                            Name = "tags",
                                            In = ParameterLocation.Query,
                                            Description = "tags to filter by",
                                            Required = false,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "array",
                                                Items = new AsyncApiSchema
                                                {
                                                    Type = "string"
                                                }
                                            }
                                        },
                                        new AsyncApiParameter
                                        {
                                            Name = "limit",
                                            In = ParameterLocation.Query,
                                            Description = "maximum number of results to return",
                                            Required = false,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int32"
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["200"] = new AsyncApiResponse
                                        {
                                            Description = "pet response",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema
                                                    {
                                                        Type = "array",
                                                        Items = petSchema
                                                    }
                                                },
                                                ["application/xml"] = new AsyncApiMediaType
                                                {
                                                    Schema = new AsyncApiSchema
                                                    {
                                                        Type = "array",
                                                        Items = petSchema
                                                    }
                                                }
                                            }
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                },
                                [OperationType.Post] = new AsyncApiOperation
                                {
                                    Tags = new List<AsyncApiTag>
                                    {
                                        tag1,
                                        tag2
                                    },
                                    Description = "Creates a new pet in the store.  Duplicates are allowed",
                                    OperationId = "addPet",
                                    RequestBody = new AsyncApiRequestBody
                                    {
                                        Description = "Pet to add to the store",
                                        Required = true,
                                        Content = new Dictionary<string, AsyncApiMediaType>
                                        {
                                            ["application/json"] = new AsyncApiMediaType
                                            {
                                                Schema = newPetSchema
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["200"] = new AsyncApiResponse
                                        {
                                            Description = "pet response",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = petSchema
                                                },
                                            }
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    },
                                    Security = new List<AsyncApiSecurityRequirement>
                                    {
                                        new AsyncApiSecurityRequirement
                                        {
                                            [securityScheme1] = new List<string>(),
                                            [securityScheme2] = new List<string>
                                            {
                                                "scope1",
                                                "scope2"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        ["/pets/{id}"] = new AsyncApiPathItem
                        {
                            Operations = new Dictionary<OperationType, AsyncApiOperation>
                            {
                                [OperationType.Get] = new AsyncApiOperation
                                {
                                    Description =
                                        "Returns a user based on a single ID, if the user does not have access to the pet",
                                    OperationId = "findPetById",
                                    Parameters = new List<AsyncApiParameter>
                                    {
                                        new AsyncApiParameter
                                        {
                                            Name = "id",
                                            In = ParameterLocation.Path,
                                            Description = "ID of pet to fetch",
                                            Required = true,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int64"
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["200"] = new AsyncApiResponse
                                        {
                                            Description = "pet response",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["application/json"] = new AsyncApiMediaType
                                                {
                                                    Schema = petSchema
                                                },
                                                ["application/xml"] = new AsyncApiMediaType
                                                {
                                                    Schema = petSchema
                                                }
                                            }
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                },
                                [OperationType.Delete] = new AsyncApiOperation
                                {
                                    Description = "deletes a single pet based on the ID supplied",
                                    OperationId = "deletePet",
                                    Parameters = new List<AsyncApiParameter>
                                    {
                                        new AsyncApiParameter
                                        {
                                            Name = "id",
                                            In = ParameterLocation.Path,
                                            Description = "ID of pet to delete",
                                            Required = true,
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "integer",
                                                Format = "int64"
                                            }
                                        }
                                    },
                                    Responses = new AsyncApiResponses
                                    {
                                        ["204"] = new AsyncApiResponse
                                        {
                                            Description = "pet deleted"
                                        },
                                        ["4XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected client error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        },
                                        ["5XX"] = new AsyncApiResponse
                                        {
                                            Description = "unexpected server error",
                                            Content = new Dictionary<string, AsyncApiMediaType>
                                            {
                                                ["text/html"] = new AsyncApiMediaType
                                                {
                                                    Schema = errorModelSchema
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Components = components,
                    Tags = new List<AsyncApiTag>
                    {
                        new AsyncApiTag
                        {
                            Name = "tagName1",
                            Description = "tagDescription1",
                            Reference = new AsyncApiReference()
                            {
                                Id = "tagName1",
                                Type = ReferenceType.Tag
                            }
                        }
                    },
                    SecurityRequirements = new List<AsyncApiSecurityRequirement>
                    {
                        new AsyncApiSecurityRequirement
                        {
                            [securityScheme1] = new List<string>(),
                            [securityScheme2] = new List<string>
                            {
                                "scope1",
                                "scope2",
                                "scope3"
                            }
                        }
                    }
                };

                actual.Should().BeEquivalentTo(expected);
            }

            context.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });
        }

        [Fact]
        public void ParsePetStoreExpandedShouldSucceed()
        {
            AsyncApiDiagnostic context;

            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "petStoreExpanded.yaml")))
            {
                var actual = new AsyncApiStreamReader().Read(stream, out context);

                // TODO: Create the object in memory and compare with the one read from YAML file.
            }

            context.Should().BeEquivalentTo(
                    new AsyncApiDiagnostic() { SpecificationVersion = AsyncApiSpecVersion.AsyncApi2_0 });
        }

        [Fact]
        public void GlobalSecurityRequirementShouldReferenceSecurityScheme()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "securedApi.yaml")))
            {
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                var securityRequirement = AsyncApiDoc.SecurityRequirements.First();

                Assert.Same(securityRequirement.Keys.First(), AsyncApiDoc.Components.SecuritySchemes.First().Value);
            }
        }

        [Fact]
        public void HeaderParameterShouldAllowExample()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "apiWithFullHeaderComponent.yaml")))
            {
                var AsyncApiDoc = new AsyncApiStreamReader().Read(stream, out var diagnostic);

                var exampleHeader = AsyncApiDoc.Components?.Headers?["example-header"];
                Assert.NotNull(exampleHeader);
                exampleHeader.Should().BeEquivalentTo(
                    new AsyncApiHeader()
                    {
                        Description = "Test header with example",
                        Required = true,
                        Deprecated = true,
                        AllowEmptyValue = true,
                        AllowReserved = true,
                        Style = ParameterStyle.Simple,
                        Explode = true,
                        Example = new AsyncApiString("99391c7e-ad88-49ec-a2ad-99ddcb1f7721"),
                        Schema = new AsyncApiSchema()
                        {
                            Type = "string",
                            Format = "uuid"
                        },
                        Reference = new AsyncApiReference()
                        {
                            Type = ReferenceType.Header,
                            Id = "example-header"
                        }
                    });

                var examplesHeader = AsyncApiDoc.Components?.Headers?["examples-header"];
                Assert.NotNull(examplesHeader);
                examplesHeader.Should().BeEquivalentTo(
                    new AsyncApiHeader()
                    {
                        Description = "Test header with example",
                        Required = true,
                        Deprecated = true,
                        AllowEmptyValue = true,
                        AllowReserved = true,
                        Style = ParameterStyle.Simple,
                        Explode = true,
                        Examples = new Dictionary<string, AsyncApiExample>()
                        {
                            { "uuid1", new AsyncApiExample()
                                {
                                    Value = new AsyncApiString("99391c7e-ad88-49ec-a2ad-99ddcb1f7721")
                                }
                            },
                            { "uuid2", new AsyncApiExample()
                                {
                                    Value = new AsyncApiString("99391c7e-ad88-49ec-a2ad-99ddcb1f7721")
                                }
                            }
                        },
                        Schema = new AsyncApiSchema()
                        {
                            Type = "string",
                            Format = "uuid"
                        },
                        Reference = new AsyncApiReference()
                        {
                            Type = ReferenceType.Header,
                            Id = "examples-header"
                        }
                    });
            }
        }
    }
}
