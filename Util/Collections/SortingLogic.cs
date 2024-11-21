using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;


public static class SortingLogic
{

    public static ArraySegment<E> SortAndDeduplicate<E>(this IEnumerable<E> collection)
    where E : IComparable<E>
    {
        E[] array = collection.ToArray();
        int n = array.Length;
        array.SortAndDeduplicate(out int m);
        if (m == n) return new ArraySegment<E>(array);

        int d = n - m;
        if (d <= n / 16)
        {
            Array.Fill(array, default(E), m, d);
            return new ArraySegment<E>(array, 0, m);
        }
        else
        {
            Array.Resize(ref array, m);
            return new ArraySegment<E>(array);
        }
    }

    private static void SortAndDeduplicate<E>(this E[] array, out int newLength)
        where E : IComparable<E>
    {
        int n = array.Length;
        newLength = n;
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

        newLength = m;
    }

}
