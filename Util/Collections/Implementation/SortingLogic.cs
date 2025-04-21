using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections.Implementation;


internal static class SortingLogic
{

    internal static E[] SortAndDeduplicate<E>(this IEnumerable<E> collection)
        where E : IComparable<E>
    {
        E[] array = collection.ToArray();
        int n = array.Length;
        array.SortAndDeduplicate(out int m);
        if (m == n) return array;
        Array.Resize(ref array, m);
        return array;
    }


    /// <summary>
    /// Sorts and deduplicates elements inside this array.
    /// </summary>
    /// <param name="array">array with elements to sort and deduplicates.</param>
    /// <param name="newCount">new (possible smaller) count of elements.</param>
    /// <typeparam name="E"></typeparam>
    internal static void SortAndDeduplicate<E>(this E[] array, out int newCount)
        where E : IComparable<E>
    {
        int n = array.Length;
        newCount = n;
        if (n <= 1) return;

        Array.Sort(array);

        // check for duplicates
        var comparer = Comparer<E>.Default;
        int k = 1;
        while (k < n && comparer.Compare(array[k-1], array[k]) < 0) k++;
        if (k == n) return; // no duplicates

        // k points to the first duplicate
        // remove duplicates
        int m = k; // m will be the number of unique elements
        k++;
        while (k < n)
        {
            while (comparer.Compare(array[m - 1], array[k]) >= 0) k++;
            array[m] = array[k];
            m++;
            k++;
        }

        newCount = m;
    }

}
