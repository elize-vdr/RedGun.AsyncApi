﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

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
        private static readonly FixedFieldMap<AsyncApiDiscriminator> _discriminatorFixedFields =
            new FixedFieldMap<AsyncApiDiscriminator>
            {
                {
                    "propertyName", (o, n) =>
                    {
                        o.PropertyName = n.GetScalarValue();
                    }
                },
                {
                    "mapping", (o, n) =>
                    {
                        o.Mapping = n.CreateSimpleMap(LoadString);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiDiscriminator> _discriminatorPatternFields =
            new PatternFieldMap<AsyncApiDiscriminator>();

        public static AsyncApiDiscriminator LoadDiscriminator(ParseNode node)
        {
            var mapNode = node.CheckMapNode("discriminator");

            var discriminator = new AsyncApiDiscriminator();
            foreach (var property in mapNode)
            {
                property.ParseField(discriminator, _discriminatorFixedFields, _discriminatorPatternFields);
            }

            return discriminator;
        }
    }
}