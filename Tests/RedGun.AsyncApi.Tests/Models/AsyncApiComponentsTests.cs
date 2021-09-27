// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiComponentsTests
    {
        public static AsyncApiComponents AdvancedComponents = new AsyncApiComponents
        {
            Schemas = new Dictionary<string, AsyncApiSchema>
            {
                ["schema1"] = new AsyncApiSchema
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
            },
            SecuritySchemes = new Dictionary<string, AsyncApiSecurityScheme>
            {
                ["securityScheme1"] = new AsyncApiSecurityScheme
                {
                    Description = "description1",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new AsyncApiOAuthFlows
                    {
                        Implicit = new AsyncApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                ["operation1:object1"] = "operation 1 on object 1",
                                ["operation2:object2"] = "operation 2 on object 2"
                            },
                            AuthorizationUrl = new Uri("https://example.com/api/oauth")
                        }
                    }
                },
                ["securityScheme2"] = new AsyncApiSecurityScheme
                {
                    Description = "description1",
                    Type = SecuritySchemeType.OpenIdConnect,
                    Scheme = "openIdConnectUrl",
                    OpenIdConnectUrl = new Uri("https://example.com/openIdConnect")
                }
            }
        };

        public static AsyncApiComponents AdvancedComponentsWithReference = new AsyncApiComponents
        {
            Schemas = new Dictionary<string, AsyncApiSchema>
            {
                ["schema1"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = "integer"
                        },
                        ["property3"] = new AsyncApiSchema
                        {
                            Reference = new AsyncApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = "schema2"
                            }
                        }
                    },
                    Reference = new AsyncApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = "schema1"
                    }
                },
                ["schema2"] = new AsyncApiSchema
                {
                    Properties = new Dictionary<string, AsyncApiSchema>
                    {
                        ["property2"] = new AsyncApiSchema
                        {
                            Type = "integer"
                        }
                    }
                },
            },
            SecuritySchemes = new Dictionary<string, AsyncApiSecurityScheme>
            {
                ["securityScheme1"] = new AsyncApiSecurityScheme
                {
                    Description = "description1",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new AsyncApiOAuthFlows
                    {
                        Implicit = new AsyncApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                ["operation1:object1"] = "operation 1 on object 1",
                                ["operation2:object2"] = "operation 2 on object 2"
                            },
                            AuthorizationUrl = new Uri("https://example.com/api/oauth")
                        }
                    },
                    Reference = new AsyncApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "securityScheme1"
                    }
                },
                ["securityScheme2"] = new AsyncApiSecurityScheme
                {
                    Description = "description1",
                    Type = SecuritySchemeType.OpenIdConnect,
                    Scheme = "openIdConnectUrl",
                    OpenIdConnectUrl = new Uri("https://example.com/openIdConnect"),
                    Reference = new AsyncApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "securityScheme2"
                    }
                }
            }
        };

        public static AsyncApiComponents BasicComponents = new AsyncApiComponents();

        public static AsyncApiComponents BrokenComponents = new AsyncApiComponents
        {
            Schemas = new Dictionary<string, AsyncApiSchema>
            {
                ["schema1"] = new AsyncApiSchema
                {
                    Type = "string"
                },
                ["schema2"] = null,
                ["schema3"] = null,
                ["schema4"] = new AsyncApiSchema
                {
                    Type = "string",
                    AllOf = new List<AsyncApiSchema>
                    {
                        null,
                        null,
                        new AsyncApiSchema
                        {
                            Type = "string"
                        },
                        null,
                        null
                    }
                }
            }
        };

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

        private readonly ITestOutputHelper _output;

        public AsyncApiComponentsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeBasicComponentsAsJsonWorks()
        {
            // Arrange
            var expected = @"{ }";

            // Act
            var actual = BasicComponents.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeBasicComponentsAsYamlWorks()
        {
            // Arrange
            var expected = @"{ }";

            // Act
            var actual = BasicComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedComponentsAsJsonV3Works()
        {
            // Arrange
            var expected = @"{
  ""schemas"": {
    ""schema1"": {
      ""properties"": {
        ""property2"": {
          ""type"": ""integer""
        },
        ""property3"": {
          ""maxLength"": 15,
          ""type"": ""string""
        }
      }
    }
  },
  ""securitySchemes"": {
    ""securityScheme1"": {
      ""type"": ""oauth2"",
      ""description"": ""description1"",
      ""flows"": {
        ""implicit"": {
          ""authorizationUrl"": ""https://example.com/api/oauth"",
          ""scopes"": {
            ""operation1:object1"": ""operation 1 on object 1"",
            ""operation2:object2"": ""operation 2 on object 2""
          }
        }
      }
    },
    ""securityScheme2"": {
      ""type"": ""openIdConnect"",
      ""description"": ""description1"",
      ""openIdConnectUrl"": ""https://example.com/openIdConnect""
    }
  }
}";

            // Act
            var actual = AdvancedComponents.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedComponentsWithReferenceAsJsonV3Works()
        {
            // Arrange
            var expected = @"{
  ""schemas"": {
    ""schema1"": {
      ""properties"": {
        ""property2"": {
          ""type"": ""integer""
        },
        ""property3"": {
          ""$ref"": ""#/components/schemas/schema2""
        }
      }
    },
    ""schema2"": {
      ""properties"": {
        ""property2"": {
          ""type"": ""integer""
        }
      }
    }
  },
  ""securitySchemes"": {
    ""securityScheme1"": {
      ""type"": ""oauth2"",
      ""description"": ""description1"",
      ""flows"": {
        ""implicit"": {
          ""authorizationUrl"": ""https://example.com/api/oauth"",
          ""scopes"": {
            ""operation1:object1"": ""operation 1 on object 1"",
            ""operation2:object2"": ""operation 2 on object 2""
          }
        }
      }
    },
    ""securityScheme2"": {
      ""type"": ""openIdConnect"",
      ""description"": ""description1"",
      ""openIdConnectUrl"": ""https://example.com/openIdConnect""
    }
  }
}";

            // Act
            var actual = AdvancedComponentsWithReference.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"schemas:
  schema1:
    properties:
      property2:
        type: integer
      property3:
        maxLength: 15
        type: string
securitySchemes:
  securityScheme1:
    type: oauth2
    description: description1
    flows:
      implicit:
        authorizationUrl: https://example.com/api/oauth
        scopes:
          operation1:object1: operation 1 on object 1
          operation2:object2: operation 2 on object 2
  securityScheme2:
    type: openIdConnect
    description: description1
    openIdConnectUrl: https://example.com/openIdConnect";

            // Act
            var actual = AdvancedComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeAdvancedComponentsWithReferenceAsYamlV3Works()
        {
            // Arrange
            var expected = @"schemas:
  schema1:
    properties:
      property2:
        type: integer
      property3:
        $ref: '#/components/schemas/schema2'
  schema2:
    properties:
      property2:
        type: integer
securitySchemes:
  securityScheme1:
    type: oauth2
    description: description1
    flows:
      implicit:
        authorizationUrl: https://example.com/api/oauth
        scopes:
          operation1:object1: operation 1 on object 1
          operation2:object2: operation 2 on object 2
  securityScheme2:
    type: openIdConnect
    description: description1
    openIdConnectUrl: https://example.com/openIdConnect";

            // Act
            var actual = AdvancedComponentsWithReference.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeBrokenComponentsAsJsonV3Works()
        {
            // Arrange
            var expected = @"{
  ""schemas"": {
    ""schema1"": {
      ""type"": ""string""
    },
    ""schema2"": null,
    ""schema3"": null,
    ""schema4"": {
      ""type"": ""string"",
      ""allOf"": [
        null,
        null,
        {
          ""type"": ""string""
        },
        null,
        null
      ]
    }
  }
}";

            // Act
            var actual = BrokenComponents.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeBrokenComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"schemas:
  schema1:
    type: string
  schema2: 
  schema3: 
  schema4:
    type: string
    allOf:
      - 
      - 
      - type: string
      - 
      - ";

            // Act
            var actual = BrokenComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeTopLevelReferencingComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"schemas:
  schema1:
    $ref: '#/components/schemas/schema2'
  schema2:
    type: object
    properties:
      property1:
        type: string";

            // Act
            var actual = TopLevelReferencingComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeTopLevelSelfReferencingComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"schemas:
  schema1: { }";

            // Act
            var actual = TopLevelSelfReferencingComponents.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeTopLevelSelfReferencingWithOtherPropertiesComponentsAsYamlV3Works()
        {
            // Arrange
            var expected = @"schemas:
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
            var actual = TopLevelSelfReferencingComponentsWithOtherProperties.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
