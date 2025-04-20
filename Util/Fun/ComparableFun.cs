using System;

namespace Util.Fun;

public static class ComparableFun
{
    /// <summary>
    /// The greatest of two values.
    /// </summary>
    public static V Greatest<V>(V a, V b)
        where V : IComparable<V>
        => a.CompareTo(b) >= 0 ? a : b;

    /// <summary>
    /// The least of two values.
    /// </summary>
    public static V Least<V>(V a, V b)
        where V : IComparable<V>
        => a.CompareTo(b) <= 0 ? a : b;

}
