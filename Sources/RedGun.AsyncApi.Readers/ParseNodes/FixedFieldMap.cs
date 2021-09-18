// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    internal class FixedFieldMap<T> : Dictionary<string, Action<T, ParseNode>>
    {
    }
}
