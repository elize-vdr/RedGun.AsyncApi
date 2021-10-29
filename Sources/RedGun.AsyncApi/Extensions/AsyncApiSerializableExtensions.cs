// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Globalization;
using System.IO;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Properties;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IAsyncApiSerializable"/> serialization.
    /// </summary>
    public static class AsyncApiSerializableExtensions
    {
        /// <summary>
        /// Serialize the <see cref="IAsyncApiSerializable"/> to the Async API document (JSON) using the given stream and specification version.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        public static void SerializeAsJson<T>(this T element, Stream stream, AsyncApiSpecVersion specVersion)
            where T : IAsyncApiSerializable
        {
            element.Serialize(stream, specVersion, AsyncApiFormat.Json);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the Async API document (YAML) using the given stream and specification version.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        public static void SerializeAsYaml<T>(this T element, Stream stream, AsyncApiSpecVersion specVersion)
            where T : IAsyncApiSerializable
        {
            element.Serialize(stream, specVersion, AsyncApiFormat.Yaml);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the Async API document using
        /// the given stream, specification version and the format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="stream">The given stream.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        /// <param name="format">The output format (JSON or YAML).</param>
        public static void Serialize<T>(
            this T element,
            Stream stream,
            AsyncApiSpecVersion specVersion,
            AsyncApiFormat format)
            where T : IAsyncApiSerializable
        {
            element.Serialize(stream, specVersion, format, null);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the Async API document using
        /// the given stream, specification version and the format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="stream">The given stream.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        /// <param name="format">The output format (JSON or YAML).</param>
        /// <param name="settings">Provide configuration settings for controlling writing output</param>
        public static void Serialize<T>(
            this T element,
            Stream stream,
            AsyncApiSpecVersion specVersion,
            AsyncApiFormat format, 
            AsyncApiWriterSettings settings)
            where T : IAsyncApiSerializable
        {
            if (stream == null)
            {
                throw Error.ArgumentNull(nameof(stream));
            }

            IAsyncApiWriter writer;
            var streamWriter = new FormattingStreamWriter(stream, CultureInfo.InvariantCulture);
            switch (format)
            {
                case AsyncApiFormat.Json:
                    writer = new AsyncApiJsonWriter(streamWriter,settings);
                    break;
                case AsyncApiFormat.Yaml:
                    writer = new AsyncApiYamlWriter(streamWriter, settings);
                    break;
                default:
                    throw new AsyncApiException(string.Format(SRResource.AsyncApiFormatNotSupported, format));
            }

            element.Serialize(writer, specVersion);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to Async API document using the given specification version and writer.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="writer">The output writer.</param>
        /// <param name="specVersion">Version of the specification the output should conform to</param>

        public static void Serialize<T>(this T element, IAsyncApiWriter writer, AsyncApiSpecVersion specVersion)
            where T : IAsyncApiSerializable
        {
            if (element == null)
            {
                throw Error.ArgumentNull(nameof(element));
            }

            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            switch (specVersion)
            {
                /*
                case AsyncApiSpecVersion.AsyncApi3_0:
                    element.SerializeAsV3(writer);
                    break;
                    */
                
                case AsyncApiSpecVersion.AsyncApi2_0:
                    element.SerializeAsV2(writer);
                    break;

                default:
                    throw new AsyncApiException(string.Format(SRResource.AsyncApiSpecVersionNotSupported, specVersion));
            }

            writer.Flush();
        }


        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the Async API document as a string in JSON format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        public static string SerializeAsJson<T>(
            this T element,
            AsyncApiSpecVersion specVersion)
            where T : IAsyncApiSerializable
        {
            return element.Serialize(specVersion, AsyncApiFormat.Json);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the Async API document as a string in YAML format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        public static string SerializeAsYaml<T>(
            this T element,
            AsyncApiSpecVersion specVersion)
            where T : IAsyncApiSerializable
        {
            return element.Serialize(specVersion, AsyncApiFormat.Yaml);
        }

        /// <summary>
        /// Serializes the <see cref="IAsyncApiSerializable"/> to the Async API document as a string in the given format.
        /// </summary>
        /// <typeparam name="T">the <see cref="IAsyncApiSerializable"/></typeparam>
        /// <param name="element">The Async API element.</param>
        /// <param name="specVersion">The Async API specification version.</param>
        /// <param name="format">Async API document format.</param>
        public static string Serialize<T>(
            this T element,
            AsyncApiSpecVersion specVersion,
            AsyncApiFormat format)
            where T : IAsyncApiSerializable
        {
            if (element == null)
            {
                throw Error.ArgumentNull(nameof(element));
            }

            using (var stream = new MemoryStream())
            {
                element.Serialize(stream, specVersion, format);
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
