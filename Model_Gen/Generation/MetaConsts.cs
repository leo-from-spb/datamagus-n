using System;
using System.Collections.Generic;
using Util.Collections;

namespace Model.Generation;

internal static class MetaConsts
{
    internal static readonly string GeneratedFileHeader =
        """
        /****************************************************************************\
         *                                                                          *
         *                   THIS FILE IS GENERATED AUTOMATICALLY                   *
         *                     DON'T MODIFY THIS FILE MANUALLY                      *
         *                                                                          *
        \****************************************************************************/
        """;

    internal static readonly ImmDict<Type,string> SystemTypes =
        new Dictionary<Type,string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(ushort), "ushort" },
            { typeof(uint), "uint" },
            { typeof(int), "int" },
            { typeof(ulong), "ulong" },
            { typeof(long), "long" },
            { typeof(string), "string" },
        }.ToImmDict();

}
