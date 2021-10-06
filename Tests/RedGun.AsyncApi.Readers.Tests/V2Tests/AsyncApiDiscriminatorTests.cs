// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using System.Linq;
using FluentAssertions;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using RedGun.AsyncApi.Models;
using SharpYaml.Serialization;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class OpenApiDiscriminatorTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/OpenApiDiscriminator/";

        [Fact]
        public void ParseBasicDiscriminatorShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicDiscriminator.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var discriminator = AsyncApiV2Deserializer.LoadDiscriminator(node);

                // Assert
                discriminator.Should().BeEquivalentTo(
                    new AsyncApiDiscriminator
                    {
                        PropertyName = "pet_type",
                        Mapping =
                        {
                            ["puppy"] = "#/components/schemas/Dog",
                            ["kitten"] = "Cat"
                        }
                    });
            }
        }
    }
}
