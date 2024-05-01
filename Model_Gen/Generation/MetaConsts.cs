using System;
using System.Collections.Generic;

namespace Model.Generation;

internal static class MetaConsts
{
    internal static readonly Dictionary<Type, string> SystemTypes =
        new()
        {
            { typeof(bool),   "bool" },
            { typeof(byte),   "byte" },
            { typeof(uint),   "uint" },
            { typeof(int),    "int" },
            { typeof(string), "string" },
        };



}
