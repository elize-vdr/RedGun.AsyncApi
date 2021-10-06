// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API Double
    /// </summary>
    public class AsyncApiDouble : AsyncApiPrimitive<double>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiDouble"/> class.
        /// </summary>
        public AsyncApiDouble(double value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Double;
    }
}
