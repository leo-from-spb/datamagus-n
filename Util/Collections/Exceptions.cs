using System;

namespace Util.Collections;


public class KeyDuplicationException : Exception
{
    public readonly int Index1;
    public readonly int Index2;

    public KeyDuplicationException(int index1, int index2, object? key)
        : base(PrepareMessage(index1, index2, key))
    {
        this.Index1 = index1;
        this.Index2 = index2;
    }

    private static string PrepareMessage(int index1, int index2, object? key)
    {
        string keyString = key?.ToString() ?? "<null>";
        return $"Duplicate key: entries with indices {index1} and {index2} have the same key: \"{keyString}\"";
    }
}
