// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
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
        public static FixedFieldMap<AsyncApiInfo> InfoFixedFields = new FixedFieldMap<AsyncApiInfo>
        {
            {
                "title", (o, n) =>
                {
                    o.Title = n.GetScalarValue();
                }
            },
            {
                "version", (o, n) =>
                {
                    o.Version = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "termsOfService", (o, n) =>
                {
                    o.TermsOfService = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                }
            },
            {
                "contact", (o, n) =>
                {
                    o.Contact = LoadContact(n);
                }
            },
            {
                "license", (o, n) =>
                {
                    o.License = LoadLicense(n);
                }
            }
        };

        public static PatternFieldMap<AsyncApiInfo> InfoPatternFields = new PatternFieldMap<AsyncApiInfo>
        {
            {s => s.StartsWith("x-"), (o, k, n) => o.AddExtension(k,LoadExtension(k, n))}
        };

        public static AsyncApiInfo LoadInfo(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Info");

            var info = new AsyncApiInfo();
            var required = new List<string> { "title", "version" };

            ParseMap(mapNode, info, InfoFixedFields, InfoPatternFields);

            return info;
        }
    }
}
