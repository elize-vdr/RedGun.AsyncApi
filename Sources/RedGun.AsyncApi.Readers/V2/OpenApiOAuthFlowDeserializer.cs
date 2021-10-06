// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiOAuthFlow> _oAuthFlowFixedFileds =
            new FixedFieldMap<AsyncApiOAuthFlow>
            {
                {
                    "authorizationUrl", (o, n) =>
                    {
                        o.AuthorizationUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    "tokenUrl", (o, n) =>
                    {
                        o.TokenUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    "refreshUrl", (o, n) =>
                    {
                        o.RefreshUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {"scopes", (o, n) => o.Scopes = n.CreateSimpleMap(LoadString)}
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlow> _oAuthFlowPatternFields =
            new PatternFieldMap<AsyncApiOAuthFlow>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiOAuthFlow LoadOAuthFlow(ParseNode node)
        {
            var mapNode = node.CheckMapNode("OAuthFlow");

            var oauthFlow = new AsyncApiOAuthFlow();
            foreach (var property in mapNode)
            {
                property.ParseField(oauthFlow, _oAuthFlowFixedFileds, _oAuthFlowPatternFields);
            }

            return oauthFlow;
        }
    }
}
