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
            { typeof(sbyte),  "sbyte" },
            { typeof(ushort), "ushort" },
            { typeof(uint),   "uint" },
            { typeof(int),    "int" },
            { typeof(ulong),  "ulong" },
            { typeof(long),   "long" },
            { typeof(string), "string" },
        };



}
