﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;
using System.IO;
using System.Threading.Tasks;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Readers.Interface
{
    /// <summary>
    /// Interface for service that translates a URI into a stream that can be loaded by a Reader
    /// </summary>
    public interface IStreamLoader
    {
        /// <summary>
        /// Use Uri to locate data and convert into an input object.
        /// </summary>
        /// <param name="uri">Identifier of some source of an AsyncAPI Description</param>
        /// <returns>A data objext that can be processed by a reader to generate an <see cref="AsyncApiDocument"/></returns>
        Task<Stream> LoadAsync(Uri uri);

        /// <summary>
        /// Use Uri to locate data and convert into an input object.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Stream Load(Uri uri);
    }
}
