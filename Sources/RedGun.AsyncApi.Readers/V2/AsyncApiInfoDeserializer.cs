// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
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
        public static FixedFieldMap<AsyncApiInfo> InfoFixedFields = new FixedFieldMap<AsyncApiInfo>
        {
            {
                AsyncApiConstants.Title, (o, n) =>
                {
                    o.Title = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Version, (o, n) =>
                {
                    o.Version = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.TermsOfService, (o, n) =>
                {
                    o.TermsOfService = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                }
            },
            {
                AsyncApiConstants.Contact, (o, n) =>
                {
                    o.Contact = LoadContact(n);
                }
            },
            {
                AsyncApiConstants.License, (o, n) =>
                {
                    o.License = LoadLicense(n);
                }
            }
        };

        public static PatternFieldMap<AsyncApiInfo> InfoPatternFields = new PatternFieldMap<AsyncApiInfo>
        {
            { s => s.StartsWith("x-"), (o, k, n) => o.AddExtension(k, LoadExtension(k, n)) }
        };

        public static AsyncApiInfo LoadInfo(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Info);

            var info = new AsyncApiInfo();
            var required = new List<string> { AsyncApiConstants.Title, AsyncApiConstants.Version };

            ParseMap(mapNode, info, InfoFixedFields, InfoPatternFields);

            return info;
        }
    }
}
