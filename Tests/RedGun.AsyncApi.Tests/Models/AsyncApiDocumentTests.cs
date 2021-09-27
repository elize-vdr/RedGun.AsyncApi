// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiDocumentTests
    {
        public static AsyncApiComponents TopLevelReferencingComponents = new AsyncApiComponents()
        {
            Schemas =
            {
                ["schema1"] = new AsyncApiSchema
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "schema2"
                    }
                },
                ["schema2"] = new AsyncApiSchema
                {
                    Type = "object",
                    Properties =
                    {
                        ["property1"] = new AsyncApiSchema()
                        {
                            Type = "string"
                        }
                    }
                },
            }
        };

        public static AsyncApiComponents TopLevelSelfReferencingComponentsWithOtherProperties = new AsyncApiComponents()
        {
            Schemas =
            {
                ["schema1"] = new AsyncApiSchema
                {
                    Type = "object",
                    Properties =
                    {
                        ["property1"] = new AsyncApiSchema()
                        {
                            Type = "string"
                        }
                    },
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "schema1"
                    }
                },
                ["schema2"] = new AsyncApiSchema
                {
                    Type = "object",
                    Properties =
                    {
                        ["property1"] = new AsyncApiSchema()
                        {
                            Type = "string"
                        }
                    }
                },
            }
        };

        public static AsyncApiComponents TopLevelSelfReferencingComponents = new AsyncApiComponents()
        {
            Schemas =
            {
                ["schema1"] = new AsyncApiSchema
                {
                    Reference = new AsyncApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = "schema1"
                    }
                }
            }
        };

        public static AsyncApiDocument SimpleDocumentWithTopLevelReferencingComponents = new AsyncApiDocument()
        {
            Info = new AsyncApiInfo()
            {
                Version = "1.0.0"
            },
            Components = TopLevelReferencingComponents
        };

        public static AsyncApiDocument SimpleDocumentWithTopLevelSelfReferencingComponentsWithOtherProperties = new AsyncApiDocument()
        {
            Info = new AsyncApiInfo()
            {
                Version = "1.0.0"
            },
            Components = TopLevelSelfReferencingComponentsWithOtherProperties
        };

        public static AsyncApiDocument SimpleDocumentWithTopLevelSelfReferencingComponents = new AsyncApiDocument()
        {
            Info = new AsyncApiInfo()
            {
                Version = "1.0.0"
            },
            Components = TopLevelSelfReferencingComponents
        };

        public static AsyncApiComponents AdvancedComponentsWithReference = new AsyncApiComponents
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
                        Id = "pet",
                        Type = ReferenceType.Schema
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
                        Id = "newPet",
                        Type = ReferenceType.Schema
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
                        Id = "errorModel",
                        Type = ReferenceType.Schema
                    }
                },
            }
        };

        public static AsyncApiSchema PetSchemaWithReference = AdvancedComponentsWithReference.Schemas["pet"];

        public static AsyncApiSchema NewPetSchemaWithReference = AdvancedComponentsWithReference.Schemas["newPet"];

        public static AsyncApiSchema ErrorModelSchemaWithReference =
            AdvancedComponentsWithReference.Schemas["errorModel"];

        public static AsyncApiDocument AdvancedDocumentWithReference = new AsyncApiDocument
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
                                                Items = PetSchemaWithReference
                                            }
                                        },
                                        ["application/xml"] = new AsyncApiMediaType
                                        {
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "array",
                                                Items = PetSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                        Schema = NewPetSchemaWithReference
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
                                            Schema = PetSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                            Schema = PetSchemaWithReference
                                        },
                                        ["application/xml"] = new AsyncApiMediaType
                                        {
                                            Schema = PetSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
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
                                            Schema = ErrorModelSchemaWithReference
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            Components = AdvancedComponentsWithReference
        };

        public static AsyncApiComponents AdvancedComponents = new AsyncApiComponents
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
                    }
                },
            }
        };

        public static AsyncApiSchema PetSchema = AdvancedComponents.Schemas["pet"];

        public static AsyncApiSchema NewPetSchema = AdvancedComponents.Schemas["newPet"];

        public static AsyncApiSchema ErrorModelSchema = AdvancedComponents.Schemas["errorModel"];

        public static AsyncApiDocument AdvancedDocument = new AsyncApiDocument
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
                                                Items = PetSchema
                                            }
                                        },
                                        ["application/xml"] = new AsyncApiMediaType
                                        {
                                            Schema = new AsyncApiSchema
                                            {
                                                Type = "array",
                                                Items = PetSchema
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
                                            Schema = ErrorModelSchema
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
                                            Schema = ErrorModelSchema
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
                                        Schema = NewPetSchema
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
                                            Schema = PetSchema
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
                                            Schema = ErrorModelSchema
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
                                            Schema = ErrorModelSchema
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
                                            Schema = PetSchema
                                        },
                                        ["application/xml"] = new AsyncApiMediaType
                                        {
                                            Schema = PetSchema
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
                                            Schema = ErrorModelSchema
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
                                            Schema = ErrorModelSchema
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
                                            Schema = ErrorModelSchema
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
                                            Schema = ErrorModelSchema
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            Components = AdvancedComponents
        };

        public static AsyncApiDocument DuplicateExtensions = new AsyncApiDocument
        {
            Info = new AsyncApiInfo
            {
                Version = "1.0.0",
                Title = "Swagger Petstore (Simple)",
                Description = "A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification",
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
                ["/add/{operand1}/{operand2}"] = new AsyncApiPathItem
                {
                    Operations = new Dictionary<OperationType, AsyncApiOperation>
                    {
                        [OperationType.Get] = new AsyncApiOperation
                        {
                            OperationId = "addByOperand1AndByOperand2",
                            Parameters = new List<AsyncApiParameter>
                            {
                                new AsyncApiParameter
                                {
                                    Name = "operand1",
                                    In = ParameterLocation.Path,
                                    Description = "The first operand",
                                    Required = true,
                                    Schema = new AsyncApiSchema
                                    {
                                        Type = "integer",
                                        Extensions = new Dictionary<string, IAsyncApiExtension>
                                        {
                                            ["my-extension"] = new Any.AsyncApiInteger(4),
                                        }
                                    },
                                    Extensions = new Dictionary<string, IAsyncApiExtension>
                                    {
                                        ["my-extension"] = new Any.AsyncApiInteger(4),
                                    }
                                },
                                new AsyncApiParameter
                                {
                                    Name = "operand2",
                                    In = ParameterLocation.Path,
                                    Description = "The second operand",
                                    Required = true,
                                    Schema = new AsyncApiSchema
                                    {
                                        Type = "integer",
                                        Extensions = new Dictionary<string, IAsyncApiExtension>
                                        {
                                            ["my-extension"] = new Any.AsyncApiInteger(4),
                                        }
                                    },
                                    Extensions = new Dictionary<string, IAsyncApiExtension>
                                    {
                                        ["my-extension"] = new Any.AsyncApiInteger(4),
                                    }
                                },
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
                                                Items = PetSchema
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiDocumentTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeAdvancedDocumentAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""AsyncApi"": ""3.0.1"",
  ""info"": {
    ""title"": ""Swagger Petstore (Simple)"",
    ""description"": ""A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification"",
    ""termsOfService"": ""http://helloreverb.com/terms/"",
    ""contact"": {
      ""name"": ""Swagger API team"",
      ""url"": ""http://swagger.io"",
      ""email"": ""foo@example.com""
    },
    ""license"": {
      ""name"": ""MIT"",
      ""url"": ""http://opensource.org/licenses/MIT""
    },
    ""version"": ""1.0.0""
  },
  ""servers"": [
    {
      ""url"": ""http://petstore.swagger.io/api""
    }
  ],
  ""paths"": {
    ""/pets"": {
      ""get"": {
        ""description"": ""Returns all pets from the system that the user has access to"",
        ""operationId"": ""findPets"",
        ""parameters"": [
          {
            ""name"": ""tags"",
            ""in"": ""query"",
            ""description"": ""tags to filter by"",
            ""schema"": {
              ""type"": ""array"",
              ""items"": {
                ""type"": ""string""
              }
            }
          },
          {
            ""name"": ""limit"",
            ""in"": ""query"",
            ""description"": ""maximum number of results to return"",
            ""schema"": {
              ""type"": ""integer"",
              ""format"": ""int32""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""type"": ""array"",
                  ""items"": {
                    ""required"": [
                      ""id"",
                      ""name""
                    ],
                    ""type"": ""object"",
                    ""properties"": {
                      ""id"": {
                        ""type"": ""integer"",
                        ""format"": ""int64""
                      },
                      ""name"": {
                        ""type"": ""string""
                      },
                      ""tag"": {
                        ""type"": ""string""
                      }
                    }
                  }
                }
              },
              ""application/xml"": {
                ""schema"": {
                  ""type"": ""array"",
                  ""items"": {
                    ""required"": [
                      ""id"",
                      ""name""
                    ],
                    ""type"": ""object"",
                    ""properties"": {
                      ""id"": {
                        ""type"": ""integer"",
                        ""format"": ""int64""
                      },
                      ""name"": {
                        ""type"": ""string""
                      },
                      ""tag"": {
                        ""type"": ""string""
                      }
                    }
                  }
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          }
        }
      },
      ""post"": {
        ""description"": ""Creates a new pet in the store.  Duplicates are allowed"",
        ""operationId"": ""addPet"",
        ""requestBody"": {
          ""description"": ""Pet to add to the store"",
          ""content"": {
            ""application/json"": {
              ""schema"": {
                ""required"": [
                  ""name""
                ],
                ""type"": ""object"",
                ""properties"": {
                  ""id"": {
                    ""type"": ""integer"",
                    ""format"": ""int64""
                  },
                  ""name"": {
                    ""type"": ""string""
                  },
                  ""tag"": {
                    ""type"": ""string""
                  }
                }
              }
            }
          },
          ""required"": true
        },
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""required"": [
                    ""id"",
                    ""name""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""id"": {
                      ""type"": ""integer"",
                      ""format"": ""int64""
                    },
                    ""name"": {
                      ""type"": ""string""
                    },
                    ""tag"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          }
        }
      }
    },
    ""/pets/{id}"": {
      ""get"": {
        ""description"": ""Returns a user based on a single ID, if the user does not have access to the pet"",
        ""operationId"": ""findPetById"",
        ""parameters"": [
          {
            ""name"": ""id"",
            ""in"": ""path"",
            ""description"": ""ID of pet to fetch"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer"",
              ""format"": ""int64""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""required"": [
                    ""id"",
                    ""name""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""id"": {
                      ""type"": ""integer"",
                      ""format"": ""int64""
                    },
                    ""name"": {
                      ""type"": ""string""
                    },
                    ""tag"": {
                      ""type"": ""string""
                    }
                  }
                }
              },
              ""application/xml"": {
                ""schema"": {
                  ""required"": [
                    ""id"",
                    ""name""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""id"": {
                      ""type"": ""integer"",
                      ""format"": ""int64""
                    },
                    ""name"": {
                      ""type"": ""string""
                    },
                    ""tag"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          }
        }
      },
      ""delete"": {
        ""description"": ""deletes a single pet based on the ID supplied"",
        ""operationId"": ""deletePet"",
        ""parameters"": [
          {
            ""name"": ""id"",
            ""in"": ""path"",
            ""description"": ""ID of pet to delete"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer"",
              ""format"": ""int64""
            }
          }
        ],
        ""responses"": {
          ""204"": {
            ""description"": ""pet deleted""
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""required"": [
                    ""code"",
                    ""message""
                  ],
                  ""type"": ""object"",
                  ""properties"": {
                    ""code"": {
                      ""type"": ""integer"",
                      ""format"": ""int32""
                    },
                    ""message"": {
                      ""type"": ""string""
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  },
  ""components"": {
    ""schemas"": {
      ""pet"": {
        ""required"": [
          ""id"",
          ""name""
        ],
        ""type"": ""object"",
        ""properties"": {
          ""id"": {
            ""type"": ""integer"",
            ""format"": ""int64""
          },
          ""name"": {
            ""type"": ""string""
          },
          ""tag"": {
            ""type"": ""string""
          }
        }
      },
      ""newPet"": {
        ""required"": [
          ""name""
        ],
        ""type"": ""object"",
        ""properties"": {
          ""id"": {
            ""type"": ""integer"",
            ""format"": ""int64""
          },
          ""name"": {
            ""type"": ""string""
          },
          ""tag"": {
            ""type"": ""string""
          }
        }
      },
      ""errorModel"": {
        ""required"": [
          ""code"",
          ""message""
        ],
        ""type"": ""object"",
        ""properties"": {
          ""code"": {
            ""type"": ""integer"",
            ""format"": ""int32""
          },
          ""message"": {
            ""type"": ""string""
          }
        }
      }
    }
  }
}";

            // Act
            AdvancedDocument.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedDocumentWithReferenceAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""AsyncApi"": ""3.0.1"",
  ""info"": {
    ""title"": ""Swagger Petstore (Simple)"",
    ""description"": ""A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification"",
    ""termsOfService"": ""http://helloreverb.com/terms/"",
    ""contact"": {
      ""name"": ""Swagger API team"",
      ""url"": ""http://swagger.io"",
      ""email"": ""foo@example.com""
    },
    ""license"": {
      ""name"": ""MIT"",
      ""url"": ""http://opensource.org/licenses/MIT""
    },
    ""version"": ""1.0.0""
  },
  ""servers"": [
    {
      ""url"": ""http://petstore.swagger.io/api""
    }
  ],
  ""paths"": {
    ""/pets"": {
      ""get"": {
        ""description"": ""Returns all pets from the system that the user has access to"",
        ""operationId"": ""findPets"",
        ""parameters"": [
          {
            ""name"": ""tags"",
            ""in"": ""query"",
            ""description"": ""tags to filter by"",
            ""schema"": {
              ""type"": ""array"",
              ""items"": {
                ""type"": ""string""
              }
            }
          },
          {
            ""name"": ""limit"",
            ""in"": ""query"",
            ""description"": ""maximum number of results to return"",
            ""schema"": {
              ""type"": ""integer"",
              ""format"": ""int32""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""type"": ""array"",
                  ""items"": {
                    ""$ref"": ""#/components/schemas/pet""
                  }
                }
              },
              ""application/xml"": {
                ""schema"": {
                  ""type"": ""array"",
                  ""items"": {
                    ""$ref"": ""#/components/schemas/pet""
                  }
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          }
        }
      },
      ""post"": {
        ""description"": ""Creates a new pet in the store.  Duplicates are allowed"",
        ""operationId"": ""addPet"",
        ""requestBody"": {
          ""description"": ""Pet to add to the store"",
          ""content"": {
            ""application/json"": {
              ""schema"": {
                ""$ref"": ""#/components/schemas/newPet""
              }
            }
          },
          ""required"": true
        },
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/pet""
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          }
        }
      }
    },
    ""/pets/{id}"": {
      ""get"": {
        ""description"": ""Returns a user based on a single ID, if the user does not have access to the pet"",
        ""operationId"": ""findPetById"",
        ""parameters"": [
          {
            ""name"": ""id"",
            ""in"": ""path"",
            ""description"": ""ID of pet to fetch"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer"",
              ""format"": ""int64""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/pet""
                }
              },
              ""application/xml"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/pet""
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          }
        }
      },
      ""delete"": {
        ""description"": ""deletes a single pet based on the ID supplied"",
        ""operationId"": ""deletePet"",
        ""parameters"": [
          {
            ""name"": ""id"",
            ""in"": ""path"",
            ""description"": ""ID of pet to delete"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer"",
              ""format"": ""int64""
            }
          }
        ],
        ""responses"": {
          ""204"": {
            ""description"": ""pet deleted""
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""content"": {
              ""text/html"": {
                ""schema"": {
                  ""$ref"": ""#/components/schemas/errorModel""
                }
              }
            }
          }
        }
      }
    }
  },
  ""components"": {
    ""schemas"": {
      ""pet"": {
        ""required"": [
          ""id"",
          ""name""
        ],
        ""type"": ""object"",
        ""properties"": {
          ""id"": {
            ""type"": ""integer"",
            ""format"": ""int64""
          },
          ""name"": {
            ""type"": ""string""
          },
          ""tag"": {
            ""type"": ""string""
          }
        }
      },
      ""newPet"": {
        ""required"": [
          ""name""
        ],
        ""type"": ""object"",
        ""properties"": {
          ""id"": {
            ""type"": ""integer"",
            ""format"": ""int64""
          },
          ""name"": {
            ""type"": ""string""
          },
          ""tag"": {
            ""type"": ""string""
          }
        }
      },
      ""errorModel"": {
        ""required"": [
          ""code"",
          ""message""
        ],
        ""type"": ""object"",
        ""properties"": {
          ""code"": {
            ""type"": ""integer"",
            ""format"": ""int32""
          },
          ""message"": {
            ""type"": ""string""
          }
        }
      }
    }
  }
}";

            // Act
            AdvancedDocumentWithReference.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedDocumentAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected = @"{
  ""swagger"": ""2.0"",
  ""info"": {
    ""title"": ""Swagger Petstore (Simple)"",
    ""description"": ""A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification"",
    ""termsOfService"": ""http://helloreverb.com/terms/"",
    ""contact"": {
      ""name"": ""Swagger API team"",
      ""url"": ""http://swagger.io"",
      ""email"": ""foo@example.com""
    },
    ""license"": {
      ""name"": ""MIT"",
      ""url"": ""http://opensource.org/licenses/MIT""
    },
    ""version"": ""1.0.0""
  },
  ""host"": ""petstore.swagger.io"",
  ""basePath"": ""/api"",
  ""schemes"": [
    ""http""
  ],
  ""paths"": {
    ""/pets"": {
      ""get"": {
        ""description"": ""Returns all pets from the system that the user has access to"",
        ""operationId"": ""findPets"",
        ""produces"": [
          ""application/json"",
          ""application/xml"",
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""query"",
            ""name"": ""tags"",
            ""description"": ""tags to filter by"",
            ""type"": ""array"",
            ""items"": {
              ""type"": ""string""
            }
          },
          {
            ""in"": ""query"",
            ""name"": ""limit"",
            ""description"": ""maximum number of results to return"",
            ""type"": ""integer"",
            ""format"": ""int32""
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""type"": ""array"",
              ""items"": {
                ""required"": [
                  ""id"",
                  ""name""
                ],
                ""type"": ""object"",
                ""properties"": {
                  ""id"": {
                    ""format"": ""int64"",
                    ""type"": ""integer""
                  },
                  ""name"": {
                    ""type"": ""string""
                  },
                  ""tag"": {
                    ""type"": ""string""
                  }
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          }
        }
      },
      ""post"": {
        ""description"": ""Creates a new pet in the store.  Duplicates are allowed"",
        ""operationId"": ""addPet"",
        ""consumes"": [
          ""application/json""
        ],
        ""produces"": [
          ""application/json"",
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""body"",
            ""name"": ""body"",
            ""description"": ""Pet to add to the store"",
            ""required"": true,
            ""schema"": {
              ""required"": [
                ""name""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""id"": {
                  ""format"": ""int64"",
                  ""type"": ""integer""
                },
                ""name"": {
                  ""type"": ""string""
                },
                ""tag"": {
                  ""type"": ""string""
                }
              }
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""required"": [
                ""id"",
                ""name""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""id"": {
                  ""format"": ""int64"",
                  ""type"": ""integer""
                },
                ""name"": {
                  ""type"": ""string""
                },
                ""tag"": {
                  ""type"": ""string""
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          }
        }
      }
    },
    ""/pets/{id}"": {
      ""get"": {
        ""description"": ""Returns a user based on a single ID, if the user does not have access to the pet"",
        ""operationId"": ""findPetById"",
        ""produces"": [
          ""application/json"",
          ""application/xml"",
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""path"",
            ""name"": ""id"",
            ""description"": ""ID of pet to fetch"",
            ""required"": true,
            ""type"": ""integer"",
            ""format"": ""int64""
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""required"": [
                ""id"",
                ""name""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""id"": {
                  ""format"": ""int64"",
                  ""type"": ""integer""
                },
                ""name"": {
                  ""type"": ""string""
                },
                ""tag"": {
                  ""type"": ""string""
                }
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          }
        }
      },
      ""delete"": {
        ""description"": ""deletes a single pet based on the ID supplied"",
        ""operationId"": ""deletePet"",
        ""produces"": [
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""path"",
            ""name"": ""id"",
            ""description"": ""ID of pet to delete"",
            ""required"": true,
            ""type"": ""integer"",
            ""format"": ""int64""
          }
        ],
        ""responses"": {
          ""204"": {
            ""description"": ""pet deleted""
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""required"": [
                ""code"",
                ""message""
              ],
              ""type"": ""object"",
              ""properties"": {
                ""code"": {
                  ""format"": ""int32"",
                  ""type"": ""integer""
                },
                ""message"": {
                  ""type"": ""string""
                }
              }
            }
          }
        }
      }
    }
  },
  ""definitions"": {
    ""pet"": {
      ""required"": [
        ""id"",
        ""name""
      ],
      ""type"": ""object"",
      ""properties"": {
        ""id"": {
          ""format"": ""int64"",
          ""type"": ""integer""
        },
        ""name"": {
          ""type"": ""string""
        },
        ""tag"": {
          ""type"": ""string""
        }
      }
    },
    ""newPet"": {
      ""required"": [
        ""name""
      ],
      ""type"": ""object"",
      ""properties"": {
        ""id"": {
          ""format"": ""int64"",
          ""type"": ""integer""
        },
        ""name"": {
          ""type"": ""string""
        },
        ""tag"": {
          ""type"": ""string""
        }
      }
    },
    ""errorModel"": {
      ""required"": [
        ""code"",
        ""message""
      ],
      ""type"": ""object"",
      ""properties"": {
        ""code"": {
          ""format"": ""int32"",
          ""type"": ""integer""
        },
        ""message"": {
          ""type"": ""string""
        }
      }
    }
  }
}";

            // Act
            AdvancedDocument.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeDuplicateExtensionsAsV3JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected = @"{
  ""AsyncApi"": ""3.0.1"",
  ""info"": {
    ""title"": ""Swagger Petstore (Simple)"",
    ""description"": ""A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification"",
    ""version"": ""1.0.0""
  },
  ""servers"": [
    {
      ""url"": ""http://petstore.swagger.io/api""
    }
  ],
  ""paths"": {
    ""/add/{operand1}/{operand2}"": {
      ""get"": {
        ""operationId"": ""addByOperand1AndByOperand2"",
        ""parameters"": [
          {
            ""name"": ""operand1"",
            ""in"": ""path"",
            ""description"": ""The first operand"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer"",
              ""my-extension"": 4
            },
            ""my-extension"": 4
          },
          {
            ""name"": ""operand2"",
            ""in"": ""path"",
            ""description"": ""The second operand"",
            ""required"": true,
            ""schema"": {
              ""type"": ""integer"",
              ""my-extension"": 4
            },
            ""my-extension"": 4
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""content"": {
              ""application/json"": {
                ""schema"": {
                  ""type"": ""array"",
                  ""items"": {
                    ""required"": [
                      ""id"",
                      ""name""
                    ],
                    ""type"": ""object"",
                    ""properties"": {
                      ""id"": {
                        ""type"": ""integer"",
                        ""format"": ""int64""
                      },
                      ""name"": {
                        ""type"": ""string""
                      },
                      ""tag"": {
                        ""type"": ""string""
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}";

            // Act
            DuplicateExtensions.SerializeAsV3(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeDuplicateExtensionsAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected = @"{
  ""swagger"": ""2.0"",
  ""info"": {
    ""title"": ""Swagger Petstore (Simple)"",
    ""description"": ""A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification"",
    ""version"": ""1.0.0""
  },
  ""host"": ""petstore.swagger.io"",
  ""basePath"": ""/api"",
  ""schemes"": [
    ""http""
  ],
  ""paths"": {
    ""/add/{operand1}/{operand2}"": {
      ""get"": {
        ""operationId"": ""addByOperand1AndByOperand2"",
        ""produces"": [
          ""application/json""
        ],
        ""parameters"": [
          {
            ""in"": ""path"",
            ""name"": ""operand1"",
            ""description"": ""The first operand"",
            ""required"": true,
            ""type"": ""integer"",
            ""my-extension"": 4
          },
          {
            ""in"": ""path"",
            ""name"": ""operand2"",
            ""description"": ""The second operand"",
            ""required"": true,
            ""type"": ""integer"",
            ""my-extension"": 4
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""type"": ""array"",
              ""items"": {
                ""required"": [
                  ""id"",
                  ""name""
                ],
                ""type"": ""object"",
                ""properties"": {
                  ""id"": {
                    ""format"": ""int64"",
                    ""type"": ""integer""
                  },
                  ""name"": {
                    ""type"": ""string""
                  },
                  ""tag"": {
                    ""type"": ""string""
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}";

            // Act
            DuplicateExtensions.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedDocumentWithReferenceAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""swagger"": ""2.0"",
  ""info"": {
    ""title"": ""Swagger Petstore (Simple)"",
    ""description"": ""A sample API that uses a petstore as an example to demonstrate features in the swagger-2.0 specification"",
    ""termsOfService"": ""http://helloreverb.com/terms/"",
    ""contact"": {
      ""name"": ""Swagger API team"",
      ""url"": ""http://swagger.io"",
      ""email"": ""foo@example.com""
    },
    ""license"": {
      ""name"": ""MIT"",
      ""url"": ""http://opensource.org/licenses/MIT""
    },
    ""version"": ""1.0.0""
  },
  ""host"": ""petstore.swagger.io"",
  ""basePath"": ""/api"",
  ""schemes"": [
    ""http""
  ],
  ""paths"": {
    ""/pets"": {
      ""get"": {
        ""description"": ""Returns all pets from the system that the user has access to"",
        ""operationId"": ""findPets"",
        ""produces"": [
          ""application/json"",
          ""application/xml"",
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""query"",
            ""name"": ""tags"",
            ""description"": ""tags to filter by"",
            ""type"": ""array"",
            ""items"": {
              ""type"": ""string""
            }
          },
          {
            ""in"": ""query"",
            ""name"": ""limit"",
            ""description"": ""maximum number of results to return"",
            ""type"": ""integer"",
            ""format"": ""int32""
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""type"": ""array"",
              ""items"": {
                ""$ref"": ""#/definitions/pet""
              }
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          }
        }
      },
      ""post"": {
        ""description"": ""Creates a new pet in the store.  Duplicates are allowed"",
        ""operationId"": ""addPet"",
        ""consumes"": [
          ""application/json""
        ],
        ""produces"": [
          ""application/json"",
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""body"",
            ""name"": ""body"",
            ""description"": ""Pet to add to the store"",
            ""required"": true,
            ""schema"": {
              ""$ref"": ""#/definitions/newPet""
            }
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""$ref"": ""#/definitions/pet""
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          }
        }
      }
    },
    ""/pets/{id}"": {
      ""get"": {
        ""description"": ""Returns a user based on a single ID, if the user does not have access to the pet"",
        ""operationId"": ""findPetById"",
        ""produces"": [
          ""application/json"",
          ""application/xml"",
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""path"",
            ""name"": ""id"",
            ""description"": ""ID of pet to fetch"",
            ""required"": true,
            ""type"": ""integer"",
            ""format"": ""int64""
          }
        ],
        ""responses"": {
          ""200"": {
            ""description"": ""pet response"",
            ""schema"": {
              ""$ref"": ""#/definitions/pet""
            }
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          }
        }
      },
      ""delete"": {
        ""description"": ""deletes a single pet based on the ID supplied"",
        ""operationId"": ""deletePet"",
        ""produces"": [
          ""text/html""
        ],
        ""parameters"": [
          {
            ""in"": ""path"",
            ""name"": ""id"",
            ""description"": ""ID of pet to delete"",
            ""required"": true,
            ""type"": ""integer"",
            ""format"": ""int64""
          }
        ],
        ""responses"": {
          ""204"": {
            ""description"": ""pet deleted""
          },
          ""4XX"": {
            ""description"": ""unexpected client error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          },
          ""5XX"": {
            ""description"": ""unexpected server error"",
            ""schema"": {
              ""$ref"": ""#/definitions/errorModel""
            }
          }
        }
      }
    }
  },
  ""definitions"": {
    ""pet"": {
      ""required"": [
        ""id"",
        ""name""
      ],
      ""type"": ""object"",
      ""properties"": {
        ""id"": {
          ""format"": ""int64"",
          ""type"": ""integer""
        },
        ""name"": {
          ""type"": ""string""
        },
        ""tag"": {
          ""type"": ""string""
        }
      }
    },
    ""newPet"": {
      ""required"": [
        ""name""
      ],
      ""type"": ""object"",
      ""properties"": {
        ""id"": {
          ""format"": ""int64"",
          ""type"": ""integer""
        },
        ""name"": {
          ""type"": ""string""
        },
        ""tag"": {
          ""type"": ""string""
        }
      }
    },
    ""errorModel"": {
      ""required"": [
        ""code"",
        ""message""
      ],
      ""type"": ""object"",
      ""properties"": {
        ""code"": {
          ""format"": ""int32"",
          ""type"": ""integer""
        },
        ""message"": {
          ""type"": ""string""
        }
      }
    }
  }
}";

            // Act
            AdvancedDocumentWithReference.SerializeAsV2(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeSimpleDocumentWithTopLevelReferencingComponentsAsYamlV2Works()
        {
            // Arrange
            var expected = @"swagger: '2.0'
info:
  version: 1.0.0
paths: { }
definitions:
  schema1:
    $ref: '#/definitions/schema2'
  schema2:
    type: object
    properties:
      property1:
        type: string";

            // Act
            var actual = SimpleDocumentWithTopLevelReferencingComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeSimpleDocumentWithTopLevelSelfReferencingComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"swagger: '2.0'
info:
  version: 1.0.0
paths: { }
definitions:
  schema1: { }";

            // Act
            var actual = SimpleDocumentWithTopLevelSelfReferencingComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeSimpleDocumentWithTopLevelSelfReferencingWithOtherPropertiesComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"swagger: '2.0'
info:
  version: 1.0.0
paths: { }
definitions:
  schema1:
    type: object
    properties:
      property1:
        type: string
  schema2:
    type: object
    properties:
      property1:
        type: string";

            // Act
            var actual = SimpleDocumentWithTopLevelSelfReferencingComponentsWithOtherProperties.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeDocumentWithReferenceButNoComponents()
        {
            // Arrange
            var document = new AsyncApiDocument()
            {
                Info = new AsyncApiInfo
                {
                    Title = "Test",
                    Version = "1.0.0"
                },
                Paths = new AsyncApiPaths
                {
                    ["/"] = new AsyncApiPathItem
                    {
                        Operations = new Dictionary<OperationType, AsyncApiOperation>
                        {
                            [OperationType.Get] = new AsyncApiOperation
                            {
                                Responses = new AsyncApiResponses
                                {
                                    ["200"] = new AsyncApiResponse
                                    {
                                        Content = new Dictionary<string, AsyncApiMediaType>()
                                        {
                                            ["application/json"] = new AsyncApiMediaType
                                            {
                                                Schema = new AsyncApiSchema
                                                {
                                                    Reference = new AsyncApiReference
                                                    {
                                                        Id = "test",
                                                        Type = ReferenceType.Schema
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };


            var reference = document.Paths["/"].Operations[OperationType.Get].Responses["200"].Content["application/json"].Schema.Reference;

            // Act
            var actual = document.Serialize(AsyncApiSpecVersion.AsyncApi2_0, AsyncApiFormat.Json);

            // Assert
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void SerializeRelativePathAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"swagger: '2.0'
info:
  version: 1.0.0
basePath: /server1
paths: { }";
            var doc = new AsyncApiDocument()
            {
                Info = new AsyncApiInfo() { Version = "1.0.0" },
                Servers = new List<AsyncApiServer>() {
                    new AsyncApiServer()
                    {
                        Url = "/server1"
                    }
                }
            };

            // Act
            var actual = doc.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeRelativePathWithHostAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"swagger: '2.0'
info:
  version: 1.0.0
host: //example.org
basePath: /server1
paths: { }";
            var doc = new AsyncApiDocument()
            {
                Info = new AsyncApiInfo() { Version = "1.0.0" },
                Servers = new List<AsyncApiServer>() {
                    new AsyncApiServer()
                    {
                        Url = "//example.org/server1"
                    }
                }
            };

            // Act
            var actual = doc.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeRelativeRootPathWithHostAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"swagger: '2.0'
info:
  version: 1.0.0
host: //example.org
paths: { }";
            var doc = new AsyncApiDocument()
            {
                Info = new AsyncApiInfo() { Version = "1.0.0" },
                Servers = new List<AsyncApiServer>() {
                    new AsyncApiServer()
                    {
                        Url = "//example.org/"
                    }
                }
            };

            // Act
            var actual = doc.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

    }
}
