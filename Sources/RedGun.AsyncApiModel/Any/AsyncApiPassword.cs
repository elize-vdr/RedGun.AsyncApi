// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

namespace RedGun.AsyncApi.Any
{
    /// <summary>
    /// Async API password.
    /// </summary>
    public class AsyncApiPassword : AsyncApiPrimitive<string>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiPassword"/> class.
        /// </summary>
        public AsyncApiPassword(string value)
            : base(value)
        {
        }

        /// <summary>
        /// The primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Password;
    }
}
