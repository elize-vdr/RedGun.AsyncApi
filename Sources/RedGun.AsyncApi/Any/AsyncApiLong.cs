// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API long.
    /// </summary>
    public class AsyncApiLong : AsyncApiPrimitive<long>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiLong"/> class.
        /// </summary>
        public AsyncApiLong(long value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Long;
    }
}
