using System;
using System.Collections.Generic;

namespace Util.Collections.ConstImp;


internal static class ConstCollectionAlgorithm
{

    internal static void SortUnique<T>(T[] array, out int newLength)
    {
        int n = array.Length;
        if (n < 2)
        {
            newLength = n;
            return;
        }

        Array.Sort(array);

        Comparer<T> comparer = Comparer<T>.Default;

        int p = 1;
        while (p < n)
        {
            int s = comparer.Compare(array[p - 1], array[p]);
            if (s < 0) p++;
            else break;
        }

        if (p == n)
        {
            newLength = n;
            return;
        }

        int q = p + 1;
        while (q < n)
        {
            int s = comparer.Compare(array[p-1], array[q]);
            if (s == 0) q++;
            else array[p++] = array[q++];
        }

        newLength = p;
    }

}
