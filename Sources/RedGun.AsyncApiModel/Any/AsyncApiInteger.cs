// Copyright (c) Microsoft Corporation. All rights reserved.
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
