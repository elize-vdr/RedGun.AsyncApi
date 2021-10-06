// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Expressions
{
    /// <summary>
    /// Method expression.
    /// </summary>
    public sealed class MethodExpression : RuntimeExpression
    {
        /// <summary>
        /// $method. string
        /// </summary>
        public const string Method = "$method";

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression { get; } = Method;

        /// <summary>
        /// Private constructor.
        /// </summary>
        public MethodExpression()
        {
        }
    }
}
