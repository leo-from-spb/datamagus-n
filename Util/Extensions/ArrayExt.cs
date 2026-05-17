using System;
using System.Collections.Generic;

namespace Util.Extensions;

/// <summary>
/// Useful functions on array.
/// </summary>
public static class ArrayExt
{
    extension<E>(E[] array)
    {
        /// <summary>
        /// Searches an item with the given value in this array;
        /// this array should be sorted by the related values.
        /// </summary>
        /// <param name="value">the value to search an element with.</param>
        /// <param name="getter">function that gets the value of an element.</param>
        /// <param name="comparer">how to compare values.</param>
        /// <typeparam name="P">type a value.</typeparam>
        /// <returns>the index of the found value, or -1 when not found.</returns>
        public int BinarySearchByPart<P>(P            value,
                                         Func<E, P>   getter,
                                         IComparer<P> comparer) =>
            array.BinarySearchByPart(0, array.Length, value, getter, comparer);

        /// <summary>
        /// Searches an item with the given value in this array;
        /// this array should be sorted by the related values.
        /// </summary>
        /// <param name="from">index from which to start.</param>
        /// <param name="length">number of elements to check.</param>
        /// <param name="value">the value to search an element with.</param>
        /// <param name="getter">function that gets the value of an element.</param>
        /// <param name="comparer">how to compare values.</param>
        /// <typeparam name="P">type a value.</typeparam>
        /// <returns>the index of the found value, or -1 when not found.</returns>
        public int BinarySearchByPart<P>(int          from,
                                         int          length,
                                         P            value,
                                         Func<E, P>   getter,
                                         IComparer<P> comparer)
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
}
