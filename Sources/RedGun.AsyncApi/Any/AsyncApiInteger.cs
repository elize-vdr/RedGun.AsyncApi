// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API Integer
    /// </summary>
    public class AsyncApiInteger : AsyncApiPrimitive<int>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiInteger"/> class.
        /// </summary>
        public AsyncApiInteger(int value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Integer;
    }
}
