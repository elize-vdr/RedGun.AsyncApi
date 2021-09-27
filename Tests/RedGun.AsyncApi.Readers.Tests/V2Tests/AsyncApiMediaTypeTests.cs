// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using FluentAssertions;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using RedGun.AsyncApi.Readers.V2;
using Xunit;

namespace RedGun.AsyncApi.Readers.Tests.V2Tests
{
    [Collection("DefaultSettings")]
    public class AsyncApiMediaTypeTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiMediaType/";

        [Fact]
        public void ParseMediaTypeWithExampleShouldSucceed()
        {
            // Arrange
            MapNode node;
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "mediaTypeWithExample.yaml")))
            {
                node = TestHelper.CreateYamlMapNode(stream);
            }

            // Act
            var mediaType = AsyncApiV2Deserializer.LoadMediaType(node);

            // Assert
            mediaType.Should().BeEquivalentTo(
                new AsyncApiMediaType
                {
                    Example = new AsyncApiFloat(5),
                    Schema = new AsyncApiSchema
                    {
                        Type = "number",
                        Format = "float"
                    }
                });
        }

        [Fact]
        public void ParseMediaTypeWithExamplesShouldSucceed()
        {
            // Arrange
            MapNode node;
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "mediaTypeWithExamples.yaml")))
            {
                node = TestHelper.CreateYamlMapNode(stream);
            }

            // Act
            var mediaType = AsyncApiV2Deserializer.LoadMediaType(node);

            // Assert
            mediaType.Should().BeEquivalentTo(
                new AsyncApiMediaType
                {
                    Examples =
                    {
                        ["example1"] = new AsyncApiExample()
                        {
                            Value = new AsyncApiFloat(5),
                        },
                        ["example2"] = new AsyncApiExample()
                        {
                            Value = new AsyncApiFloat((float)7.5),
                        }
                    },
                    Schema = new AsyncApiSchema
                    {
                        Type = "number",
                        Format = "float"
                    }
                });
        }
    }
}
