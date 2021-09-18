// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Services;

namespace RedGun.AsyncApi.Readers.Services
{
    /// <summary>
    /// Builds a list of all remote references used in an OpenApi document
    /// </summary>
    internal class AsyncApiRemoteReferenceCollector : AsyncApiVisitorBase
    {
        private AsyncApiDocument _document;
        private Dictionary<string, AsyncApiReference> _references = new Dictionary<string, AsyncApiReference>();
        public AsyncApiRemoteReferenceCollector(AsyncApiDocument document)
        {
            _document = document;
        }

        /// <summary>
        /// List of external references collected from AsyncApiDocument
        /// </summary>
        public IEnumerable<AsyncApiReference> References
        {
            get {
                return _references.Values;
            }
        }

        /// <summary>
        /// Collect reference for each reference 
        /// </summary>
        /// <param name="referenceable"></param>
        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            AddReference(referenceable.Reference);
        }

        /// <summary>
        /// Collect external reference
        /// </summary>
        private void AddReference(AsyncApiReference reference)
        {
            if (reference != null)
            {
                if (reference.IsExternal)
                {
                    if (!_references.ContainsKey(reference.ExternalResource))
                    {
                        _references.Add(reference.ExternalResource, reference);
                    }
                }
            }
        }    
    }
}
