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
        private static readonly FixedFieldMap<AsyncApiServer> _serverFixedFields = new FixedFieldMap<AsyncApiServer>
        {
            {
                "url", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "variables", (o, n) =>
                {
                    o.Variables = n.CreateMap(LoadServerVariable);
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiServer> _serverPatternFields = new PatternFieldMap<AsyncApiServer>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiServer LoadServer(ParseNode node)
        {
            var mapNode = node.CheckMapNode("server");

            var server = new AsyncApiServer();

            ParseMap(mapNode, server, _serverFixedFields, _serverPatternFields);

            return server;
        }
    }
}
