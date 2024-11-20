using System;
using System.Collections.Generic;

namespace Util.Collections;

/// <summary>
/// Immutable set of items, all items are different.
/// </summary>
/// <typeparam name="E">type of elements.</typeparam>
public abstract class ImmSet<E> : ImmList<E>
{

    protected ImmSet(E[] array, bool copy) : base(array, copy) { }

    internal ImmSet(ArraySegment<E> segment) : base(segment) { }

}


/// <summary>
/// Empty Set.
/// </summary>
/// <typeparam name="E">type of elements.</typeparam>
public class ImmZeroSet<E> : ImmSet<E>
{
    public static readonly ImmZeroSet<E> Instance = new ImmZeroSet<E>();

    private static readonly E[] EmptyArray = [];

    public ImmZeroSet() : base(EmptyArray) { }

    public override int IndexOf(E item) => -1;
    public override int LastIndexOf(E item) => -1;

    protected override string CollectionWord => "ZeroSet";
}


/// <summary>
/// Small immutable set of items.
/// Preserves the order of items.
/// </summary>
/// <typeparam name="E">type of elements.</typeparam>
public class ImmMiniSet<E> : ImmSet<E>
{
    public ImmMiniSet(E[] array, bool copy) : base(array, copy) { }

    internal ImmMiniSet(ArraySegment<E> segment) : base(segment) { }

    protected override string CollectionWord => "MiniSet";
}



/// <summary>
/// Compact hash set.
/// Preserves the order of items.
/// </summary>
/// <typeparam name="E">type of elements.</typeparam>
public class ImmHashSet<E> : ImmSet<E>
{
    private static readonly EqualityComparer<E> comparer = EqualityComparer<E>.Default;

    private readonly HashTableEntry[] hashTable;

    public ImmHashSet(E[] array, bool copy) : base(array, copy)
    {
        HashTableLogic.BuildHasTable<E,E>(this.segment, e => e, comparer, out hashTable);
    }

    internal ImmHashSet(ArraySegment<E> segment) : base(segment)
    {
        HashTableLogic.BuildHasTable<E,E>(this.segment, e => e, comparer, out hashTable);
    }

    public override int IndexOf(E item)
    {
        return HashTableLogic.FindIndex(segment, hashTable, e => e, comparer, item);
    }

    public override int LastIndexOf(E item) => IndexOf(item);

    protected override string CollectionWord => "HashSet";
}

