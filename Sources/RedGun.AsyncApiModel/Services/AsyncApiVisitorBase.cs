// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Services
{
    /// <summary>
    /// Async API visitor base provides common logic for concrete visitors
    /// </summary>
    public abstract class AsyncApiVisitorBase
    {
        private readonly Stack<string> _path = new Stack<string>();

        /// <summary>
        /// Properties available to identify context of where an object is within AsyncAPI Document
        /// </summary>
        public CurrentKeys CurrentKeys { get; } = new CurrentKeys();

        /// <summary>
        /// Allow Rule to indicate validation error occured at a deeper context level.  
        /// </summary>
        /// <param name="segment">Identifier for context</param>
        public void Enter(string segment)
        {
            this._path.Push(segment);
        }

        /// <summary>
        /// Exit from path context elevel.  Enter and Exit calls should be matched.
        /// </summary>
        public void Exit()
        {
            this._path.Pop();
        }

        /// <summary>
        /// Pointer to source of validation error in document
        /// </summary>
        public string PathString
        {
            get
            {
                return "#/" + String.Join("/", _path.Reverse());
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiDocument"/>
        /// </summary>
        public virtual void Visit(AsyncApiDocument doc)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiInfo"/>
        /// </summary>
        public virtual void Visit(AsyncApiInfo info)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiContact"/>
        /// </summary>
        public virtual void Visit(AsyncApiContact contact)
        {
        }


        /// <summary>
        /// Visits <see cref="AsyncApiLicense"/>
        /// </summary>
        public virtual void Visit(AsyncApiLicense license)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiServer"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiServer> servers)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiServer"/>
        /// </summary>
        public virtual void Visit(AsyncApiServer server)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiPaths"/>
        /// </summary>
        public virtual void Visit(AsyncApiPaths paths)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiPathItem"/>
        /// </summary>
        public virtual void Visit(AsyncApiPathItem pathItem)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiServerVariable"/>
        /// </summary>
        public virtual void Visit(AsyncApiServerVariable serverVariable)
        {
        }

        /// <summary>
        /// Visits the operations.
        /// </summary>
        public virtual void Visit(IDictionary<OperationType, AsyncApiOperation> operations)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOperation"/>
        /// </summary>
        public virtual void Visit(AsyncApiOperation operation)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiParameter"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiParameter> parameters)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiParameter"/>
        /// </summary>
        public virtual void Visit(AsyncApiParameter parameter)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiRequestBody"/>
        /// </summary>
        public virtual void Visit(AsyncApiRequestBody requestBody)
        {
        }


        /// <summary>
        /// Visits headers.
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiHeader> headers)
        {
        }

        /// <summary>
        /// Visits callbacks.
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiCallback> callbacks)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiResponse"/>
        /// </summary>
        public virtual void Visit(AsyncApiResponse response)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiResponses"/>
        /// </summary>
        public virtual void Visit(AsyncApiResponses response)
        {
        }

        /// <summary>
        /// Visits media type content.
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiMediaType> content)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiMediaType"/>
        /// </summary>
        public virtual void Visit(AsyncApiMediaType mediaType)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiEncoding"/>
        /// </summary>
        public virtual void Visit(AsyncApiEncoding encoding)
        {
        }

        /// <summary>
        /// Visits the examples.
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiExample> examples)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiComponents"/>
        /// </summary>
        public virtual void Visit(AsyncApiComponents components)
        {
        }


        /// <summary>
        /// Visits <see cref="AsyncApiComponents"/>
        /// </summary>
        public virtual void Visit(AsyncApiExternalDocs externalDocs)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSchema"/>
        /// </summary>
        public virtual void Visit(AsyncApiSchema schema)
        {
        }

        /// <summary>
        /// Visits the links.
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiLink> links)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiLink"/>
        /// </summary>
        public virtual void Visit(AsyncApiLink link)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiCallback"/>
        /// </summary>
        public virtual void Visit(AsyncApiCallback callback)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiTag"/>
        /// </summary>
        public virtual void Visit(AsyncApiTag tag)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiHeader"/>
        /// </summary>
        public virtual void Visit(AsyncApiHeader tag)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOAuthFlow"/>
        /// </summary>
        public virtual void Visit(AsyncApiOAuthFlow asyncApiOAuthFlow)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityRequirement"/>
        /// </summary>
        public virtual void Visit(AsyncApiSecurityRequirement securityRequirement)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityScheme"/>
        /// </summary>
        public virtual void Visit(AsyncApiSecurityScheme securityScheme)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiExample"/>
        /// </summary>
        public virtual void Visit(AsyncApiExample example)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiTag"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiTag> asyncApiTags)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiSecurityRequirement"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiSecurityRequirement> asyncApiSecurityRequirements)
        {
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtensible"/>
        /// </summary>
        public virtual void Visit(IAsyncApiExtensible asyncApiExtensible)
        {
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtension"/>
        /// </summary>
        public virtual void Visit(IAsyncApiExtension asyncApiExtension)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiExample"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiExample> example)
        {
        }

        /// <summary>
        /// Visits a dictionary of server variables
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiServerVariable> serverVariables)
        {
        }

        /// <summary>
        /// Visits a dictionary of encodings
        /// </summary>
        /// <param name="encodings"></param>
        public virtual void Visit(IDictionary<string, AsyncApiEncoding> encodings)
        {
        }

        /// <summary>
        /// Visits IAsyncApiReferenceable instances that are references and not in components
        /// </summary>
        /// <param name="referenceable">referenced object</param>
        public virtual void Visit(IAsyncApiReferenceable referenceable)
        {
        }
    }
}
