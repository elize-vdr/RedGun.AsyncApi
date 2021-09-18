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
        private static FixedFieldMap<AsyncApiLicense> _licenseFixedFields = new FixedFieldMap<AsyncApiLicense>
        {
            {
                "name", (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                "url", (o, n) =>
                {
                    o.Url = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                }
            },
        };

        private static PatternFieldMap<AsyncApiLicense> _licensePatternFields = new PatternFieldMap<AsyncApiLicense>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        internal static AsyncApiLicense LoadLicense(ParseNode node)
        {
            var mapNode = node.CheckMapNode("License");

            var license = new AsyncApiLicense();

            ParseMap(mapNode, license, _licenseFixedFields, _licensePatternFields);

            return license;
        }
    }
}
