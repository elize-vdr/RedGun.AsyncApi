// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Services;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Describes an AsyncAPI object (AsyncAPI document). See: https://www.asyncapi.com/
    /// </summary>
    public class AsyncApiDocument : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Related workspace containing AsyncApiDocuments that are referenced in this document
        /// </summary>
        public AsyncApiWorkspace Workspace { get; set; }
        
        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// REQUIRED. Provides metadata about the API. The metadata MAY be used by tooling as required.
        /// </summary>
        public AsyncApiInfo Info { get; set; }

        /// <summary>
        /// Default content type to use when encoding/decoding a message's payload.
        /// </summary>
        public string DefaultContentType { get; set; }

        /// <summary>
        /// An array of Server Objects, which provide connectivity information to a target server.
        /// </summary>
        public AsyncApiServers Servers { get; set; } = new AsyncApiServers();

        /// <summary>
        /// REQUIRED. The available channels and messages for the API.
        /// </summary>
        public AsyncApiChannels Channels { get; set; }

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        public AsyncApiComponents Components { get; set; }

        /// <summary>
        /// A list of tags used by the specification with additional metadata.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        public AsyncApiExternalDocs ExternalDocs { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiDocument"/> to the latest patch of AsyncAPI object V2.0.
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // TODO: Why is version hard-coded here?
            writer.WriteProperty(AsyncApiConstants.AsyncApi, "2.2.0");

            // info
            writer.WriteRequiredObject(AsyncApiConstants.Info, Info, (w, i) => i.SerializeAsV2(w));
            
            // id
            writer.WriteProperty(AsyncApiConstants.Id, Id);

            // servers
            writer.WriteOptionalObject(AsyncApiConstants.Servers, Servers, (w, s) => s.SerializeAsV2(w));
            
            // defaultContentType
            writer.WriteProperty(AsyncApiConstants.Default, DefaultContentType);

            // paths
            writer.WriteRequiredObject(AsyncApiConstants.Channels, Channels, (w, p) => p.SerializeAsV2(w));

            // components
            writer.WriteOptionalObject(AsyncApiConstants.Components, Components, (w, c) => c.SerializeAsV2(w));

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, Tags, (w, t) => t.SerializeAsV2WithoutReference(w));

            // external docs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, e) => e.SerializeAsV2(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Walk the AsyncApiDocument and resolve unresolved references
        /// </summary>
        /// <param name="useExternal">Indicates if external references should be resolved.  Document needs to reference a workspace for this to be possible.</param>
        public IEnumerable<AsyncApiError> ResolveReferences(bool useExternal = false)
        {
            var resolver = new AsyncApiReferenceResolver(this, useExternal);
            var walker = new AsyncApiWalker(resolver);
            walker.Walk(this);
            return resolver.Errors;
        }

        /// <summary>
        /// Load the referenced <see cref="IAsyncApiReferenceable"/> object from a <see cref="AsyncApiReference"/> object
        /// </summary>
        public IAsyncApiReferenceable ResolveReference(AsyncApiReference reference)
        {
            return ResolveReference(reference, false);
        }

        /// <summary>
        /// Load the referenced <see cref="IAsyncApiReferenceable"/> object from a <see cref="AsyncApiReference"/> object
        /// </summary>
        public IAsyncApiReferenceable ResolveReference(AsyncApiReference reference, bool useExternal)
        {
            if (reference == null)
            {
                return null;
            }

            // Todo: Verify if we need to check to see if this external reference is actually targeted at this document.
            if (useExternal)
            {
                if (this.Workspace == null)
                {
                    throw new ArgumentException(Properties.SRResource.WorkspaceRequiredForExternalReferenceResolution);
                }
                return this.Workspace.ResolveReference(reference);
            } 

            if (!reference.Type.HasValue)
            {
                throw new ArgumentException(Properties.SRResource.LocalReferenceRequiresType);
            }

            // Special case for Tag
            if (reference.Type == ReferenceType.Tag)
            {
                foreach (var tag in this.Tags)
                {
                    if (tag.Name == reference.Id)
                    {
                        tag.Reference = reference;
                        return tag;
                    }
                }

                return null;
            }

            if (this.Components == null)
            {
                throw new AsyncApiException(string.Format(Properties.SRResource.InvalidReferenceId, reference.Id));
            }

            try
            {
                switch (reference.Type)
                {
                    case ReferenceType.Schema:
                        return this.Components.Schemas[reference.Id];

                    case ReferenceType.Message:
                        return this.Components.Messages[reference.Id];

                    case ReferenceType.SecurityScheme:
                        return this.Components.SecuritySchemes[reference.Id];

                    case ReferenceType.Parameter:
                        return this.Components.Parameters[reference.Id];

                    case ReferenceType.CorrelationId:
                        return this.Components.CorrelationIds[reference.Id];

                    case ReferenceType.OperationTrait:
                        return this.Components.OperationTraits[reference.Id];

                    case ReferenceType.MessageTrait:
                        return this.Components.MessageTraits[reference.Id];

                    case ReferenceType.ServerBindings:
                        return this.Components.ServerBindings[reference.Id];

                    case ReferenceType.ChannelBindings:
                        return this.Components.ChannelBindings[reference.Id];

                    case ReferenceType.OperationBindings:
                        return this.Components.OperationBindings[reference.Id];

                    case ReferenceType.MessageBindings:
                        return this.Components.MessageBindings[reference.Id];

                    default:
                        throw new AsyncApiException(Properties.SRResource.InvalidReferenceType);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new AsyncApiException(string.Format(Properties.SRResource.InvalidReferenceId, reference.Id));
            }
        }
    }

    internal class FindSchemaReferences : AsyncApiVisitorBase
    {
        private Dictionary<string, AsyncApiSchema> Schemas;

        public static void ResolveSchemas(AsyncApiComponents components, Dictionary<string, AsyncApiSchema> schemas )
        {
            var visitor = new FindSchemaReferences();
            visitor.Schemas = schemas;
            var walker = new AsyncApiWalker(visitor);
            walker.Walk(components);
        }

        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            switch (referenceable)
            {
                case AsyncApiSchema schema:
                    if (!Schemas.ContainsKey(schema.Reference.Id))
                    {
                        Schemas.Add(schema.Reference.Id, schema);
                    }
                    break;

                default:
                    break;
            }
            base.Visit(referenceable);
        }

        public override void Visit(AsyncApiSchema schema)
        {
            // This is needed to handle schemas used in Responses in components
            if (schema.Reference != null)
            {
                if (!Schemas.ContainsKey(schema.Reference.Id))
                {
                    Schemas.Add(schema.Reference.Id, schema);
                }
            }
            base.Visit(schema);
        }
    }
}
