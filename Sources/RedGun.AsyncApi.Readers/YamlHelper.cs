﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.IO;
using System.Linq;
using RedGun.AsyncApi.Exceptions;
using SharpYaml.Serialization;

namespace RedGun.AsyncApi.Readers
{
    internal static class YamlHelper
    {
        public static string GetScalarValue(this YamlNode node)
        {
            var scalarNode = node as YamlScalarNode;
            if (scalarNode == null)
            {
                throw new AsyncApiException($"Expected scalar at line {node.Start.Line}");
            }

            return scalarNode.Value;
        }

        public static YamlNode ParseYamlString(string yamlString)
        {
            var reader = new StringReader(yamlString);
            var yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var yamlDocument = yamlStream.Documents.First();
            return yamlDocument.RootNode;
        }
    }
}
