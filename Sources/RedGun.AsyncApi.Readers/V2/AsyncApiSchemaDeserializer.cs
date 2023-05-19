// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using System.Collections.Generic;
using System.Globalization;

namespace RedGun.AsyncApi.Readers.V2
{
    /// <summary>
    /// Class containing logic to deserialize Open API V2 document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiSchema> _schemaFixedFields = new FixedFieldMap<AsyncApiSchema>
        {
            {
                AsyncApiConstants.Title, (o, n) =>
                {
                    o.Title = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.MultipleOf, (o, n) =>
                {
                    o.MultipleOf = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture); 
                }
            },
            {
                AsyncApiConstants.Maximum, (o, n) =>
                {
                    o.Maximum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.ExclusiveMaximum, (o, n) =>
                {
                    o.ExclusiveMaximum = bool.Parse(n.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.Minimum, (o, n) =>
                {
                    o.Minimum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.ExclusiveMinimum, (o, n) =>
                {
                    o.ExclusiveMinimum = bool.Parse(n.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.MaxLength, (o, n) =>
                {
                    o.MaxLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.MinLength, (o, n) =>
                {
                    o.MinLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.Pattern, (o, n) =>
                {
                    o.Pattern = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.MaxItems, (o, n) =>
                {
                    o.MaxItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.MinItems, (o, n) =>
                {
                    o.MinItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.UniqueItems, (o, n) =>
                {
                    o.UniqueItems = bool.Parse(n.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.MaxProperties, (o, n) =>
                {
                    o.MaxProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.MinProperties, (o, n) =>
                {
                    o.MinProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                AsyncApiConstants.Required, (o, n) =>
                {
                    o.Required = new HashSet<string>(n.CreateSimpleList(n2 => n2.GetScalarValue()));
                }
            },
            {
                AsyncApiConstants.Enum, (o, n) =>
                {
                    o.Enum = n.CreateListOfAny();
                }
            },
            {
                AsyncApiConstants.Type, (o, n) =>
                {
                    o.Type = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.AllOf, (o, n) =>
                {
                    o.AllOf = n.CreateList(LoadSchema);
                }
            },
            {
                AsyncApiConstants.OneOf, (o, n) =>
                {
                    o.OneOf = n.CreateList(LoadSchema);
                }
            },
            {
                AsyncApiConstants.AnyOf, (o, n) =>
                {
                    o.AnyOf = n.CreateList(LoadSchema);
                }
            },
            {
                AsyncApiConstants.Not, (o, n) =>
                {
                    o.Not = LoadSchema(n);
                }
            },
            {
                AsyncApiConstants.Items, (o, n) =>
                {
                    o.Items = LoadSchema(n);
                }
            },
            {
                AsyncApiConstants.Properties, (o, n) =>
                {
                    o.Properties = n.CreateMap(LoadSchema);
                }
            },
            {
                AsyncApiConstants.AdditionalProperties, (o, n) =>
                {
                    if (n is ValueNode)
                    {
                        o.AdditionalPropertiesAllowed = bool.Parse(n.GetScalarValue());
                    }
                    else
                    {
                        o.AdditionalProperties = LoadSchema(n);
                    }
                }
            },
            {
                AsyncApiConstants.Description, (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Format, (o, n) =>
                {
                    o.Format = n.GetScalarValue();
                }
            },
            {
                AsyncApiConstants.Default, (o, n) =>
                {
                    o.Default = n.CreateAny();
                }
            },

            {
                AsyncApiConstants.Nullable, (o, n) =>
                {
                    o.Nullable = bool.Parse(n.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.Discriminator, (o, n) =>
                {
                    o.Discriminator = LoadDiscriminator(n);
                }
            },
            {
                AsyncApiConstants.ReadOnly, (o, n) =>
                {
                    o.ReadOnly = bool.Parse(n.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.WriteOnly, (o, n) =>
                {
                    o.WriteOnly = bool.Parse(n.GetScalarValue());
                }
            },
            {
                AsyncApiConstants.Xml, (o, n) =>
                {
                    o.Xml = LoadXml(n);
                }
            },
            {
                AsyncApiConstants.ExternalDocs, (o, n) =>
                {
                    o.ExternalDocs = LoadExternalDocs(n);
                }
            },
            {
                AsyncApiConstants.Example, (o, n) =>
                {
                    o.Example = n.CreateAny();
                }
            },
            {
                AsyncApiConstants.Deprecated, (o, n) =>
                {
                    o.Deprecated = bool.Parse(n.GetScalarValue());
                }
            },
        };

        private static readonly PatternFieldMap<AsyncApiSchema> _schemaPatternFields = new PatternFieldMap<AsyncApiSchema>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        private static readonly AnyFieldMap<AsyncApiSchema> _schemaAnyFields = new AnyFieldMap<AsyncApiSchema>
        {
            {
                AsyncApiConstants.Default,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Default,
                    (s, v) => s.Default = v,
                    s => s)
            },
            {
                 AsyncApiConstants.Example,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Example,
                    (s, v) => s.Example = v,
                    s => s)
            }
        };

        private static readonly AnyListFieldMap<AsyncApiSchema> _schemaAnyListFields = new AnyListFieldMap<AsyncApiSchema>
        {
            {
                AsyncApiConstants.Enum,
                new AnyListFieldMapParameter<AsyncApiSchema>(
                    s => s.Enum,
                    (s, v) => s.Enum = v,
                    s => s)
            }
        };

        public static AsyncApiSchema LoadSchema(ParseNode node)
        {
            var mapNode = node.CheckMapNode(AsyncApiConstants.Schema);

            var pointer = mapNode.GetReferencePointer();

            if (pointer != null)
            {
                return new AsyncApiSchema()
                {
                    UnresolvedReference = true,
                    Reference = node.Context.VersionService.ConvertToAsyncApiReference(pointer, ReferenceType.Schema)
                };
            }

            var schema = new AsyncApiSchema();

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(schema, _schemaFixedFields, _schemaPatternFields);
            }

            ProcessAnyFields(mapNode, schema, _schemaAnyFields);
            ProcessAnyListFields(mapNode, schema, _schemaAnyListFields);

            return schema;
        }
    }
}
