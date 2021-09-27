// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API Float
    /// </summary>
    public class AsyncApiFloat : AsyncApiPrimitive<float>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiFloat"/> class.
        /// </summary>
        public AsyncApiFloat(float value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Float;
    }
}
