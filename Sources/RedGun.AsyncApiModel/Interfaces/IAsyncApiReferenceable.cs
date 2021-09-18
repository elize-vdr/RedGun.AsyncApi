// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Interfaces
{
    /// <summary>
    /// Represents an Async API element is referenceable.
    /// </summary>
    public interface IAsyncApiReferenceable : IAsyncApiSerializable
    {

        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data
        /// </summary>
        bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Serialize to OpenAPI V3 document without using reference.
        /// </summary>
        void SerializeAsV3WithoutReference(IOpenApiWriter writer);

        /// <summary>
        /// Serialize to OpenAPI V2 document without using reference.
        /// </summary>
        void SerializeAsV2WithoutReference(IOpenApiWriter writer);
    }
}
