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
        private static readonly FixedFieldMap<AsyncApiEncoding> _encodingFixedFields = new FixedFieldMap<AsyncApiEncoding>
        {
            {
                "contentType", (o, n) =>
                {
                    o.ContentType = n.GetScalarValue();
                }
            },
            {
                "headers", (o, n) =>
                {
                    o.Headers = n.CreateMap(LoadHeader);
                }
            },
            {
                "style", (o, n) =>
                {
                    ParameterStyle style;
                    if (Enum.TryParse(n.GetScalarValue(), out style))
                    {
                        o.Style = style;
                    }
                    else
                    {
                        o.Style = null;
                    }
                }
            },
            {
                "explode", (o, n) =>
                {
                    o.Explode = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "allowedReserved", (o, n) =>
                {
                    o.AllowReserved = bool.Parse(n.GetScalarValue());
                }
            },
        };

        private static readonly PatternFieldMap<AsyncApiEncoding> _encodingPatternFields =
            new PatternFieldMap<AsyncApiEncoding>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiEncoding LoadEncoding(ParseNode node)
        {
            var mapNode = node.CheckMapNode("encoding");

            var encoding = new AsyncApiEncoding();
            foreach (var property in mapNode)
            {
                property.ParseField(encoding, _encodingFixedFields, _encodingPatternFields);
            }

            return encoding;
        }
    }
}
