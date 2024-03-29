﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Open API V2 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiServerVariable> _serverVariableFixedFields =
            new FixedFieldMap<AsyncApiServerVariable>
            {
                {
                    AsyncApiConstants.Enum, (o, n) =>
                    {
                        o.Enum = n.CreateSimpleList(s => s.GetScalarValue());
                    }
                },
                {
                    AsyncApiConstants.Default, (o, n) =>
                    {
                        o.Default = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Description, (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Examples, (o, n) =>
                    {
                        o.Examples = n.CreateSimpleList(s => s.GetScalarValue());
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiServerVariable> _serverVariablePatternFields =
            new PatternFieldMap<AsyncApiServerVariable>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiServerVariable LoadServerVariable(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.ServerVariable);

            var serverVariable = new AsyncApiServerVariable();

            ParseMap(mapNode, serverVariable, _serverVariableFixedFields, _serverVariablePatternFields);

            return serverVariable;
        }
    }
}
