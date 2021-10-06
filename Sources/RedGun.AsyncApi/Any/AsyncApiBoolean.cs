// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API boolean.
    /// </summary>
    public class AsyncApiBoolean : AsyncApiPrimitive<bool>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiBoolean"/> class.
        /// </summary>
        /// <param name="value"></param>
        public AsyncApiBoolean(bool value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Boolean;
    }
}
