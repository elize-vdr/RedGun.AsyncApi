// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API Byte
    /// </summary>
    public class AsyncApiByte : AsyncApiPrimitive<byte[]>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiByte"/> class.
        /// </summary>
        public AsyncApiByte(byte value)
            : this(new byte[] { value })
        {
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiByte"/> class.
        /// </summary>
        public AsyncApiByte(byte[] value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Byte;
    }
}
