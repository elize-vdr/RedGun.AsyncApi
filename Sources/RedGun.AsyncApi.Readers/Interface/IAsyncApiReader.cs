﻿// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Readers.Interface
{
    /// <summary>
    /// Interface for Async API readers.
    /// </summary>
    /// <typeparam name="TInput">The type of input to read from.</typeparam>
    /// <typeparam name="TDiagnostic">The type of diagnostic for information from reading process.</typeparam>
    public interface IAsyncApiReader<TInput, TDiagnostic> where TDiagnostic : IDiagnostic
    {
        /// <summary>
        /// Reads the input and parses it into an Async API document.
        /// </summary>
        /// <param name="input">The input to read from.</param>
        /// <param name="diagnostic">The diagnostic entity containing information from the reading process.</param>
        /// <returns>The Async API document.</returns>
        AsyncApiDocument Read(TInput input, out TDiagnostic diagnostic);
    }
}
