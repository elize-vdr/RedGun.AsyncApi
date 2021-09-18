// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Interfaces
{
    /// <summary>
    /// Represents an Async API element that comes with serialzation functionality.
    /// </summary>
    public interface IAsyncApiSerializable : IAsyncApiElement
    {
        /// <summary>
        /// Serialize Async API element to v3.0.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void SerializeAsV3(IOpenApiWriter writer);

        /// <summary>
        /// Serialize Async API element to v2.0.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void SerializeAsV2(IOpenApiWriter writer);
    }
}
