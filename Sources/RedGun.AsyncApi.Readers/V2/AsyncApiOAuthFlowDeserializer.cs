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
        private static readonly FixedFieldMap<AsyncApiOAuthFlow> _oAuthFlowFixedFileds =
            new FixedFieldMap<AsyncApiOAuthFlow>
            {
                {
                    AsyncApiConstants.AuthorizationUrl, (o, n) =>
                    {
                        o.AuthorizationUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    AsyncApiConstants.TokenUrl, (o, n) =>
                    {
                        o.TokenUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    AsyncApiConstants.RefreshUrl, (o, n) =>
                    {
                        o.RefreshUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {AsyncApiConstants.Scopes, (o, n) =>
                    {
                        o.Scopes = n.CreateSimpleMap(LoadString);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlow> _oAuthFlowPatternFields =
            new PatternFieldMap<AsyncApiOAuthFlow>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiOAuthFlow LoadOAuthFlow(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Flow);

            var oauthFlow = new AsyncApiOAuthFlow();
            foreach (var property in mapNode)
            {
                property.ParseField(oauthFlow, _oAuthFlowFixedFileds, _oAuthFlowPatternFields);
            }

            return oauthFlow;
        }
    }
}
