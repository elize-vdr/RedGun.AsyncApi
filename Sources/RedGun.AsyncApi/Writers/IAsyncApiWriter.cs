﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Writers
{
    /// <summary>
    /// Interface for writing Async API documentation.
    /// </summary>
    public interface IAsyncApiWriter
    {
        /// <summary>
        /// Write the start object.
        /// </summary>
        void WriteStartObject();

        /// <summary>
        /// Write the end object.
        /// </summary>
        void WriteEndObject();

        /// <summary>
        /// Write the start array.
        /// </summary>
        void WriteStartArray();

        /// <summary>
        /// Write the end array.
        /// </summary>
        void WriteEndArray();

        /// <summary>
        /// Write the property name.
        /// </summary>
        void WritePropertyName(string name);

        /// <summary>
        /// Write the string value.
        /// </summary>
        void WriteValue(string value);

        /// <summary>
        /// Write the decimal value.
        /// </summary>
        void WriteValue(decimal value);

        /// <summary>
        /// Write the int value.
        /// </summary>
        void WriteValue(int value);

        /// <summary>
        /// Write the boolean value.
        /// </summary>
        void WriteValue(bool value);

        /// <summary>
        /// Write the null value.
        /// </summary>
        void WriteNull();

        /// <summary>
        /// Write the raw content value.
        /// </summary>
        void WriteRaw(string value);

        /// <summary>
        /// Write the object value.
        /// </summary>
        void WriteValue(object value);

        /// <summary>
        /// Flush the writer.
        /// </summary>
        void Flush();
    }
}
