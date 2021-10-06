// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Validations;
using Xunit;

namespace RedGun.AsyncApi.Tests.Validations
{
    public class AsyncApiReferenceValidationTests
    {
        [Fact]
        public void ReferencedSchemaShouldOnlyBeValidatedOnce()
        {
            // Arrange

            var sharedSchema = new AsyncApiSchema
            {
                Type = "string",
                Reference = new AsyncApiReference()
                {
                    Id = "test"
                },
                UnresolvedReference = false
            };

            AsyncApiDocument document = new AsyncApiDocument();
            document.Components = new AsyncApiComponents()
            {
                Schemas = new Dictionary<string, AsyncApiSchema>()
                {
                    [sharedSchema.Reference.Id] = sharedSchema
                }
            };

            document.Paths = new AsyncApiPaths()
            {
                ["/"] = new AsyncApiPathItem()
                {
                    Operations = new Dictionary<OperationType, AsyncApiOperation>
                    {
                        [OperationType.Get] = new AsyncApiOperation()
                        {
                            Responses = new AsyncApiResponses()
                            {
                                ["200"] = new AsyncApiResponse()
                                {
                                    Content = new Dictionary<string, AsyncApiMediaType>()
                                    {
                                        ["application/json"] = new AsyncApiMediaType()
                                        {
                                            Schema = sharedSchema
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var errors = document.Validate(new ValidationRuleSet() { new AlwaysFailRule<AsyncApiSchema>() });


            // Assert
            Assert.True(errors.Count() == 1);
        }

        [Fact]
        public void UnresolvedReferenceSchemaShouldNotBeValidated()
        {
            // Arrange
            var sharedSchema = new AsyncApiSchema
            {
                Type = "string",
                Reference = new AsyncApiReference()
                {
                    Id = "test"
                },
                UnresolvedReference = true
            };

            AsyncApiDocument document = new AsyncApiDocument();
            document.Components = new AsyncApiComponents()
            {
                Schemas = new Dictionary<string, AsyncApiSchema>()
                {
                    [sharedSchema.Reference.Id] = sharedSchema
                }
            };

            // Act
            var errors = document.Validate(new ValidationRuleSet() { new AlwaysFailRule<AsyncApiSchema>() });

            // Assert
            Assert.True(errors.Count() == 0);
        }

        [Fact]
        public void UnresolvedSchemaReferencedShouldNotBeValidated()
        {
            // Arrange

            var sharedSchema = new AsyncApiSchema
            {
                Reference = new AsyncApiReference()
                {
                    Id = "test"
                },
                UnresolvedReference = true
            };

            AsyncApiDocument document = new AsyncApiDocument();

            document.Paths = new AsyncApiPaths()
            {
                ["/"] = new AsyncApiPathItem()
                {
                    Operations = new Dictionary<OperationType, AsyncApiOperation>
                    {
                        [OperationType.Get] = new AsyncApiOperation()
                        {
                            Responses = new AsyncApiResponses()
                            {
                                ["200"] = new AsyncApiResponse()
                                {
                                    Content = new Dictionary<string, AsyncApiMediaType>()
                                    {
                                        ["application/json"] = new AsyncApiMediaType()
                                        {
                                            Schema = sharedSchema
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var errors = document.Validate(new ValidationRuleSet() { new AlwaysFailRule<AsyncApiSchema>() });

            // Assert
            Assert.True(errors.Count() == 0);
        }
    }

    public class AlwaysFailRule<T> : ValidationRule<T> where T : IAsyncApiElement
    {
        public AlwaysFailRule() : base((c, t) => c.CreateError("x", "y"))
        {

        }
    }
}
