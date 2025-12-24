using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections.Implementation;


internal static class CollectionLogic
{
    internal static int IndexOf<E>(this E[]            array,
                                   E                   element,
                                   EqualityComparer<E> eq,
                                   int                 fromIndex = 0,
                                   int                 tillIndex = int.MaxValue,
                                   int                 notFound  = ImmConst.notFoundIndex)
    {
        int n = array.Length;
        if (fromIndex >= n) return notFound;
        for (var i = 0; i < int.Min(tillIndex, n); i++)
            if (eq.Equals(array[i], element)) return i;
        return notFound;
    }

    internal static int IndexOf<E>(this E[]     array,
                                   Predicate<E> predicate,
                                   int          fromIndex = 0,
                                   int          tillIndex = int.MaxValue,
                                   int          notFound  = ImmConst.notFoundIndex)
    {
        int n = array.Length;
        if (fromIndex >= n) return notFound;
        for (int i = 0; i < int.Min(tillIndex, n); i++)
            if (predicate(array[i])) return i;
        return notFound;
    }

    internal static int LastIndexOf<E>(this E[]     array,
                                       Predicate<E> predicate,
                                       int          fromIndex = 0,
                                       int          tillIndex = int.MaxValue,
                                       int          notFound  = ImmConst.notFoundIndex)
    {
        int n = array.Length;
        if (fromIndex >= n) return notFound;
        for (var i = int.Min(tillIndex, n) - 1; i >= 0; i--)
            if (predicate(array[i])) return i;
        return notFound;
    }

    internal static Found<E> FindElement<E>(this E[] array, Predicate<E> predicate)
    {
        foreach (E element in array)
            if (predicate(element)) return new Found<E>(true, element);
        return Found<E>.NotFound;
    }

    internal static Found<P> FindElementPart<E,P>(this E[] array, Func<E,P> part, Predicate<P> predicate, int fromIndex = 0)
    {
        for (var i = fromIndex; i < array.Length; i++)
        {
            P p = part(array[i]);
            if (predicate(p)) return new Found<P>(true, p);
        }

        return Found<P>.NotFound;
    }

    internal static Found<P> FindLastElementPart<E,P>(this E[] array, Func<E,P> part, Predicate<P> predicate)
    {
        for (var i = array.Length-1; i >= 0; i--)
        {
            P p = part(array[i]);
            if (predicate(p)) return new Found<P>(true, p);
        }

        return Found<P>.NotFound;
    }

    internal static Found<E> FindByIndex<E>(this E[] array, int index) =>
        index >= 0 && index < array.Length
            ? new Found<E>(true, array[index])
            : Found<E>.NotFound;

    internal static E[] CloneArray<E>(this E[] originalArray)
    {
        int n        = originalArray.Length;
        E[] newArray = new E[n];
        Array.Copy(originalArray, newArray, n);
        return newArray;
    }

    internal static List<E> Deduplicate<E>(this IEnumerable<E> source)
    {
        int n    = source is IReadOnlyCollection<E> sourceCollection ? sourceCollection.Count : 16;
        var list = new List<E>(n);
        var hset = new HashSet<E>(n);

        foreach (E element in source)
        {
            if (hset.Add(element))
                list.Add(element);
        }

        return list;
    }


    internal static bool IsNotEmpty<E>(this IEnumerable<E> source)
    {
        if (source is IReadOnlyCollection<E> collection)
        {
            return collection.Count > 0;
        }
        else
        {
            using IEnumerator<E> enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }
    }

    internal static bool IsEmpty<E>(this IEnumerable<E> source) => !source.IsNotEmpty();


    internal static int CountTrues(this BitArray bits)
    {
        int n = 0;
        foreach (bool b in bits)
            if (b) n++;
        return n;
    }


    internal static bool IsTheSetSubsetOf<E>(int count, Func<E,int> findIndex, IEnumerable<E> anotherSources, bool strict)
        => IsTheSetSubsetOf(count, count, findIndex, anotherSources, strict);

