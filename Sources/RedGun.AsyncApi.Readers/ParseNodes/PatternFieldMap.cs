// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. 

using System;
using System.Collections.Generic;

namespace RedGun.AsyncApi.Readers.ParseNodes
{
    internal class PatternFieldMap<T> : Dictionary<Func<string, bool>, Action<T, string, ParseNode>>
    {
    }
}
