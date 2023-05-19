// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
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
        private static readonly FixedFieldMap<AsyncApiSecurityScheme> _securitySchemeFixedFields =
            new FixedFieldMap<AsyncApiSecurityScheme>
            {
                {
                    AsyncApiConstants.Type, (o, n) =>
                    {
                        o.Type = n.GetScalarValue().GetEnumFromDisplayName<SecuritySchemeType>();
                    }
                },
                {
                    AsyncApiConstants.Description, (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.Name, (o, n) =>
                    {
                        o.Name = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.In, (o, n) =>
                    {
                        o.In = n.GetScalarValue().GetEnumFromDisplayName<ParameterLocation>();
                    }
                },
                {
                    AsyncApiConstants.Scheme, (o, n) =>
                    {
                        o.Scheme = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.BearerFormat, (o, n) =>
                    {
                        o.BearerFormat = n.GetScalarValue();
                    }
                },
                {
                    AsyncApiConstants.OpenIdConnectUrl, (o, n) =>
                    {
                        o.OpenIdConnectUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    AsyncApiConstants.Flows, (o, n) =>
                    {
                        o.Flows = LoadOAuthFlows(n);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiSecurityScheme> _securitySchemePatternFields =
            new PatternFieldMap<AsyncApiSecurityScheme>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiSecurityScheme LoadSecurityScheme(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.SecurityScheme);

            var securityScheme = new AsyncApiSecurityScheme();
            foreach (var property in mapNode)
            {
                property.ParseField(securityScheme, _securitySchemeFixedFields, _securitySchemePatternFields);
            }

            return securityScheme;
        }
    }
}
