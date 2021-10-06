// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

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
    public class OpenApiEncodingTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/OpenApiEncoding/";

        [Fact]
        public void ParseBasicEncodingShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicEncoding.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var encoding = AsyncApiV2Deserializer.LoadEncoding(node);

                // Assert
                encoding.Should().BeEquivalentTo(
                    new AsyncApiEncoding
                    {
                        ContentType = "application/xml; charset=utf-8"
                    });
            }
        }

        [Fact]
        public void ParseAdvancedEncodingShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "advancedEncoding.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var encoding = AsyncApiV2Deserializer.LoadEncoding(node);

                // Assert
                encoding.Should().BeEquivalentTo(
                    new AsyncApiEncoding
                    {
                        ContentType = "image/png, image/jpeg",
                        Headers =
                        {
                            ["X-Rate-Limit-Limit"] =
                            new AsyncApiHeader
                            {
                                Description = "The number of allowed requests in the current period",
                                Schema = new AsyncApiSchema
                                {
                                    Type = "integer"
                                }
                            }
                        }
                    });
            }
        }
    }
}
