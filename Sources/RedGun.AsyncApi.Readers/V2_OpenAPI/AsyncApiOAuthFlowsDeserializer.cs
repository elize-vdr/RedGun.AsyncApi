// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

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
        private static readonly FixedFieldMap<AsyncApiOAuthFlows> _oAuthFlowsFixedFileds =
            new FixedFieldMap<AsyncApiOAuthFlows>
            {
                {"implicit", (o, n) => o.Implicit = LoadOAuthFlow(n)},
                {"password", (o, n) => o.Password = LoadOAuthFlow(n)},
                {"clientCredentials", (o, n) => o.ClientCredentials = LoadOAuthFlow(n)},
                {"authorizationCode", (o, n) => o.AuthorizationCode = LoadOAuthFlow(n)}
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlows> _oAuthFlowsPatternFields =
            new PatternFieldMap<AsyncApiOAuthFlows>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiOAuthFlows LoadOAuthFlows(ParseNode node)
        {
            var mapNode = node.CheckMapNode("OAuthFlows");

            var oAuthFlows = new AsyncApiOAuthFlows();
            foreach (var property in mapNode)
            {
                property.ParseField(oAuthFlows, _oAuthFlowsFixedFileds, _oAuthFlowsPatternFields);
            }

            return oAuthFlows;
        }
    }
}
