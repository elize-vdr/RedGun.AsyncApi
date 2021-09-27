// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
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
    public class AsyncApiInfoTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiInfo/";

        [Fact]
        public void ParseAdvancedInfoShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "advancedInfo.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var AsyncApiInfo = AsyncApiV2Deserializer.LoadInfo(node);

                // Assert
                AsyncApiInfo.Should().BeEquivalentTo(
                    new AsyncApiInfo
                    {
                        Title = "Advanced Info",
                        Description = "Sample Description",
                        Version = "1.0.0",
                        TermsOfService = new Uri("http://example.org/termsOfService"),
                        Contact = new AsyncApiContact
                        {
                            Email = "example@example.com",
                            Extensions =
                            {
                                ["x-twitter"] = new AsyncApiString("@exampleTwitterHandler")
                            },
                            Name = "John Doe",
                            Url = new Uri("http://www.example.com/url1")
                        },
                        License = new AsyncApiLicense
                        {
                            Extensions = { ["x-disclaimer"] = new AsyncApiString("Sample Extension String Disclaimer") },
                            Name = "licenseName",
                            Url = new Uri("http://www.example.com/url2")
                        },
                        Extensions =
                        {
                            ["x-something"] = new AsyncApiString("Sample Extension String Something"),
                            ["x-contact"] = new AsyncApiObject
                            {
                                ["name"] = new AsyncApiString("John Doe"),
                                ["url"] = new AsyncApiString("http://www.example.com/url3"),
                                ["email"] = new AsyncApiString("example@example.com")
                            },
                            ["x-list"] = new AsyncApiArray
                            {
                                new AsyncApiString("1"),
                                new AsyncApiString("2")
                            }
                        }
                    });
            }
        }

        [Fact]
        public void ParseBasicInfoShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicInfo.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var AsyncApiInfo = AsyncApiV2Deserializer.LoadInfo(node);

                // Assert
                AsyncApiInfo.Should().BeEquivalentTo(
                    new AsyncApiInfo
                    {
                        Title = "Basic Info",
                        Description = "Sample Description",
                        Version = "1.0.1",
                        TermsOfService = new Uri("http://swagger.io/terms/"),
                        Contact = new AsyncApiContact
                        {
                            Email = "support@swagger.io",
                            Name = "API Support",
                            Url = new Uri("http://www.swagger.io/support")
                        },
                        License = new AsyncApiLicense
                        {
                            Name = "Apache 2.0",
                            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
                        }
                    });
            }
        }

        [Fact]
        public void ParseMinimalInfoShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "minimalInfo.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var AsyncApiInfo = AsyncApiV2Deserializer.LoadInfo(node);

                // Assert
                AsyncApiInfo.Should().BeEquivalentTo(
                    new AsyncApiInfo
                    {
                        Title = "Minimal Info",
                        Version = "1.0.1"
                    });
            }
        }
    }
}
