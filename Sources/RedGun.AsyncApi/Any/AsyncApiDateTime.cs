// Copied from Microsoft OpenAPI.Net SDK and altered to obtain an AsyncAPI.Net SDK
// Licensed under the MIT license. 

using System;

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API Datetime
    /// </summary>
    public class AsyncApiDateTime : AsyncApiPrimitive<DateTimeOffset>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiDateTime"/> class.
        /// </summary>
        public AsyncApiDateTime(DateTimeOffset value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.DateTime;
    }
}
