// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System.Collections.Generic;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Writers;

namespace RedGun.AsyncApi.Models
{
    /// <summary>
    /// Discriminator object.
    /// </summary>
    public class AsyncApiDiscriminator : IAsyncApiSerializable
    {
        /// <summary>
        /// REQUIRED. The name of the property in the payload that will hold the discriminator value.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// An object to hold mappings between payload values and schema names or references.
        /// </summary>
        public IDictionary<string, string> Mapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Serialize <see cref="AsyncApiDiscriminator"/> to Async API v3.0
        /// </summary>
        public void SerializeAsV3(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            writer.WriteStartObject();

            // propertyName
            writer.WriteProperty(AsyncApiConstants.PropertyName, PropertyName);

            // mapping
            writer.WriteOptionalMap(AsyncApiConstants.Mapping, Mapping, (w, s) => w.WriteValue(s));

            writer.WriteEndObject();
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiDiscriminator"/> to Async API v2.0
        /// </summary>
        public void SerializeAsV2(IAsyncApiWriter writer)
        {
            // Discriminator object does not exist in V2.
        }
    }
}
