// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;
using RedGun.AsyncApi.Any;
using RedGun.AsyncApi.Interfaces;
using RedGun.AsyncApi.Models;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    internal class AnyMapFieldMapParameter<T, U>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AnyMapFieldMapParameter(
            Func<T, IDictionary<string, U>> propertyMapGetter,
            Func<U, IAsyncApiAny> propertyGetter,
            Action<U, IAsyncApiAny> propertySetter,
            Func<T, AsyncApiSchema> schemaGetter)
        {
            this.PropertyMapGetter = propertyMapGetter;
            this.PropertyGetter = propertyGetter;
            this.PropertySetter = propertySetter;
            this.SchemaGetter = schemaGetter;
        }

        /// <summary>
        /// Function to retrieve the property that is a map from string to an inner element containing IAsyncApiAny.
        /// </summary>
        public Func<T, IDictionary<string, U>> PropertyMapGetter { get; }

        /// <summary>
        /// Function to retrieve the value of the property from an inner element.
        /// </summary>
        public Func<U, IAsyncApiAny> PropertyGetter { get; }

        /// <summary>
        /// Function to set the value of the property.
        /// </summary>
        public Action<U, IAsyncApiAny> PropertySetter { get; }

        /// <summary>
        /// Function to get the schema to apply to the property.
        /// </summary>
        public Func<T, AsyncApiSchema> SchemaGetter { get; }
    }
}
