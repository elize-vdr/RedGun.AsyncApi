﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Extensions;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;
using System.Collections.Generic;
using System.Globalization;

namespace RedGun.AsyncApi.Readers.V3
{
    /// <summary>
    /// Class containing logic to deserialize Async API V3 document into
    /// runtime Async API object model.
    /// </summary>
    internal static partial class AsyncApiV3Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiSchema> _schemaFixedFields = new FixedFieldMap<AsyncApiSchema>
        {
            {
                "title", (o, n) =>
                {
                    o.Title = n.GetScalarValue();
                }
            },
            {
                "multipleOf", (o, n) =>
                {
                    o.MultipleOf = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture); 
                }
            },
            {
                "maximum", (o, n) =>
                {
                    o.Maximum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMaximum", (o, n) =>
                {
                    o.ExclusiveMaximum = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "minimum", (o, n) =>
                {
                    o.Minimum = decimal.Parse(n.GetScalarValue(), NumberStyles.Float, CultureInfo.InvariantCulture);
                }
            },
            {
                "exclusiveMinimum", (o, n) =>
                {
                    o.ExclusiveMinimum = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "maxLength", (o, n) =>
                {
                    o.MaxLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "minLength", (o, n) =>
                {
                    o.MinLength = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "pattern", (o, n) =>
                {
                    o.Pattern = n.GetScalarValue();
                }
            },
            {
                "maxItems", (o, n) =>
                {
                    o.MaxItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "minItems", (o, n) =>
                {
                    o.MinItems = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "uniqueItems", (o, n) =>
                {
                    o.UniqueItems = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "maxProperties", (o, n) =>
                {
                    o.MaxProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "minProperties", (o, n) =>
                {
                    o.MinProperties = int.Parse(n.GetScalarValue(), CultureInfo.InvariantCulture);
                }
            },
            {
                "required", (o, n) =>
                {
                    o.Required = new HashSet<string>(n.CreateSimpleList(n2 => n2.GetScalarValue()));
                }
            },
            {
                "enum", (o, n) =>
                {
                    o.Enum = n.CreateListOfAny();
                }
            },
            {
                "type", (o, n) =>
                {
                    o.Type = n.GetScalarValue();
                }
            },
            {
                "allOf", (o, n) =>
                {
                    o.AllOf = n.CreateList(LoadSchema);
                }
            },
            {
                "oneOf", (o, n) =>
                {
                    o.OneOf = n.CreateList(LoadSchema);
                }
            },
            {
                "anyOf", (o, n) =>
                {
                    o.AnyOf = n.CreateList(LoadSchema);
                }
            },
            {
                "not", (o, n) =>
                {
                    o.Not = LoadSchema(n);
                }
            },
            {
                "items", (o, n) =>
                {
                    o.Items = LoadSchema(n);
                }
            },
            {
                "properties", (o, n) =>
                {
                    o.Properties = n.CreateMap(LoadSchema);
                }
            },
            {
                "additionalProperties", (o, n) =>
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
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "format", (o, n) =>
                {
                    o.Format = n.GetScalarValue();
                }
            },
            {
                "default", (o, n) =>
                {
                    o.Default = n.CreateAny();
                }
            },

            {
                "nullable", (o, n) =>
                {
                    o.Nullable = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "discriminator", (o, n) =>
                {
                    o.Discriminator = LoadDiscriminator(n);
                }
            },
            {
                "readOnly", (o, n) =>
                {
                    o.ReadOnly = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "writeOnly", (o, n) =>
                {
                    o.WriteOnly = bool.Parse(n.GetScalarValue());
                }
            },
            {
                "xml", (o, n) =>
                {
                    o.Xml = LoadXml(n);
                }
            },
            {
                "externalDocs", (o, n) =>
                {
                    o.ExternalDocs = LoadExternalDocs(n);
                }
            },
            {
                "example", (o, n) =>
                {
                    o.Example = n.CreateAny();
                }
            },
            {
                "deprecated", (o, n) =>
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
                OpenApiConstants.Default,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Default,
                    (s, v) => s.Default = v,
                    s => s)
            },
            {
                 OpenApiConstants.Example,
                new AnyFieldMapParameter<AsyncApiSchema>(
                    s => s.Example,
                    (s, v) => s.Example = v,
                    s => s)
            }
        };

        private static readonly AnyListFieldMap<AsyncApiSchema> _schemaAnyListFields = new AnyListFieldMap<AsyncApiSchema>
        {
            {
                OpenApiConstants.Enum,
                new AnyListFieldMapParameter<AsyncApiSchema>(
                    s => s.Enum,
                    (s, v) => s.Enum = v,
                    s => s)
            }
        };

        public static AsyncApiSchema LoadSchema(ParseNode node)
        {
            var mapNode = node.CheckMapNode(OpenApiConstants.Schema);

            var pointer = mapNode.GetReferencePointer();

            if (pointer != null)
            {
                return new AsyncApiSchema()
                {
                    UnresolvedReference = true,
                    Reference = node.Context.VersionService.ConvertToOpenApiReference(pointer, ReferenceType.Schema)
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
