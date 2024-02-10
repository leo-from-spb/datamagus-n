namespace Model.Gen.Generation;

internal static class MetaConsts
{
    internal static readonly Dictionary<Type, string> SystemTypes =
        new Dictionary<Type, string>()
        {
            { typeof(bool),   "bool" },
            { typeof(byte),   "byte" },
            { typeof(uint),   "uint" },
            { typeof(int),    "int" },
            { typeof(string), "string" },
        };



}
