// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiServerVariable> _serverVariableFixedFields =
            new FixedFieldMap<AsyncApiServerVariable>
            {
                {
                    "enum", (o, n) =>
                    {
                        o.Enum = n.CreateSimpleList(s => s.GetScalarValue());
                    }
                },
                {
                    "default", (o, n) =>
                    {
                        o.Default = n.GetScalarValue();
                    }
                },
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
            };

        private static readonly PatternFieldMap<AsyncApiServerVariable> _serverVariablePatternFields =
            new PatternFieldMap<AsyncApiServerVariable>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiServerVariable LoadServerVariable(ParseNode node)
        {
            var mapNode = node.CheckMapNode("serverVariable");

            var serverVariable = new AsyncApiServerVariable();

            ParseMap(mapNode, serverVariable, _serverVariableFixedFields, _serverVariablePatternFields);

            return serverVariable;
        }
    }
}
