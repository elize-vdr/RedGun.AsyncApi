// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Exceptions;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;
using RedGun.AsyncApi.Properties;

namespace RedGun.AsyncApi.Extensions
{
    /// <summary>
    /// Extension methods to verify validatity and add an extension to Extensions property.
    /// </summary>
    public static class AsyncApiExtensibleExtensions
    {
        /// <summary>
        /// Add extension into the Extensions
        /// </summary>
        /// <typeparam name="T"><see cref="IAsyncApiExtensible"/>.</typeparam>
        /// <param name="element">The extensible Async API element. </param>
        /// <param name="name">The extension name.</param>
        /// <param name="any">The extension value.</param>
        public static void AddExtension<T>(this T element, string name, IAsyncApiExtension any)
            where T : IAsyncApiExtensible
        {
            if (element == null)
            {
                throw Error.ArgumentNull(nameof(element));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw Error.ArgumentNullOrWhiteSpace(nameof(name));
            }

            if (!name.StartsWith(AsyncApiConstants.ExtensionFieldNamePrefix))
            {
                throw new AsyncApiException(string.Format(SRResource.ExtensionFieldNameMustBeginWithXDash, name));
            }

            element.Extensions[name] = any ?? throw Error.ArgumentNull(nameof(any));
        }
    }
}
