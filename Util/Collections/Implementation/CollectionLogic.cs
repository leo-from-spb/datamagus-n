using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections.Implementation;


internal static class CollectionLogic
{

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

    internal static bool IsEmpty<E>(this IEnumerable<E> source) => !IsNotEmpty(source);


    internal static int CountTrues(this BitArray bits)
    {
        int n = 0;
        foreach (bool b in bits)
            if (b) n++;
        return n;
    }


    internal static bool IsTheSetSubsetOf<E>(int count, Func<E,int> findIndex, IEnumerable<E> anotherSources, bool strict)
    {
        BitArray indices       = new BitArray(count);
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
    {
        BitArray indices = new BitArray(count);
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
    {
        BitArray indices = new BitArray(count);
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

}
