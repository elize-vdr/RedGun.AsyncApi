// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Schema Object.
    /// </summary>
    public class AsyncApiSchema : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// Follow JSON Schema definition. Short text providing information about the data.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Value MUST be a string. Multiple types via an array are not supported.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// While relying on JSON Schema's defined formats,
        /// the OAS offers a few additional predefined formats.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public decimal? Maximum { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public bool? ExclusiveMaximum { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public decimal? Minimum { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public bool? ExclusiveMinimum { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public decimal? MultipleOf { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// The default value represents what would be assumed by the consumer of the input as the value of the schema if one is not provided.
        /// Unlike JSON Schema, the value MUST conform to the defined type for the Schema Object defined at the same level.
        /// For example, if type is string, then default can be "foo" but cannot be 1.
        /// </summary>
        public IAsyncApiAny Default { get; set; }

        /// <summary>
        /// Relevant only for Schema "properties" definitions. Declares the property as "read only".
        /// This means that it MAY be sent as part of a response but SHOULD NOT be sent as part of the request.
        /// If the property is marked as readOnly being true and is in the required list,
        /// the required will take effect on the response only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Relevant only for Schema "properties" definitions. Declares the property as "write only".
        /// Therefore, it MAY be sent as part of a request but SHOULD NOT be sent as part of the response.
        /// If the property is marked as writeOnly being true and is in the required list,
        /// the required will take effect on the request only.
        /// A property MUST NOT be marked as both readOnly and writeOnly being true.
        /// Default value is false.
        /// </summary>
        public bool WriteOnly { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> AllOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> OneOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public IList<AsyncApiSchema> AnyOf { get; set; } = new List<AsyncApiSchema>();

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Inline or referenced schema MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public AsyncApiSchema Not { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public ISet<string> Required { get; set; } = new HashSet<string>();

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Value MUST be an object and not an array. Inline or referenced schema MUST be of a Schema Object
        /// and not a standard JSON Schema. items MUST be present if the type is array.
        /// </summary>
        public AsyncApiSchema Items { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public int? MaxItems { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public int? MinItems { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public bool? UniqueItems { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Property definitions MUST be a Schema Object and not a standard JSON Schema (inline or referenced).
        /// </summary>
        public IDictionary<string, AsyncApiSchema> Properties { get; set; } = new Dictionary<string, AsyncApiSchema>();

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public int? MaxProperties { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public int? MinProperties { get; set; }

        /// <summary>
        /// Indicates if the schema can contain properties other than those defined by the properties map.
        /// </summary>
        public bool AdditionalPropertiesAllowed { get; set; } = true;

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// Value can be boolean or object. Inline or referenced schema
        /// MUST be of a Schema Object and not a standard JSON Schema.
        /// </summary>
        public AsyncApiSchema AdditionalProperties { get; set; }


        /// <summary>
        /// Adds support for polymorphism. The discriminator is an object name that is used to differentiate
        /// between other schemas which may satisfy the payload description.
        /// </summary>
        public AsyncApiDiscriminator Discriminator { get; set; }

        /// <summary>
        /// A free-form property to include an example of an instance for this schema.
        /// To represent examples that cannot be naturally represented in JSON or YAML,
        /// a string value can be used to contain the example with escaping where necessary.
        /// </summary>
        public IAsyncApiAny Example { get; set; }

        /// <summary>
        /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
        /// </summary>
        public IList<IAsyncApiAny> Enum { get; set; } = new List<IAsyncApiAny>();

        /// <summary>
        /// Allows sending a null value for the defined schema. Default value is false.
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// Additional external documentation for this schema.
        /// </summary>
        public AsyncApiExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// Specifies that a schema is deprecated and SHOULD be transitioned out of usage.
        /// Default value is false.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// This MAY be used only on properties schemas. It has no effect on root schemas.
        /// Adds additional metadata to describe the XML representation of this property.
        /// </summary>
        public AsyncApiXml Xml { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Indicates object is a placeholder reference to an actual object and does not contain valid data.
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Serialize <see cref="AsyncApiSchema"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            var settings = writer.GetSettings();

            if (Reference != null)
            {
                if (settings.ReferenceInline != ReferenceInlineSetting.InlineLocalReferences)
                {
                    Reference.SerializeAsV3(writer);
                    return;
                }

                // If Loop is detected then just Serialize as a reference.
                if (!settings.LoopDetector.PushLoop<AsyncApiSchema>(this))
                {
                    settings.LoopDetector.SaveLoop(this);
                    Reference.SerializeAsV3(writer);
                    return;
                }
            }

            SerializeAsV3WithoutReference(writer);

            if (Reference != null)
            {
                settings.LoopDetector.PopLoop<AsyncApiSchema>();
            }
        }

        /// <summary>
        /// Serialize to OpenAPI V3 document without using reference.
        /// </summary>
        public void SerializeAsV3WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // title
            writer.WriteProperty(AsyncApiConstants.Title, Title);

            // multipleOf
            writer.WriteProperty(AsyncApiConstants.MultipleOf, MultipleOf);

            // maximum
            writer.WriteProperty(AsyncApiConstants.Maximum, Maximum);

            // exclusiveMaximum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMaximum, ExclusiveMaximum);

            // minimum
            writer.WriteProperty(AsyncApiConstants.Minimum, Minimum);

            // exclusiveMinimum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMinimum, ExclusiveMinimum);

            // maxLength
            writer.WriteProperty(AsyncApiConstants.MaxLength, MaxLength);

            // minLength
            writer.WriteProperty(AsyncApiConstants.MinLength, MinLength);

            // pattern
            writer.WriteProperty(AsyncApiConstants.Pattern, Pattern);

            // maxItems
            writer.WriteProperty(AsyncApiConstants.MaxItems, MaxItems);

            // minItems
            writer.WriteProperty(AsyncApiConstants.MinItems, MinItems);

            // uniqueItems
            writer.WriteProperty(AsyncApiConstants.UniqueItems, UniqueItems);

            // maxProperties
            writer.WriteProperty(AsyncApiConstants.MaxProperties, MaxProperties);

            // minProperties
            writer.WriteProperty(AsyncApiConstants.MinProperties, MinProperties);

            // required
            writer.WriteOptionalCollection(AsyncApiConstants.Required, Required, (w, s) => w.WriteValue(s));

            // enum
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, Enum, (nodeWriter, s) => nodeWriter.WriteAny(s));

            // type
            writer.WriteProperty(AsyncApiConstants.Type, Type);

            // allOf
            writer.WriteOptionalCollection(AsyncApiConstants.AllOf, AllOf, (w, s) => s.SerializeAsV3(w));

            // anyOf
            writer.WriteOptionalCollection(AsyncApiConstants.AnyOf, AnyOf, (w, s) => s.SerializeAsV3(w));

            // oneOf
            writer.WriteOptionalCollection(AsyncApiConstants.OneOf, OneOf, (w, s) => s.SerializeAsV3(w));

            // not
            writer.WriteOptionalObject(AsyncApiConstants.Not, Not, (w, s) => s.SerializeAsV3(w));

            // items
            writer.WriteOptionalObject(AsyncApiConstants.Items, Items, (w, s) => s.SerializeAsV3(w));

            // properties
            writer.WriteOptionalMap(AsyncApiConstants.Properties, Properties, (w, s) => s.SerializeAsV3(w));

            // additionalProperties
            if (AdditionalPropertiesAllowed)
            {
                writer.WriteOptionalObject(
                    AsyncApiConstants.AdditionalProperties,
                    AdditionalProperties,
                    (w, s) => s.SerializeAsV3(w));
            }
            else
            {
                writer.WriteProperty(AsyncApiConstants.AdditionalProperties, AdditionalPropertiesAllowed);
            }

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // format
            writer.WriteProperty(AsyncApiConstants.Format, Format);

            // default
            writer.WriteOptionalObject(AsyncApiConstants.Default, Default, (w, d) => w.WriteAny(d));

            // nullable
            writer.WriteProperty(AsyncApiConstants.Nullable, Nullable, false);

            // discriminator
            writer.WriteOptionalObject(AsyncApiConstants.Discriminator, Discriminator, (w, s) => s.SerializeAsV3(w));

            // readOnly
            writer.WriteProperty(AsyncApiConstants.ReadOnly, ReadOnly, false);

            // writeOnly
            writer.WriteProperty(AsyncApiConstants.WriteOnly, WriteOnly, false);

            // xml
            writer.WriteOptionalObject(AsyncApiConstants.Xml, Xml, (w, s) => s.SerializeAsV2(w));

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, s) => s.SerializeAsV3(w));

            // example
            writer.WriteOptionalObject(AsyncApiConstants.Example, Example, (w, e) => w.WriteAny(e));

            // deprecated
            writer.WriteProperty(AsyncApiConstants.Deprecated, Deprecated, false);

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiSchema"/> to Async API v2.0
        /// </summary>
        // TODO: Remove
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            SerializeAsV2(writer: writer, parentRequiredProperties: new HashSet<string>(), propertyName: null);
        }

        /// <summary>
        /// Serialize to OpenAPI V2 document without using reference.
        /// </summary>
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            SerializeAsV2WithoutReference(
                writer: writer,
                parentRequiredProperties: new HashSet<string>(),
                propertyName: null);
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiSchema"/> to Async API v2.0 and handles not marking the provided property 
        /// as readonly if its included in the provided list of required properties of parent schema.
        /// </summary>
        /// <param name="writer">The Async API writer.</param>
        /// <param name="parentRequiredProperties">The list of required properties in parent schema.</param>
        /// <param name="propertyName">The property name that will be serialized.</param>
        // TODO: Remove
        internal void SerializeAsV2(
            IAsyncApiWriter writer,
            ISet<string> parentRequiredProperties,
            string propertyName)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null)
            {
                var settings = writer.GetSettings();
                if (settings.ReferenceInline != ReferenceInlineSetting.InlineLocalReferences)
                {
                    Reference.SerializeAsV2(writer);
                    return;
                }

                // If Loop is detected then just Serialize as a reference.
                if (!settings.LoopDetector.PushLoop<AsyncApiSchema>(this))
                {
                    settings.LoopDetector.SaveLoop(this);
                    Reference.SerializeAsV2(writer);
                    return;
                }
            }


            if (parentRequiredProperties == null)
            {
                parentRequiredProperties = new HashSet<string>();
            }

            SerializeAsV2WithoutReference(writer, parentRequiredProperties, propertyName);
        }

        /// <summary>
        /// Serialize to OpenAPI V2 document without using reference and handles not marking the provided property 
        /// as readonly if its included in the provided list of required properties of parent schema.
        /// </summary>
        /// <param name="writer">The Async API writer.</param>
        /// <param name="parentRequiredProperties">The list of required properties in parent schema.</param>
        /// <param name="propertyName">The property name that will be serialized.</param>
        // TODO: Remove
        internal void SerializeAsV2WithoutReference(
            IAsyncApiWriter writer,
            ISet<string> parentRequiredProperties,
            string propertyName)
        {
            writer.WriteStartObject();
            WriteAsSchemaProperties(writer, parentRequiredProperties, propertyName);
            writer.WriteEndObject();
        }

        internal void WriteAsItemsProperties(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            // type
            writer.WriteProperty(AsyncApiConstants.Type, Type);

            // format
            writer.WriteProperty(AsyncApiConstants.Format, Format);

            // items
            writer.WriteOptionalObject(AsyncApiConstants.Items, Items, (w, s) => s.SerializeAsV2(w));

            // collectionFormat
            // We need information from style in parameter to populate this.
            // The best effort we can make is to pull this information from the first parameter
            // that leverages this schema. However, that in itself may not be as simple
            // as the schema directly under parameter might be referencing one in the Components,
            // so we will need to do a full scan of the object before we can write the value for
            // this property. This is not supported yet, so we will skip this property at the moment.

            // default
            writer.WriteOptionalObject(AsyncApiConstants.Default, Default, (w, d) => w.WriteAny(d));

            // maximum
            writer.WriteProperty(AsyncApiConstants.Maximum, Maximum);

            // exclusiveMaximum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMaximum, ExclusiveMaximum);

            // minimum
            writer.WriteProperty(AsyncApiConstants.Minimum, Minimum);

            // exclusiveMinimum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMinimum, ExclusiveMinimum);

            // maxLength
            writer.WriteProperty(AsyncApiConstants.MaxLength, MaxLength);

            // minLength
            writer.WriteProperty(AsyncApiConstants.MinLength, MinLength);

            // pattern
            writer.WriteProperty(AsyncApiConstants.Pattern, Pattern);

            // maxItems
            writer.WriteProperty(AsyncApiConstants.MaxItems, MaxItems);

            // minItems
            writer.WriteProperty(AsyncApiConstants.MinItems, MinItems);

            // enum
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, Enum, (w, s) => w.WriteAny(s));

            // multipleOf
            writer.WriteProperty(AsyncApiConstants.MultipleOf, MultipleOf);

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.OpenApi2_0);
        }

        // TODO: Remove
        internal void WriteAsSchemaProperties(
            IAsyncApiWriter writer,
            ISet<string> parentRequiredProperties,
            string propertyName)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            // format
            writer.WriteProperty(AsyncApiConstants.Format, Format);

            // title
            writer.WriteProperty(AsyncApiConstants.Title, Title);

            // description
            writer.WriteProperty(AsyncApiConstants.Description, Description);

            // default
            writer.WriteOptionalObject(AsyncApiConstants.Default, Default, (w, d) => w.WriteAny(d));

            // multipleOf
            writer.WriteProperty(AsyncApiConstants.MultipleOf, MultipleOf);

            // maximum
            writer.WriteProperty(AsyncApiConstants.Maximum, Maximum);

            // exclusiveMaximum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMaximum, ExclusiveMaximum);

            // minimum
            writer.WriteProperty(AsyncApiConstants.Minimum, Minimum);

            // exclusiveMinimum
            writer.WriteProperty(AsyncApiConstants.ExclusiveMinimum, ExclusiveMinimum);

            // maxLength
            writer.WriteProperty(AsyncApiConstants.MaxLength, MaxLength);

            // minLength
            writer.WriteProperty(AsyncApiConstants.MinLength, MinLength);

            // pattern
            writer.WriteProperty(AsyncApiConstants.Pattern, Pattern);

            // maxItems
            writer.WriteProperty(AsyncApiConstants.MaxItems, MaxItems);

            // minItems
            writer.WriteProperty(AsyncApiConstants.MinItems, MinItems);

            // uniqueItems
            writer.WriteProperty(AsyncApiConstants.UniqueItems, UniqueItems);

            // maxProperties
            writer.WriteProperty(AsyncApiConstants.MaxProperties, MaxProperties);

            // minProperties
            writer.WriteProperty(AsyncApiConstants.MinProperties, MinProperties);

            // required
            writer.WriteOptionalCollection(AsyncApiConstants.Required, Required, (w, s) => w.WriteValue(s));

            // enum
            writer.WriteOptionalCollection(AsyncApiConstants.Enum, Enum, (w, s) => w.WriteAny(s));

            // type
            writer.WriteProperty(AsyncApiConstants.Type, Type);

            // items
            writer.WriteOptionalObject(AsyncApiConstants.Items, Items, (w, s) => s.SerializeAsV2(w));

            // allOf
            writer.WriteOptionalCollection(AsyncApiConstants.AllOf, AllOf, (w, s) => s.SerializeAsV2(w));

            // properties
            writer.WriteOptionalMap(AsyncApiConstants.Properties, Properties, (w, key, s) =>
                s.SerializeAsV2(w, Required, key));

            // additionalProperties
            if (AdditionalPropertiesAllowed)
            {
                writer.WriteOptionalObject(
                    AsyncApiConstants.AdditionalProperties,
                    AdditionalProperties,
                    (w, s) => s.SerializeAsV2(w));
            }
            else
            {
                writer.WriteProperty(AsyncApiConstants.AdditionalProperties, AdditionalPropertiesAllowed);
            }

            // discriminator
            writer.WriteProperty(AsyncApiConstants.Discriminator, Discriminator?.PropertyName);

            // readOnly
            // In V2 schema if a property is part of required properties of parent schema,
            // it cannot be marked as readonly.
            if (!parentRequiredProperties.Contains(propertyName))
            {
                writer.WriteProperty(name: AsyncApiConstants.ReadOnly, value: ReadOnly, defaultValue: false);
            }

            // xml
            writer.WriteOptionalObject(AsyncApiConstants.Xml, Xml, (w, s) => s.SerializeAsV2(w));

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, s) => s.SerializeAsV2(w));

            // example
            writer.WriteOptionalObject(AsyncApiConstants.Example, Example, (w, e) => w.WriteAny(e));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.OpenApi2_0);
        }
    }
}
