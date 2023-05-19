// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Server Bindings object.
    /// </summary>
    public class AsyncApiServerBindings : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiExtensible
    {
        
        
        // TODO: No server bindings to implement yet


        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        public bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        public AsyncApiReference Reference { get; set; }

        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize <see cref="AsyncApiServerBindings"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineLocalReferences)
            {
                Reference.SerializeAsV2(writer);
                return;
            }

            SerializeAsV2WithoutReference(writer);
        }

        /// <summary>
        /// Serialize to AsyncAPI V2 document without using reference.
        /// </summary>
        public void SerializeAsV2WithoutReference(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();

            // ws
            //writer.WriteOptionalObject(AsyncApiConstants.BindingWebSockets, BindingWebSockets, (w, s) => s.SerializeAsV2(w));
            
            // TODO: Add rest of bindings

            // extensions
            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }

    }
}
