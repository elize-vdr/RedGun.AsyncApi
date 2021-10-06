// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiSecuritySchemeTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiSecurityScheme/";

        [Fact]
        public void ParseHttpSecuritySchemeShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "httpSecurityScheme.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var securityScheme = AsyncApiV2Deserializer.LoadSecurityScheme(node);

                // Assert
                securityScheme.Should().BeEquivalentTo(
                    new AsyncApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "basic"
                    });
            }
        }

        [Fact]
        public void ParseApiKeySecuritySchemeShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "apiKeySecurityScheme.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var securityScheme = AsyncApiV2Deserializer.LoadSecurityScheme(node);

                // Assert
                securityScheme.Should().BeEquivalentTo(
                    new AsyncApiSecurityScheme
                    {
                        Type = SecuritySchemeType.ApiKey,
                        Name = "api_key",
                        In = ParameterLocation.Header
                    });
            }
        }

        [Fact]
        public void ParseBearerSecuritySchemeShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "bearerSecurityScheme.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var securityScheme = AsyncApiV2Deserializer.LoadSecurityScheme(node);

                // Assert
                securityScheme.Should().BeEquivalentTo(
                    new AsyncApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });
            }
        }

        [Fact]
        public void ParseOAuth2SecuritySchemeShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "oauth2SecurityScheme.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var securityScheme = AsyncApiV2Deserializer.LoadSecurityScheme(node);

                // Assert
                securityScheme.Should().BeEquivalentTo(
                    new AsyncApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new AsyncApiOAuthFlows
                        {
                            Implicit = new AsyncApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri("https://example.com/api/oauth/dialog"),
                                Scopes =
                                {
                                    ["write:pets"] = "modify pets in your account",
                                    ["read:pets"] = "read your pets"
                                }
                            }
                        }
                    });
            }
        }

        [Fact]
        public void ParseOpenIdConnectSecuritySchemeShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "openIdConnectSecurityScheme.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var securityScheme = AsyncApiV2Deserializer.LoadSecurityScheme(node);

                // Assert
                securityScheme.Should().BeEquivalentTo(
                    new AsyncApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OpenIdConnect,
                        Description = "Sample Description",
                        OpenIdConnectUrl = new Uri("http://www.example.com")
                    });
            }
        }
    }
}
