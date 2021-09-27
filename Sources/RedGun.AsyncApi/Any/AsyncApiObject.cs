// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API object.
    /// </summary>
    public class AsyncApiObject : Dictionary<string, IAsyncApiAny>, IAsyncApiAny
    {
        /// <summary>
        /// Type of <see cref="IAsyncApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Object;

        /// <summary>
        /// Serialize AsyncApiObject to writer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="specVersion">Version of the AsyncAPI specification that that will be output.</param>
        public void Write(IAsyncApiWriter writer, AsyncApiSpecVersion specVersion)
        {
            writer.WriteStartObject();

            foreach (var item in this)
            {
                writer.WritePropertyName(item.Key);
                writer.WriteAny(item.Value);
            }

            writer.WriteEndObject();

        }
    }
}
