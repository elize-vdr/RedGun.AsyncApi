// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using System.Linq;
using RedGun.AsyncApi.Readers.ParseNodes;
using SharpYaml.Serialization;

namespace RedGun.AsyncApi.Readers.Tests
{
    internal class TestHelper
    {
        public static MapNode CreateYamlMapNode(Stream stream)
        {
            var yamlStream = new YamlStream();
            yamlStream.Load(new StreamReader(stream));
            var yamlNode = yamlStream.Documents.First().RootNode;

            var context = new ParsingContext(new AsyncApiDiagnostic());

            return new MapNode(context, (YamlMappingNode)yamlNode);
        }
    }
}
