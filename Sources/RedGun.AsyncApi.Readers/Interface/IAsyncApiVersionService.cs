// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Readers.ParseNodes;

namespace RedGun.AsyncApi.Readers.Interface
{
    /// <summary>
    /// Interface to a version specific parsing implementations.
    /// </summary>
    internal interface IAsyncApiVersionService
    {

        /// <summary>
        /// Parse the string to a <see cref="AsyncApiReference"/> object.
        /// </summary>
        /// <param name="reference">The reference string.</param>
        /// <param name="type">The type of the reference.</param>
        /// <returns>The <see cref="AsyncApiReference"/> object or null.</returns>
        AsyncApiReference ConvertToAsyncApiReference(string reference, ReferenceType? type);

        /// <summary>
        /// Loads an AsyncAPI Element from a document fragment
        /// </summary>
        /// <typeparam name="T">Type of element to load</typeparam>
        /// <param name="node">document fragment node</param>
        /// <returns>Instance of AsyncAPIElement</returns>
        T LoadElement<T>(ParseNode node) where T : IAsyncApiElement;

        /// <summary>
        /// Converts a generic RootNode instance into a strongly typed AsyncApiDocument
        /// </summary>
        /// <param name="rootNode">RootNode containing the information to be converted into an AsyncAPI Document</param>
        /// <returns>Instance of AsyncApiDocument populated with data from rootNode</returns>
        AsyncApiDocument LoadDocument(RootNode rootNode);
    }
}
