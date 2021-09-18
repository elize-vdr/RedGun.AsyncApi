// Copyright (c) Microsoft Corporation. All rights reserved.
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
