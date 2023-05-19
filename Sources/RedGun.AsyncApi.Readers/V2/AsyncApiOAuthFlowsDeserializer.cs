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
        private static readonly FixedFieldMap<AsyncApiOAuthFlows> _oAuthFlowsFixedFields =
            new FixedFieldMap<AsyncApiOAuthFlows>
            {
                {AsyncApiConstants.Implicit, (o, n) =>
                    {
                        o.Implicit = LoadOAuthFlow(n);
                    }
                },
                {AsyncApiConstants.Password, (o, n) =>
                    {
                        o.Password = LoadOAuthFlow(n);
                    }
                },
                {AsyncApiConstants.ClientCredentials, (o, n) =>
                    {
                        o.ClientCredentials = LoadOAuthFlow(n);
                    }
                },
                {AsyncApiConstants.AuthorizationCode, (o, n) =>
                    {
                        o.AuthorizationCode = LoadOAuthFlow(n);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlows> _oAuthFlowsPatternFields =
            new PatternFieldMap<AsyncApiOAuthFlows>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiOAuthFlows LoadOAuthFlows(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Flows);

            var oAuthFlows = new AsyncApiOAuthFlows();
            foreach (var property in mapNode)
            {
                property.ParseField(oAuthFlows, _oAuthFlowsFixedFields, _oAuthFlowsPatternFields);
            }

            return oAuthFlows;
        }
    }
}
