using System;
using System.Collections.Generic;

namespace Model.Generation;

/// <summary>
/// Constant for MetaModel and produced C# files.
/// </summary>
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


    internal const string ModuleDirPath      = "./Model_Imp";
    internal const string ImmDirPath         = ModuleDirPath + "/_generated_";
    internal const string ImmCommonFilePath  = ImmDirPath + "/ModelCommonImmImp.cs";
    internal const string ImmConceptFilePath = ImmDirPath + "/ModelConceptImmImp.cs";
    internal const string ImmVisualityFilePath = ImmDirPath + "/ModelVisualityImmImp.cs";
}
