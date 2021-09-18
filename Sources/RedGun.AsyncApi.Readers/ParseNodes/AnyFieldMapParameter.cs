// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    internal class AnyFieldMapParameter<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AnyFieldMapParameter(
            Func<T, IAsyncApiAny> propertyGetter,
            Action<T, IAsyncApiAny> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        /// <summary>
        /// Function to retrieve the value of the property.
        /// </summary>
        public Func<T, IAsyncApiAny> PropertyGetter { get; }

        /// <summary>
        /// Function to set the value of the property.
        /// </summary>
        public Action<T, IAsyncApiAny> PropertySetter { get; }

        /// <summary>
        /// Function to get the schema to apply to the property.
        /// </summary>
        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}
