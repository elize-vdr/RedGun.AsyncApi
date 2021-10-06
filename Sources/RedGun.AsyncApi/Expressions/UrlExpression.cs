// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Expressions
{
    /// <summary>
    /// Url expression.
    /// </summary>
    public sealed class UrlExpression : RuntimeExpression
    {
        /// <summary>
        /// $url string.
        /// </summary>
        public const string Url = "$url";

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression { get; } = Url;

        /// <summary>
        /// Private constructor.
        /// </summary>
        public UrlExpression()
        {
        }
    }
}
