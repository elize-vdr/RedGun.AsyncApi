﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using RedGun.AsyncApi.Readers.Interface;

namespace RedGun.AsyncApi.Readers.Services
{
    /// <summary>
    /// Implementation of IInputLoader that loads streams from URIs
    /// </summary>
    internal class DefaultStreamLoader : IStreamLoader
    {
        private HttpClient _httpClient = new HttpClient();

        public Stream Load(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "file":
                    return File.OpenRead(uri.AbsolutePath);
                case "http":
                case "https":
                    return _httpClient.GetStreamAsync(uri).GetAwaiter().GetResult();

                default:
                    throw new ArgumentException("Unsupported scheme");
            }
        }

        public async Task<Stream> LoadAsync(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "file":
                    return File.OpenRead(uri.AbsolutePath);
                case "http":
                case "https":
                    return await _httpClient.GetStreamAsync(uri);
                default:
                    throw new ArgumentException("Unsupported scheme");
            }
        }
    }
}
