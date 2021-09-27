// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Interfaces
{
    /// <summary>
    /// Interface requuired for implementing any custom extension
    /// </summary>
    public interface IAsyncApiExtension
    {
        /// <summary>
        /// Write out contents of custom extension
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="specVersion">Version of the AsyncAPI specification that that will be output.</param>
        void Write(IAsyncApiWriter writer, AsyncApiSpecVersion specVersion);
    }
}
