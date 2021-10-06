// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

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
    public class OpenApiExampleTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/OpenApiExample/";

        [Fact]
        public void ParseAdvancedExampleShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "advancedExample.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                var example = AsyncApiV2Deserializer.LoadExample(node);

                diagnostic.Errors.Should().BeEmpty();

                example.Should().BeEquivalentTo(
                    new AsyncApiExample
                    {
                        Value = new AsyncApiObject
                        {
                            ["versions"] = new AsyncApiArray
                            {
                                new AsyncApiObject
                                {
                                    ["status"] = new AsyncApiString("Status1"),
                                    ["id"] = new AsyncApiString("v1"),
                                    ["links"] = new AsyncApiArray
                                    {
                                        new AsyncApiObject
                                        {
                                            ["href"] = new AsyncApiString("http://example.com/1"),
                                            ["rel"] = new AsyncApiString("sampleRel1")
                                        }
                                    }
                                },

                                new AsyncApiObject
                                {
                                    ["status"] = new AsyncApiString("Status2"),
                                    ["id"] = new AsyncApiString("v2"),
                                    ["links"] = new AsyncApiArray
                                    {
                                        new AsyncApiObject
                                        {
                                            ["href"] = new AsyncApiString("http://example.com/2"),
                                            ["rel"] = new AsyncApiString("sampleRel2")
                                        }
                                    }
                                }
                            }
                        }
                    });
            }
        }

        [Fact]
        public void ParseExampleForcedStringSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "explicitString.yaml")))
            {
                new AsyncApiStreamReader().Read(stream, out var diagnostic);
                diagnostic.Errors.Should().BeEmpty();
            }
        }
    }
}
