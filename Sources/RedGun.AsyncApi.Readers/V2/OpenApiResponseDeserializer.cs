// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
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
        private static readonly FixedFieldMap<AsyncApiResponse> _responseFixedFields = new FixedFieldMap<AsyncApiResponse>
        {
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "headers", (o, n) =>
                {
                    o.Headers = n.CreateMap(LoadHeader);
                }
            },
            {
                "content", (o, n) =>
                {
                    o.Content = n.CreateMap(LoadMediaType);
                }
            },
            {
                "links", (o, n) =>
                {
                    o.Links = n.CreateMap(LoadLink);
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiResponse> _responsePatternFields =
            new PatternFieldMap<AsyncApiResponse>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiResponse LoadResponse(ParseNode node)
        {
            var mapNode = node.CheckMapNode("response");

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiResponse>(ReferenceType.Response, pointer);
            }

            var requiredFields = new List<string> { "description" };
            var response = new AsyncApiResponse();
            ParseMap(mapNode, response, _responseFixedFields, _responsePatternFields);

            return response;
        }
    }
}
