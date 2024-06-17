using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections.ConstImp;


public static class ConstFactory
{

    public static ImmList<T> MakeList<T>(Span<T> elements) =>
        elements.Length switch
        {
            0 => ConstEmptySet<T>.Instance,
            1 => new ConstSingletonSet<T>(elements[0]),
            _ => new ConstArrayList<T>(elements.ToArray(), false)
        };

    public static ImmList<T> MakeList<T>(ReadOnlySpan<T> elements) =>
        elements.Length switch
        {
            0 => ConstEmptySet<T>.Instance,
            1 => new ConstSingletonSet<T>(elements[0]),
            _ => new ConstArrayList<T>(elements.ToArray(), false)
        };

    public static ImmList<T> MakeList<T>(IEnumerable<T> elements)
    {
        if (elements is IReadOnlyCollection<T> collection)
        {
            return MakeList(collection);
        }
        else
        {
            T[] array = elements.ToArray();
            return array.Length switch
                   {
                       0 => ConstEmptySet<T>.Instance,
                       1 => new ConstSingletonSet<T>(array[0]),
                       _ => new ConstArrayList<T>(array, false)
                   };
        }
    }

    public static ImmList<T> MakeList<T>(IReadOnlyCollection<T> elements) =>
        elements.Count switch
        {
            0 => ConstEmptySet<T>.Instance,
            1 => new ConstSingletonSet<T>(elements.First()),
            _ => new ConstArrayList<T>(elements)
        };

    public static ImmSortedSet<T> MakeSortedSet<T>(Span<T> elements)
        where T: IComparable<T>
    {
        int n = elements.Length;
        if (n == 0) return ConstEmptySortedSet<T>.Instance;
        if (n == 1) return new ConstSingletonSortedSet<T>(elements[0]);

        T[] array = elements.ToArray(); // copies elements into a new array
        return MakeSortedSetUsingCopiedArray(array);
    }

    public static ImmSortedSet<T> MakeSortedSet<T>(ReadOnlySpan<T> elements)
        where T: IComparable<T>
    {
        int n = elements.Length;
        if (n == 0) return ConstEmptySortedSet<T>.Instance;
        if (n == 1) return new ConstSingletonSortedSet<T>(elements[0]);

        T[] array = elements.ToArray(); // copies elements into a new array
        return MakeSortedSetUsingCopiedArray(array);
    }

    internal static ImmSortedSet<T> MakeSortedSetUsingCopiedArray<T>(T[] array)
        where T : IComparable<T>
    {
        int n = array.Length;
        if (n == 0) return ConstEmptySortedSet<T>.Instance;
        if (n == 1) return new ConstSingletonSortedSet<T>(array[0]);
        ConstCollectionAlgorithm.SortUnique(array, out n);
        if (n == 1) return new ConstSingletonSortedSet<T>(array[0]);
        return new ConstArraySortedSet<T>(array, 0, n, false);
    }


}
