// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Writers;
using System.Collections.Generic;

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API array.
    /// </summary>
    public class AsyncApiArray : List<IAsyncApiAny>, IAsyncApiAny
    {
        /// <summary>
        /// The type of <see cref="IAsyncApiAny"/>
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Array;

        /// <summary>
        /// Write out contents of AsyncApiArray to passed writer
        /// </summary>
        /// <param name="writer">Instance of JSON or YAML writer.</param>
        /// <param name="specVersion">Version of the OpenAPI specification that that will be output.</param>
        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            writer.WriteStartArray();

            foreach (var item in this)
            {
                writer.WriteAny(item);
            }

            writer.WriteEndArray();

        }
    }
}
