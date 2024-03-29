﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Open API V2 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiLicense> _licenseFixedFields = new FixedFieldMap<AsyncApiLicense>
        {
            {
                AsyncApiConstants.Name, (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Url, (o, n) =>
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
            var mapNode = node.CheckMapNode(AsyncApiConstants.License);

            var license = new AsyncApiLicense();

            ParseMap(mapNode, license, _licenseFixedFields, _licensePatternFields);

            return license;
        }
    }
}
