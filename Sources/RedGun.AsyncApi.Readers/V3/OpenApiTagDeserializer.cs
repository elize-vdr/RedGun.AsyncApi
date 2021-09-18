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
        private static readonly FixedFieldMap<AsyncApiTag> _tagFixedFields = new FixedFieldMap<AsyncApiTag>
        {
            {
                OpenApiConstants.Name, (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                OpenApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                OpenApiConstants.ExternalDocs, (o, n) =>
                {
                    o.ExternalDocs = LoadExternalDocs(n);
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiTag> _tagPatternFields = new PatternFieldMap<AsyncApiTag>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiTag LoadTag(ParseNode n)
        {
            var mapNode = n.CheckMapNode("tag");

            var domainObject = new AsyncApiTag();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, _tagFixedFields, _tagPatternFields);
            }

            return domainObject;
        }
    }
}
