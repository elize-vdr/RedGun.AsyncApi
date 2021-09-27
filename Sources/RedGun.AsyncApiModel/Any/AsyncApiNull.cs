// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public class AsyncApiNull : IAsyncApiAny
    {
        /// <summary>
        /// The type of <see cref="IAsyncApiAny"/>
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Null;

        /// <summary>
        /// Write out null representation
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="specVersion">Version of the AsyncAPI specification that that will be output.</param>
        public void Write(IAsyncApiWriter writer, AsyncApiSpecVersion specVersion)
        {
            writer.WriteAny(this);
        }
    }
}
