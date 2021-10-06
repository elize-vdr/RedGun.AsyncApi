// Copyright (c) Microsoft Corporation. All rights reserved.
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
        private static readonly FixedFieldMap<AsyncApiLink> _linkFixedFields = new FixedFieldMap<AsyncApiLink>
        {
            {
                "operationRef", (o, n) =>
                {
                    o.OperationRef = n.GetScalarValue();
                }
            },
            {
                "operationId", (o, n) =>
                {
                    o.OperationId = n.GetScalarValue();
                }
            },
            {
                "parameters", (o, n) =>
                {
                    o.Parameters = n.CreateSimpleMap(LoadRuntimeExpressionAnyWrapper);
                }
            },
            {
                "requestBody", (o, n) =>
                {
                    o.RequestBody = LoadRuntimeExpressionAnyWrapper(n);
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {"server", (o, n) => o.Server = LoadServer(n)}
        };

        private static readonly PatternFieldMap<AsyncApiLink> _linkPatternFields = new PatternFieldMap<AsyncApiLink>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
        };

        public static AsyncApiLink LoadLink(ParseNode node)
        {
            var mapNode = node.CheckMapNode("link");
            var link = new AsyncApiLink();

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiLink>(ReferenceType.Link, pointer);
            }

            ParseMap(mapNode, link, _linkFixedFields, _linkPatternFields);

            return link;
        }
    }
}
