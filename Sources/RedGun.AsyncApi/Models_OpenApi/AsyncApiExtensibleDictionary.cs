// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Generic dictionary type for Async API dictionary element.
    /// </summary>
    /// <typeparam name="T">The Async API element, <see cref="IAsyncApiElement"/></typeparam>
    public abstract class AsyncApiExtensibleDictionary<T> : Dictionary<string, T>,
        IAsyncApiSerializable,
        IAsyncApiExtensible
        where T : IAsyncApiSerializable
    {
        /// <summary>
        /// This object MAY be extended with Specification Extensions.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <summary>
        /// Serialize to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            foreach (var item in this)
            {
                writer.WriteRequiredObject(item.Key, item.Value, (w, p) => p.SerializeAsV2(w));
            }

            writer.WriteExtensions(Extensions, AsyncApiSpecVersion.AsyncApi2_0);

            writer.WriteEndObject();
        }
    }
}
