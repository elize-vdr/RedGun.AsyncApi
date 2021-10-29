// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using System.Linq;
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
        private static readonly FixedFieldMap<AsyncApiParameter> _parameterFixedFields =
            new FixedFieldMap<AsyncApiParameter>
            {
                {
                    "name", (o, n) =>
                    {
                        o.Name = n.GetScalarValue();
                    }
                },
                {
                    "in", (o, n) =>
                    {
                        var inString = n.GetScalarValue();

                        if ( Enum.GetValues(typeof(ParameterLocation)).Cast<ParameterLocation>()
                            .Select( e => e.GetDisplayName() )
                            .Contains(inString) )
                        {
                            o.In = n.GetScalarValue().GetEnumFromDisplayName<ParameterLocation>();
                        }
                        else
                        {
                            o.In = null;
                        }
                    }
                },
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    "required", (o, n) =>
                    {
                        o.Required = bool.Parse(n.GetScalarValue());
                    }
                },
                {
                    "deprecated", (o, n) =>
                    {
                        o.Deprecated = bool.Parse(n.GetScalarValue());
                    }
                },
                {
                    "allowEmptyValue", (o, n) =>
                    {
                        o.AllowEmptyValue = bool.Parse(n.GetScalarValue());
                    }
                },
                {
                    "allowReserved", (o, n) =>
                    {
                        o.AllowReserved = bool.Parse(n.GetScalarValue());
                    }
                },
                {
                    "style", (o, n) =>
                    {
                        o.Style = n.GetScalarValue().GetEnumFromDisplayName<ParameterStyle>();
                    }
                },
                {
                    "explode", (o, n) =>
                    {
                        o.Explode = bool.Parse(n.GetScalarValue());
                    }
                },
                {
                    "schema", (o, n) =>
                    {
                        o.Schema = LoadSchema(n);
                    }
                },
                {
                    "content", (o, n) =>
                    {
                        o.Content = n.CreateMap(LoadMediaType);
                    }
                },
                {
                    "examples", (o, n) =>
                    {
                        o.Examples = n.CreateMap(LoadExample);
                    }
                },
                {
                    "example", (o, n) =>
                    {
                        o.Example = n.CreateAny();
                    }
                },
            };

        private static readonly PatternFieldMap<AsyncApiParameter> _parameterPatternFields =
            new PatternFieldMap<AsyncApiParameter>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        private static readonly AnyFieldMap<AsyncApiParameter> _parameterAnyFields = new AnyFieldMap<AsyncApiParameter>
        {
            {
                AsyncApiConstants.Example,
                new AnyFieldMapParameter<AsyncApiParameter>(
                    s => s.Example,
                    (s, v) => s.Example = v,
                    s => s.Schema)
            }
        };

        private static readonly AnyMapFieldMap<AsyncApiParameter, AsyncApiExample> _parameterAnyMapAsyncApiExampleFields =
            new AnyMapFieldMap<AsyncApiParameter, AsyncApiExample>
        {
            {
                AsyncApiConstants.Examples,
                new AnyMapFieldMapParameter<AsyncApiParameter, AsyncApiExample>(
                    m => m.Examples,
                    e => e.Value,
                    (e, v) => e.Value = v,
                    m => m.Schema)
            }
        };

        public static AsyncApiParameter LoadParameter(ParseNode node)
        {
            var mapNode = node.CheckMapNode("parameter");

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiParameter>(ReferenceType.Parameter, pointer);
            }

            var parameter = new AsyncApiParameter();

            ParseMap(mapNode, parameter, _parameterFixedFields, _parameterPatternFields);
            ProcessAnyFields(mapNode, parameter, _parameterAnyFields);
            ProcessAnyMapFields(mapNode, parameter, _parameterAnyMapAsyncApiExampleFields);

            return parameter;
        }
    }
}
