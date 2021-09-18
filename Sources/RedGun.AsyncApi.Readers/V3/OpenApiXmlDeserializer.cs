// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
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
        private static readonly FixedFieldMap<AsyncApiXml> _xmlFixedFields = new FixedFieldMap<AsyncApiXml>
        {
            {
                "name", (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                "namespace", (o, n) =>
                {
                    o.Namespace = new Uri(n.GetScalarValue(), UriKind.Absolute);
                }
            },
            {
                "prefix", (o, n) =>
                {
                    o.Prefix = n.GetScalarValue();
                }
            },
            {
                "attribute", (o, n) =>
                {
                    o.Attribute = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "wrapped", (o, n) =>
                {
                    o.Wrapped = bool.Parse(n.GetScalarValue());
                }
            },
        };

        private static readonly PatternFieldMap<AsyncApiXml> _xmlPatternFields =
            new PatternFieldMap<AsyncApiXml>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiXml LoadXml(ParseNode node)
        {
            var mapNode = node.CheckMapNode("xml");

            var xml = new AsyncApiXml();
            foreach (var property in mapNode)
            {
                property.ParseField(xml, _xmlFixedFields, _xmlPatternFields);
            }

            return xml;
        }
    }
}
