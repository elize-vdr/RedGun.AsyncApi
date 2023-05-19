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
        private static readonly FixedFieldMap<AsyncApiServer> _serverFixedFields = new FixedFieldMap<AsyncApiServer>
        {
            {
                AsyncApiConstants.Url, (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Protocol, (o, n) =>
                {
                    o.Protocol = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.ProtocolVersion, (o, n) =>
                {
                    o.ProtocolVersion = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Variables, (o, n) =>
                {
                    o.Variables = n.CreateMap(LoadServerVariable);
                }
            },
            {
                AsyncApiConstants.Security, (o, n) =>
                {
                    o.SecurityRequirements = n.CreateList(LoadSecurityRequirement);
                }
            },
            {
                AsyncApiConstants.Bindings, (o, n) =>
                {
                    o.Bindings = LoadServerBindings(n);
                }
            }
        };

        private static readonly PatternFieldMap<AsyncApiServer> _serverPatternFields = new PatternFieldMap<AsyncApiServer>
        {
            {
                s => s.StartsWith("x-"),
                (o, p, n) =>
                    o.AddExtension(p,
                        LoadExtension(p, n))
            }
        };

        public static AsyncApiServer LoadServer(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Server);

            var server = new AsyncApiServer();

            ParseMap(mapNode, server, _serverFixedFields, _serverPatternFields);

            return server;
        }
    }
}
