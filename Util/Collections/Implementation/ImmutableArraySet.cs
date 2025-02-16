using System;
using System.Collections;
using System.Collections.Generic;

namespace Util.Collections.Implementation;


/// <summary>
/// Array-based immutable set.
/// All elements are expected to be different.
/// </summary>
internal class ImmutableArraySet<T> : ImmutableArrayList<T>, ImmOrderedSet<T>
{
    /// <summary>
    /// Internal constructor.
    /// The caller must guarantee that all elements are different.
    /// </summary>
    /// <param name="elements">array of different elements.</param>
    internal ImmutableArraySet(T[] elements) : base(elements) { }

    public bool IsSubsetOf(IEnumerable<T>       other) => IsSubsetOf(other, false);
    public bool IsProperSubsetOf(IEnumerable<T> other) => IsSubsetOf(other, true);

    private bool IsSubsetOf(IEnumerable<T> other, bool strict)
    {
        if (other is IReadOnlyCollection<T> collection && collection.Count + (strict ? 1 : 0) < this.Count) return false;
        BitArray indices       = new BitArray(this.Count);
        bool     wasAnotherOne = false;
        foreach (var x in other)
        {
            int index = this.IndexOf(x);

            if (index >= 0) indices[index] = true;
            else wasAnotherOne             = true;

            if (indices.CountTrues() >= this.Count && (wasAnotherOne || !strict)) return true;
        }
        return false;
    }

    public bool IsSupersetOf(IEnumerable<T>       other) => IsSupersetOf(other, false);
    public bool IsProperSupersetOf(IEnumerable<T> other) => IsSupersetOf(other, true);

    public bool IsSupersetOf(IEnumerable<T> other, bool strict)
    {
        BitArray indices = new BitArray(this.Count);
        foreach (var x in other)
        {
            int index = this.IndexOf(x);
            if (index < 0) return false;
            indices[index] = true;
        }
        return indices.CountTrues() < this.Count || !strict;
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        foreach (var x in other)
            if (this.Contains(x)) return true;
        return false;
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        BitArray indices = new BitArray(this.Count);
        foreach (var x in other)
        {
            int index = this.IndexOf(x);
            if (index < 0) return false;
            indices[index] = true;
        }
        return indices.CountTrues() == this.Count;
    }
}




/// <summary>
/// Small immutable set of items.
/// Preserves the order of items.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
internal class ImmutableMiniSet<T> : ImmutableArraySet<T>
{
    internal ImmutableMiniSet(T[] elements) : base(elements) { }

}



/// <summary>
/// Compact hash set.
/// Preserves the order of items.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
internal class ImmutableHashSet<T> : ImmutableArraySet<T>
{
    private readonly HashTableEntry[] HashTable;

    public ImmutableHashSet(T[] array) : base(array)
    {
        HashTableLogic.BuildHashTable<T,T>(Elements, e => e, eq, out HashTable);
    }

    public override bool Contains(T element)
    {
        return HashTableLogic.FindIndex<T,T>(Elements, HashTable, e => e, eq, element, -1) >= 0;
    }

    public override int IndexOf(T element, int notFound = int.MinValue)
    {
        return HashTableLogic.FindIndex<T,T>(Elements, HashTable, e => e, eq, element, notFound);
    }

    public override int LastIndexOf(T element, int notFound = int.MinValue) => IndexOf(element, notFound);
}



/// <summary>
/// Array-based sorted immutable set.
/// </summary>
internal class ImmutableSortedSet<T> : ImmutableArraySet<T>, ImmSortedSet<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Makes a sorted set.
    /// </summary>
    /// <param name="elements">sorted array without duplicated elements.</param>
    internal ImmutableSortedSet(T[] elements) : base(elements) { }

    public override bool Contains(T element)
    {
        int index = Array.BinarySearch(Elements, element);
        return index >= 0;
    }

    public override int IndexOf(T element, int notFound = int.MinValue)
    {
        int index = Array.BinarySearch(Elements, element);
        return index >= 0 ? index : notFound;
    }

    public override int LastIndexOf(T element, int notFound = int.MinValue) => IndexOf(element, notFound);
}
