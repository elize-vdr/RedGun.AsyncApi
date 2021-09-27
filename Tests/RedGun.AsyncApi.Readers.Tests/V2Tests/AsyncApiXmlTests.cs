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
    public class AsyncApiXmlTests
    {
        private const string SampleFolderPath = "V2Tests/Samples/AsyncApiXml/";

        [Fact]
        public void ParseBasicXmlShouldSucceed()
        {
            using (var stream = Resources.GetStream(Path.Combine(SampleFolderPath, "basicXml.yaml")))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(new StreamReader(stream));
                var yamlNode = yamlStream.Documents.First().RootNode;

                var diagnostic = new AsyncApiDiagnostic();
                var context = new ParsingContext(diagnostic);

                var node = new MapNode(context, (YamlMappingNode)yamlNode);

                // Act
                var xml = AsyncApiV2Deserializer.LoadXml(node);

                // Assert
                xml.Should().BeEquivalentTo(
                    new AsyncApiXml
                    {
                        Name = "name1",
                        Namespace = new Uri("http://example.com/schema/namespaceSample"),
                        Prefix = "samplePrefix",
                        Wrapped = true
                    });
            }
        }
    }
}
