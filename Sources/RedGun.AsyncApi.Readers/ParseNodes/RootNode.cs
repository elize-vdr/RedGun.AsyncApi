// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using SharpYaml.Serialization;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    /// <summary>
    /// Wrapper class around YamlDocument to isolate semantic parsing from details of Yaml DOM.
    /// </summary>
    internal class RootNode : ParseNode
    {
        private readonly YamlDocument _yamlDocument;

        public RootNode(
            ParsingContext context,
            YamlDocument yamlDocument) : base(context)
        {
            _yamlDocument = yamlDocument;
        }

        public ParseNode Find(JsonPointer referencePointer)
        {
            var yamlNode = referencePointer.Find(_yamlDocument.RootNode);
            if (yamlNode == null)
            {
                return null;
            }

            return Create(Context, yamlNode);
        }

        public MapNode GetMap()
        {
            return new MapNode(Context, (YamlMappingNode)_yamlDocument.RootNode);
        }
    }
}
