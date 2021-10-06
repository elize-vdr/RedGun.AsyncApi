// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API Date
    /// </summary>
    public class AsyncApiDate : AsyncApiPrimitive<DateTime>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiDate"/> class.
        /// </summary>
        public AsyncApiDate(DateTime value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Date;
    }
}
