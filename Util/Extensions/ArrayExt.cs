using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Util.Extensions;

/// <summary>
/// Useful functions on array.
/// </summary>
public static class ArrayExt
{
    extension<E>(E[]? array)
    {
        /// <summary>
        /// Checks whether this array contains something.
        /// </summary>
        /// <seealso cref="Nothing"/>
        [OverloadResolutionPriority(Int32.MaxValue)]
        public bool Some => array is not null && array.Length != 0;

        /// <summary>
        /// Checks whether this array is null or empty.
        /// <p>
        /// The difference between <c>Nothing</c> and <c>IsEmpty</c> is
        /// that <c>Nothing</c> checks the array for null but <c>IsEmpty</c> requires a non-null reference.
        /// </p>
        /// </summary>
        /// <seealso cref="Some"/>
        [OverloadResolutionPriority(Int32.MaxValue)]
        public bool Nothing => array is null || array.Length == 0;
    }


    extension<E>(E[] array)
    {
        /// <summary>
        /// Checks whether this array is empty.
        /// </summary>
        [OverloadResolutionPriority(Int32.MaxValue)]
        public bool IsEmpty => array.Length == 0;


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
            if (array.IsEmpty) return -1;

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
