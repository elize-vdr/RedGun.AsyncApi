// Copyright (c) Microsoft Corporation. All rights reserved.
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
