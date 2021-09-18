// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V3
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV3Deserializer
    {
        public static FixedFieldMap<AsyncApiResponses> ResponsesFixedFields = new FixedFieldMap<AsyncApiResponses>();

        public static PatternFieldMap<AsyncApiResponses> ResponsesPatternFields = new PatternFieldMap<AsyncApiResponses>
        {
            {s => !s.StartsWith("x-"), (o, p, n) => o.Add(p, LoadResponse(n))},
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiResponses LoadResponses(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Responses");

            var domainObject = new AsyncApiResponses();

            ParseMap(mapNode, domainObject, ResponsesFixedFields, ResponsesPatternFields);

            return domainObject;
        }
    }
}
