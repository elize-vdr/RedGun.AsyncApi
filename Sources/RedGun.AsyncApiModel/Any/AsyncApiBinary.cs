﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API binary.
    /// </summary>
    public class AsyncApiBinary : AsyncApiPrimitive<byte[]>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiBinary"/> class.
        /// </summary>
        /// <param name="value"></param>
        public AsyncApiBinary(byte[] value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Binary;
    }
}
