using System;
using System.Collections.Generic;

namespace Util.Extensions;

/// <summary>
/// Useful functions on array.
/// </summary>
public static class ArrayExt
{

    public static int BinarySearchByPart<E, P>(this E[] array, P value, Func<E, P> getter, IComparer<P> comparer) =>
        array.BinarySearchByPart(0, array.Length, value, getter, comparer);

    public static int BinarySearchByPart<E,P>(this E[] array, int from, int length, P value, Func<E,P> getter, IComparer<P> comparer)
    {
        if (array.IsEmpty()) return -1;

        int lo = from;
        int hi = from + length - 1;
        while (lo <= hi)
        {
            int i     = lo + ((hi - lo) >> 1);
            int order = comparer.Compare(getter(array[i]), value);

            if (order == 0) return i;
            if (order < 0)
            {
                lo = i + 1;
            }
            else
            {
                hi = i - 1;
            }
        }

        return ~lo;
    }

}
