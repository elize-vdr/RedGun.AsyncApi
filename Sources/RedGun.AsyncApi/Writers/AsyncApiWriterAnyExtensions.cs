// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;

namespace RedGun.AsyncApi.Writers
{
    /// <summary>
    /// Extensions methods for writing the <see cref="IAsyncApiAny"/>
    /// </summary>
    public static class AsyncApiWriterAnyExtensions
    {
        /// <summary>
        /// Write the specification extensions
        /// </summary>
        /// <param name="writer">The Async API writer.</param>
        /// <param name="extensions">The specification extensions.</param>
        /// <param name="specVersion">Version of the AsyncAPI specification that that will be output.</param>
        public static void WriteExtensions(this IAsyncApiWriter writer, IDictionary<string, IAsyncApiExtension> extensions, AsyncApiSpecVersion specVersion)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (extensions != null)
            {
                foreach (var item in extensions)
                {
                    writer.WritePropertyName(item.Key);
                    item.Value.Write(writer, specVersion);
                }
            }
        }

        /// <summary>
        /// Write the <see cref="IAsyncApiAny"/> value.
        /// </summary>
        /// <typeparam name="T">The Async API Any type.</typeparam>
        /// <param name="writer">The Async API writer.</param>
        /// <param name="any">The Any value</param>
        public static void WriteAny<T>(this IAsyncApiWriter writer, T any) where T : IAsyncApiAny
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (any == null)
            {
                writer.WriteNull();
                return;
            }

            switch (any.AnyType)
            {
                case AnyType.Array: // Array
                    writer.WriteArray(any as AsyncApiArray);
                    break;

                case AnyType.Object: // Object
                    writer.WriteObject(any as AsyncApiObject);
                    break;

                case AnyType.Primitive: // Primitive
                    writer.WritePrimitive(any as IAsyncApiPrimitive);
                    break;

                case AnyType.Null: // null
                    writer.WriteNull();
                    break;

                default:
                    break;
            }
        }

        private static void WriteArray(this IAsyncApiWriter writer, AsyncApiArray array)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (array == null)
            {
                throw Error.ArgumentNull(nameof(array));
            }

            writer.WriteStartArray();

            foreach (var item in array)
            {
                writer.WriteAny(item);
            }

            writer.WriteEndArray();
        }

        private static void WriteObject(this IAsyncApiWriter writer, AsyncApiObject entity)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (entity == null)
            {
                throw Error.ArgumentNull(nameof(entity));
            }

            writer.WriteStartObject();

            foreach (var item in entity)
            {
                writer.WritePropertyName(item.Key);
                writer.WriteAny(item.Value);
            }

            writer.WriteEndObject();
        }

        private static void WritePrimitive(this IAsyncApiWriter writer, IAsyncApiPrimitive primitive)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (primitive == null)
            {
                throw Error.ArgumentNull(nameof(primitive));
            }

            // The Spec version is meaning for the Any type, so it's ok to use the latest one.
            primitive.Write(writer, AsyncApiSpecVersion.AsyncApi2_0);
        }
    }
}
