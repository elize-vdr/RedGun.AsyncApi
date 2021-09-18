// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Interfaces;
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
        private static FixedFieldMap<AsyncApiComponents> _componentsFixedFields = new FixedFieldMap<AsyncApiComponents>
        {
            {
                "schemas", (o, n) => o.Schemas = n.CreateMapWithReference(ReferenceType.Schema, LoadSchema)
            },
            {"responses", (o, n) => o.Responses = n.CreateMapWithReference(ReferenceType.Response, LoadResponse)},
            {"parameters", (o, n) => o.Parameters = n.CreateMapWithReference(ReferenceType.Parameter, LoadParameter)},
            {"examples", (o, n) => o.Examples = n.CreateMapWithReference(ReferenceType.Example, LoadExample)},
            {"requestBodies", (o, n) => o.RequestBodies = n.CreateMapWithReference(ReferenceType.RequestBody, LoadRequestBody)},
            {"headers", (o, n) => o.Headers = n.CreateMapWithReference(ReferenceType.Header, LoadHeader)},
            {"securitySchemes", (o, n) => o.SecuritySchemes = n.CreateMapWithReference(ReferenceType.SecurityScheme, LoadSecurityScheme)},
            {"links", (o, n) => o.Links = n.CreateMapWithReference(ReferenceType.Link, LoadLink)},
            {"callbacks", (o, n) => o.Callbacks = n.CreateMapWithReference(ReferenceType.Callback, LoadCallback)},
        };


        private static PatternFieldMap<AsyncApiComponents> _componentsPatternFields =
            new PatternFieldMap<AsyncApiComponents>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiComponents LoadComponents(ParseNode node)
        {
            var mapNode = node.CheckMapNode("components");
            var components = new AsyncApiComponents();

            ParseMap(mapNode, components, _componentsFixedFields, _componentsPatternFields);

            return components;
        }
    }
}
