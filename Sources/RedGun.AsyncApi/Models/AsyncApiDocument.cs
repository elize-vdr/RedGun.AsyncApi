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
        /// REQUIRED. Provides metadata about the API. The metadata MAY be used by tooling as required.
        /// </summary>
        public AsyncApiInfo Info { get; set; }

        /// <summary>
        /// An array of Server Objects, which provide connectivity information to a target server.
        /// </summary>
        public IList<AsyncApiServer> Servers { get; set; } = new List<AsyncApiServer>();

        /// <summary>
        /// REQUIRED. The available paths and operations for the API.
        /// </summary>
        public AsyncApiPaths Paths { get; set; }

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        public AsyncApiComponents Components { get; set; }

        /// <summary>
        /// A declaration of which security mechanisms can be used across the API.
        /// </summary>
        public IList<AsyncApiSecurityRequirement> SecurityRequirements { get; set; } =
            new List<AsyncApiSecurityRequirement>();

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
        /// Serialize <see cref="AsyncApiDocument"/> to the latest patch of AsyncAPI object V3.0.
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // TODO: Version????
            // asyncapi
            writer.WriteProperty(AsyncApiConstants.AsyncApi, "3.0.1");

            // info
            writer.WriteRequiredObject(AsyncApiConstants.Info, Info, (w, i) => i.SerializeAsV3(w));

            // servers
            writer.WriteOptionalCollection(AsyncApiConstants.Servers, Servers, (w, s) => s.SerializeAsV3(w));

            // paths
            writer.WriteRequiredObject(AsyncApiConstants.Paths, Paths, (w, p) => p.SerializeAsV3(w));

            // components
            writer.WriteOptionalObject(AsyncApiConstants.Components, Components, (w, c) => c.SerializeAsV3(w));

            // security
            writer.WriteOptionalCollection(
                AsyncApiConstants.Security,
                SecurityRequirements,
                (w, s) => s.SerializeAsV3(w));

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, Tags, (w, t) => t.SerializeAsV3WithoutReference(w));

            // external docs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, e) => e.SerializeAsV3(w));

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

        
        /// <summary>
        /// Serialize <see cref="AsyncApiDocument"/> to OpenAPI object V2.0.
        /// </summary>
        // TODO: Remove??
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // swagger
            writer.WriteProperty(AsyncApiConstants.Swagger, "2.0");

            // info
            writer.WriteRequiredObject(AsyncApiConstants.Info, Info, (w, i) => i.SerializeAsV2(w));

            // host, basePath, schemes, consumes, produces
            WriteHostInfoV2(writer, Servers);

            // paths
            writer.WriteRequiredObject(AsyncApiConstants.Paths, Paths, (w, p) => p.SerializeAsV2(w));

            // If references have been inlined we don't need the to render the components section
            // however if they have cycles, then we will need a component rendered
            if (writer.GetSettings().ReferenceInline != ReferenceInlineSetting.DoNotInlineReferences)
            {
                var loops = writer.GetSettings().LoopDetector.Loops;

                if (loops.TryGetValue(typeof(AsyncApiSchema), out List<object> schemas))
                {
                    var asyncApiSchemas = schemas.Cast<AsyncApiSchema>().Distinct().ToList()
                        .ToDictionary<AsyncApiSchema, string>(k => k.Reference.Id);

                    foreach (var schema in asyncApiSchemas.Values.ToList())
                    {
                        FindSchemaReferences.ResolveSchemas(Components, asyncApiSchemas);
                    }

                    writer.WriteOptionalMap(
                       AsyncApiConstants.Definitions,
                       asyncApiSchemas,
                       (w, key, component) =>
                       {
                           component.SerializeAsV2WithoutReference(w);
                       });
                }
            }
            else
            {
                // Serialize each referenceable object as full object without reference if the reference in the object points to itself. 
                // If the reference exists but points to other objects, the object is serialized to just that reference.
                // definitions
                writer.WriteOptionalMap(
                    AsyncApiConstants.Definitions,
                    Components?.Schemas,
                    (w, key, component) =>
                    {
                        if (component.Reference != null &&
                            component.Reference.Type == ReferenceType.Schema &&
                            component.Reference.Id == key)
                        {
                            component.SerializeAsV2WithoutReference(w);
                        }
                        else
                        {
                            component.SerializeAsV2(w);
                        }
                    });
            }
            // parameters
            writer.WriteOptionalMap(
                AsyncApiConstants.Parameters,
                Components?.Parameters,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Parameter &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // responses
            writer.WriteOptionalMap(
                AsyncApiConstants.Responses,
                Components?.Responses,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Response &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // securityDefinitions
            writer.WriteOptionalMap(
                AsyncApiConstants.SecurityDefinitions,
                Components?.SecuritySchemes,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.SecurityScheme &&
                        component.Reference.Id == key)
                    {
                        component.SerializeAsV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeAsV2(w);
                    }
                });

            // security
            writer.WriteOptionalCollection(
                AsyncApiConstants.Security,
                SecurityRequirements,
                (w, s) => s.SerializeAsV2(w));

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, Tags, (w, t) => t.SerializeAsV2WithoutReference(w));

            // externalDocs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, ExternalDocs, (w, e) => e.SerializeAsV2(w));

            // extensions
            // TODO: Remove
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.OpenApi2_0);

            writer.WriteEndObject();
        }

        // TODO: Remove???
        private static void WriteHostInfoV2(IAsyncApiWriter writer, IList<AsyncApiServer> servers)
        {
            if (servers == null || !servers.Any())
            {
                return;
            }

            // Arbitrarily choose the first server given that V2 only allows 
            // one host, port, and base path.
            var firstServer = servers.First();

            // Divide the URL in the Url property into host and basePath required in OpenAPI V2
            // The Url property cannotcontain path templating to be valid for V2 serialization.
            var firstServerUrl = new Uri(firstServer.Url, UriKind.RelativeOrAbsolute);

            // host
            if (firstServerUrl.IsAbsoluteUri)
            {
                writer.WriteProperty(
                    AsyncApiConstants.Host,
                    firstServerUrl.GetComponents(UriComponents.Host | UriComponents.Port, UriFormat.SafeUnescaped));
                
                // basePath
                if (firstServerUrl.AbsolutePath != "/")
                {
                    writer.WriteProperty(AsyncApiConstants.BasePath, firstServerUrl.AbsolutePath);
                }
            } else
            {
                var relativeUrl = firstServerUrl.OriginalString;
                if (relativeUrl.StartsWith("//"))
                {
                    var pathPosition = relativeUrl.IndexOf('/', 3);
                    writer.WriteProperty(AsyncApiConstants.Host, relativeUrl.Substring(0, pathPosition));
                    relativeUrl = relativeUrl.Substring(pathPosition);
                }
                if (!String.IsNullOrEmpty(relativeUrl) && relativeUrl != "/")
                {
                    writer.WriteProperty(AsyncApiConstants.BasePath, relativeUrl);
                }
            }

            // Consider all schemes of the URLs in the server list that have the same
            // host, port, and base path as the first server.
            var schemes = servers.Select(
                    s =>
                    {
                        Uri.TryCreate(s.Url, UriKind.RelativeOrAbsolute, out var url);
                        return url;
                    })
                .Where(
                    u => Uri.Compare(
                            u,
                            firstServerUrl,
                            UriComponents.Host | UriComponents.Port | UriComponents.Path,
                            UriFormat.SafeUnescaped,
                            StringComparison.OrdinalIgnoreCase) ==
                        0 && u.IsAbsoluteUri)
                .Select(u => u.Scheme)
                .Distinct()
                .ToList();

            // schemes
            writer.WriteOptionalCollection(AsyncApiConstants.Schemes, schemes, (w, s) => w.WriteValue(s));
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

                    case ReferenceType.Response:
                        return this.Components.Responses[reference.Id];

                    case ReferenceType.Parameter:
                        return this.Components.Parameters[reference.Id];

                    case ReferenceType.Example:
                        return this.Components.Examples[reference.Id];

                    case ReferenceType.RequestBody:
                        return this.Components.RequestBodies[reference.Id];

                    case ReferenceType.Header:
                        return this.Components.Headers[reference.Id];

                    case ReferenceType.SecurityScheme:
                        return this.Components.SecuritySchemes[reference.Id];

                    case ReferenceType.Link:
                        return this.Components.Links[reference.Id];

                    case ReferenceType.Callback:
                        return this.Components.Callbacks[reference.Id];

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
