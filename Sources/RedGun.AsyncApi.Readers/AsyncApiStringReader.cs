// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.IO;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.Interface;

namespace RedGun.AsyncApi.Readers
{
    /// <summary>
    /// Service class for converting strings into AsyncApiDocument instances
    /// </summary>
    public class AsyncApiStringReader : IAsyncApiReader<string, AsyncApiDiagnostic>
    {
        private readonly AsyncApiReaderSettings _settings;

        /// <summary>
        /// Constructor tha allows reader to use non-default settings
        /// </summary>
        /// <param name="settings"></param>
        public AsyncApiStringReader(AsyncApiReaderSettings settings = null)
        {
            _settings = settings ?? new AsyncApiReaderSettings();
        }

        /// <summary>
        /// Reads the string input and parses it into an Async API document.
        /// </summary>
        public AsyncApiDocument Read(string input, out AsyncApiDiagnostic diagnostic)
        {
            using (var reader = new StringReader(input))
            {
                return new AsyncApiTextReaderReader(_settings).Read(reader, out diagnostic);
            }
        }

        /// <summary>
        /// Reads the string input and parses it into an Async API element.
        /// </summary>
        public T ReadFragment<T>(string input, AsyncApiSpecVersion version, out AsyncApiDiagnostic diagnostic) where T : IAsyncApiElement
        {
            using (var reader = new StringReader(input))
            {
                return new AsyncApiTextReaderReader(_settings).ReadFragment<T>(reader, version, out diagnostic);
            }
        }
    }
}