    internal static bool IsTheSetSubsetOf<E>(int Capacity, int count, Func<E,int> findIndex, IEnumerable<E> anotherSources, bool strict)
    {
        BitArray indices       = new BitArray(Capacity);
        bool     wasAnotherOne = false;
        foreach (var x in anotherSources)
        {
            int index = findIndex(x);

            if (index >= 0) indices[index] = true;
            else wasAnotherOne             = true;

            if (indices.CountTrues() >= count && (wasAnotherOne || !strict)) return true;
        }
        return false;
    }

    internal static bool IsTheSetSupersetOf<E>(int count, Func<E,int> findIndex, IEnumerable<E> anotherSources, bool strict)
        => IsTheSetSupersetOf(count, count, findIndex, anotherSources, strict);

    internal static bool IsTheSetSupersetOf<E>(int Capacity, int count, Func<E,int> findIndex, IEnumerable<E> anotherSources, bool strict)
    {
        BitArray indices = new BitArray(Capacity);
        foreach (var x in anotherSources)
        {
            int index = findIndex(x);
            if (index < 0) return false;
            indices[index] = true;
        }
        return indices.CountTrues() < count || !strict;
    }

    internal static bool IsTheSetOverlapping<E>(Predicate<E> elementContains, IEnumerable<E> anotherSources)
    {
        foreach (var x in anotherSources)
            if (elementContains(x)) return true;
        return false;
    }

    internal static bool IsTheSetEqualTo<E>(int count, Func<E,int> findIndex, IEnumerable<E> anotherSources)
        => IsTheSetEqualTo(count, count, findIndex, anotherSources);

    internal static bool IsTheSetEqualTo<E>(int Capacity, int count, Func<E,int> findIndex, IEnumerable<E> anotherSources)
    {
        BitArray indices = new BitArray(Capacity);
        foreach (var x in anotherSources)
        {
            int index = findIndex(x);
            if (index < 0) return false;
            indices[index] = true;
        }
        return indices.CountTrues() == count;
    }

    internal static bool IsTheSingletonSubsetOf<E>(E element, EqualityComparer<E> eq, IEnumerable<E> anotherSources, bool strict)
    {
        if (!strict) return anotherSources.Contains(element);

        bool such = false, another = false;
        foreach (var x in anotherSources)
        {
            bool equal = eq.Equals(element, x);
            such    |= equal;
            another |= !equal;
            if (such && another) return true;
        }
        return false;
    }

    internal static bool IsTheSingletonSupersetOf<E>(E element, EqualityComparer<E> eq, IEnumerable<E> anotherSources, bool strict)
    {
        if (strict) return anotherSources.IsEmpty();

        foreach (var x in anotherSources)
            if (eq.Equals(element, x) == false) return false;
        return true;
    }

    internal static bool IsTheSingletonEqualTo<E>(E element, EqualityComparer<E> eq, IEnumerable<E> other)
    {
        int n = 0;
        foreach (var x in other)
        {
            n++;
            if (n > 1) return false;
            if (eq.Equals(element, x) == false) return false;
        }
        return true;
    }

    internal static ImmSet<E> UnionSet<E>(ImmSet<E> setA, ImmSet<E> setB)
    {
        int nA = setA.Count;
        int nB = setB.Count;
        if (nB == 0) return setA;
        if (nA == 0) return setB;
        // TODO optimize
        return new ImmutableUnionSet<E>(setA, setB);
    }

    internal static ImmSet<E> UnionSet<E>(ImmSet<E> setA, E[] arrayB)
    {
        if (setA.IsEmpty) return arrayB.ToImmSet();
        var listB = new List<E>(arrayB.Length);
        foreach (var el in arrayB)
            if (setA.Contains(el) == false) listB.Add(el);
        if (listB.IsEmpty()) return setA;
        var setB     = listB.ToImmSet();
        int newCount = setA.Count + setB.Count;
        return new ImmutableUnionSet<E>(setA, setB, newCount);
    }

}
