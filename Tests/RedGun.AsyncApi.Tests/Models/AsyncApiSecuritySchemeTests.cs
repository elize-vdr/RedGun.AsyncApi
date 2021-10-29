// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;
using Xunit;
using Xunit.Abstractions;

namespace RedGun.AsyncApi.Tests.Models
{
    [Collection("DefaultSettings")]
    public class AsyncApiSecuritySchemeTests
    {
        public static AsyncApiSecurityScheme ApiKeySecurityScheme = new AsyncApiSecurityScheme
        {
            Description = "description1",
            Name = "parameterName",
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Query,
        };

        public static AsyncApiSecurityScheme HttpBasicSecurityScheme = new AsyncApiSecurityScheme
        {
            Description = "description1",
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
        };

        public static AsyncApiSecurityScheme HttpBearerSecurityScheme = new AsyncApiSecurityScheme
        {
            Description = "description1",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        };

        public static AsyncApiSecurityScheme OAuth2SingleFlowSecurityScheme = new AsyncApiSecurityScheme
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
        };

        public static AsyncApiSecurityScheme OAuth2MultipleFlowSecurityScheme = new AsyncApiSecurityScheme
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
                },
                ClientCredentials = new AsyncApiOAuthFlow
                {
                    Scopes = new Dictionary<string, string>
                    {
                        ["operation1:object1"] = "operation 1 on object 1",
                        ["operation2:object2"] = "operation 2 on object 2"
                    },
                    TokenUrl = new Uri("https://example.com/api/token"),
                    RefreshUrl = new Uri("https://example.com/api/refresh"),
                },
                AuthorizationCode = new AsyncApiOAuthFlow
                {
                    Scopes = new Dictionary<string, string>
                    {
                        ["operation1:object1"] = "operation 1 on object 1",
                        ["operation2:object2"] = "operation 2 on object 2"
                    },
                    TokenUrl = new Uri("https://example.com/api/token"),
                    AuthorizationUrl = new Uri("https://example.com/api/oauth"),
                }
            }
        };

        public static AsyncApiSecurityScheme OpenIdConnectSecurityScheme = new AsyncApiSecurityScheme
        {
            Description = "description1",
            Type = SecuritySchemeType.OpenIdConnect,
            Scheme = "openIdConnectUrl",
            OpenIdConnectUrl = new Uri("https://example.com/openIdConnect")
        };

        public static AsyncApiSecurityScheme ReferencedSecurityScheme = new AsyncApiSecurityScheme
        {
            Description = "description1",
            Type = SecuritySchemeType.OpenIdConnect,
            Scheme = "openIdConnectUrl",
            OpenIdConnectUrl = new Uri("https://example.com/openIdConnect"),
            Reference = new AsyncApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "sampleSecurityScheme"
            }
        };

        private readonly ITestOutputHelper _output;

        public AsyncApiSecuritySchemeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SerializeApiKeySecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""type"": ""apiKey"",
  ""description"": ""description1"",
  ""name"": ""parameterName"",
  ""in"": ""query""
}";

            // Act
            var actual = ApiKeySecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeApiKeySecuritySchemeAsV2YamlWorks()
        {
            // Arrange
            var expected =
                @"type: apiKey
description: description1
name: parameterName
in: query";

            // Act
            var actual = ApiKeySecurityScheme.SerializeAsYaml(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeHttpBasicSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""type"": ""http"",
  ""description"": ""description1"",
  ""scheme"": ""basic""
}";

            // Act
            var actual = HttpBasicSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeHttpBearerSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""type"": ""http"",
  ""description"": ""description1"",
  ""scheme"": ""bearer"",
  ""bearerFormat"": ""JWT""
}";

            // Act
            var actual = HttpBearerSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeOAuthSingleFlowSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
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
}";

            // Act
            var actual = OAuth2SingleFlowSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeOAuthMultipleFlowSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""type"": ""oauth2"",
  ""description"": ""description1"",
  ""flows"": {
    ""implicit"": {
      ""authorizationUrl"": ""https://example.com/api/oauth"",
      ""scopes"": {
        ""operation1:object1"": ""operation 1 on object 1"",
        ""operation2:object2"": ""operation 2 on object 2""
      }
    },
    ""clientCredentials"": {
      ""tokenUrl"": ""https://example.com/api/token"",
      ""refreshUrl"": ""https://example.com/api/refresh"",
      ""scopes"": {
        ""operation1:object1"": ""operation 1 on object 1"",
        ""operation2:object2"": ""operation 2 on object 2""
      }
    },
    ""authorizationCode"": {
      ""authorizationUrl"": ""https://example.com/api/oauth"",
      ""tokenUrl"": ""https://example.com/api/token"",
      ""scopes"": {
        ""operation1:object1"": ""operation 1 on object 1"",
        ""operation2:object2"": ""operation 2 on object 2""
      }
    }
  }
}";

            // Act
            var actual = OAuth2MultipleFlowSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeOpenIdConnectSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var expected =
                @"{
  ""type"": ""openIdConnect"",
  ""description"": ""description1"",
  ""openIdConnectUrl"": ""https://example.com/openIdConnect""
}";

            // Act
            var actual = OpenIdConnectSecurityScheme.SerializeAsJson(AsyncApiSpecVersion.AsyncApi2_0);

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedSecuritySchemeAsV2JsonWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""sampleSecurityScheme"": null
}";

            // Act
            // Add dummy start object, value, and end object to allow SerializeAsV2 to output security scheme 
            // as property name.
            writer.WriteStartObject();
            ReferencedSecurityScheme.SerializeAsV2(writer);
            writer.WriteNull();
            writer.WriteEndObject();
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }

        [Fact]
        public void SerializeReferencedSecuritySchemeAsV2JsonWithoutReferenceWorks()
        {
            // Arrange
            var outputStringWriter = new StringWriter(CultureInfo.InvariantCulture);
            var writer = new AsyncApiJsonWriter(outputStringWriter);
            var expected =
                @"{
  ""type"": ""openIdConnect"",
  ""description"": ""description1"",
  ""openIdConnectUrl"": ""https://example.com/openIdConnect""
}";

            // Act
            ReferencedSecurityScheme.SerializeAsV2WithoutReference(writer);
            writer.Flush();
            var actual = outputStringWriter.GetStringBuilder().ToString();

            // Assert
            actual = actual.MakeLineBreaksEnvironmentNeutral();
            expected = expected.MakeLineBreaksEnvironmentNeutral();
            actual.Should().Be(expected);
        }
    }
}
