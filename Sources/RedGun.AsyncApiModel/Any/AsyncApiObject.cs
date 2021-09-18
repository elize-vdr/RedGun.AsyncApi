// Copyright (c) Microsoft Corporation. All rights reserved.
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
        /// <param name="specVersion">Version of the OpenAPI specification that that will be output.</param>
        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
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
