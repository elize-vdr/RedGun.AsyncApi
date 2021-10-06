// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System.Collections.Generic;
using System.Linq;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Extensions
{
    /// <summary>
    /// Extension methods for resolving references on <see cref="IAsyncApiReferenceable"/> elements.
    /// </summary>
    public static class AsyncApiReferencableExtensions
    {
        /// <summary>
        /// Resolves a JSON Pointer with respect to an element, returning the referenced element.
        /// </summary>
        /// <param name="element">The referencable Async API element on which to apply the JSON pointer</param>
        /// <param name="pointer">a JSON Pointer [RFC 6901](https://tools.ietf.org/html/rfc6901).</param>
        /// <returns>The element pointed to by the JSON pointer.</returns>
        public static IAsyncApiReferenceable ResolveReference(this IAsyncApiReferenceable element, JsonPointer pointer)
        {
            if (!pointer.Tokens.Any())
            {
                return element;
            }
            var propertyName = pointer.Tokens.FirstOrDefault();
            var mapKey = pointer.Tokens.ElementAtOrDefault(1);
            try
            {
                if (element.GetType() == typeof(AsyncApiHeader))
                {
                    return ResolveReferenceOnHeaderElement((AsyncApiHeader)element, propertyName, mapKey, pointer);
                }
                if (element.GetType() == typeof(AsyncApiParameter))
                {
                    return ResolveReferenceOnParameterElement((AsyncApiParameter)element, propertyName, mapKey, pointer);
                }
                if (element.GetType() == typeof(AsyncApiResponse))
                {
                    return ResolveReferenceOnResponseElement((AsyncApiResponse)element, propertyName, mapKey, pointer);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
            }
            throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
        }

        private static IAsyncApiReferenceable ResolveReferenceOnHeaderElement(
            AsyncApiHeader headerElement,
            string propertyName,
            string mapKey,
            JsonPointer pointer)
        {
            switch (propertyName)
            {
                case AsyncApiConstants.Schema:
                    return headerElement.Schema;
                case AsyncApiConstants.Examples when mapKey != null:
                    return headerElement.Examples[mapKey];
                default:
                    throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
            }
        }

        private static IAsyncApiReferenceable ResolveReferenceOnParameterElement(
            AsyncApiParameter parameterElement,
            string propertyName,
            string mapKey,
            JsonPointer pointer)
        {
            switch (propertyName)
            {
                case AsyncApiConstants.Schema:
                    return parameterElement.Schema;
                case AsyncApiConstants.Examples when mapKey != null:
                    return parameterElement.Examples[mapKey];
                default:
                    throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
            }
        }

        private static IAsyncApiReferenceable ResolveReferenceOnResponseElement(
            AsyncApiResponse responseElement,
            string propertyName,
            string mapKey,
            JsonPointer pointer)
        {
            switch (propertyName)
            {
                case AsyncApiConstants.Headers when mapKey != null:
                    return responseElement.Headers[mapKey];
                case AsyncApiConstants.Links when mapKey != null:
                    return responseElement.Links[mapKey];
                default:
                    throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
            }
        }
    }
}
