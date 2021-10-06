// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Linq;
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
        private static readonly FixedFieldMap<AsyncApiMediaType> _mediaTypeFixedFields =
            new FixedFieldMap<AsyncApiMediaType>
            {
                {
                    AsyncApiConstants.Schema, (o, n) =>
                    {
                        o.Schema = LoadSchema(n);
                    }
                },
                {
                    AsyncApiConstants.Examples, (o, n) =>
                    {
                        o.Examples = n.CreateMap(LoadExample);
                    }
                },
                {
                    AsyncApiConstants.Example, (o, n) =>
                    {
                        o.Example = n.CreateAny();
                    }
                },
                {
                    AsyncApiConstants.Encoding, (o, n) =>
                    {
                        o.Encoding = n.CreateMap(LoadEncoding);
                    }
                },
            };

        private static readonly PatternFieldMap<AsyncApiMediaType> _mediaTypePatternFields =
            new PatternFieldMap<AsyncApiMediaType>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        private static readonly AnyFieldMap<AsyncApiMediaType> _mediaTypeAnyFields = new AnyFieldMap<AsyncApiMediaType>
        {
            {
                AsyncApiConstants.Example,
                new AnyFieldMapParameter<AsyncApiMediaType>(
                    s => s.Example,
                    (s, v) => s.Example = v,
                    s => s.Schema)
            }
        };


        private static readonly AnyMapFieldMap<AsyncApiMediaType, AsyncApiExample> _mediaTypeAnyMapAsyncApiExampleFields =
            new AnyMapFieldMap<AsyncApiMediaType, AsyncApiExample>
        {
            {
                AsyncApiConstants.Examples,
                new AnyMapFieldMapParameter<AsyncApiMediaType, AsyncApiExample>(
                    m => m.Examples,
                    e => e.Value,
                    (e, v) => e.Value = v,
                    m => m.Schema)
            }
        };

        public static AsyncApiMediaType LoadMediaType(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Content);

            if (!mapNode.Any())
            {
                return null;
            }

            var mediaType = new AsyncApiMediaType();

            ParseMap(mapNode, mediaType, _mediaTypeFixedFields, _mediaTypePatternFields);

            ProcessAnyFields(mapNode, mediaType, _mediaTypeAnyFields);
            ProcessAnyMapFields(mapNode, mediaType, _mediaTypeAnyMapAsyncApiExampleFields);

            return mediaType;
        }
    }
}
