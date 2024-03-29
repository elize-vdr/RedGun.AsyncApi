﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
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
        /// Serialize to AsyncAPI V2 document without using reference.
        /// </summary>
        void SerializeAsV2WithoutReference(IAsyncApiWriter writer);
    }
}
