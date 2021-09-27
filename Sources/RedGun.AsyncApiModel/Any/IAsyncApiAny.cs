// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Interfaces;

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Base interface for all the types that represent Async API Any.
    /// </summary>
    public interface IAsyncApiAny : IAsyncApiElement, IAsyncApiExtension
    {
        /// <summary>
        /// Type of an <see cref="IAsyncApiAny"/>.
        /// </summary>
        AnyType AnyType { get; }
    }
}
