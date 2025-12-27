using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Util.Fun.NumberConstants;

namespace Util.Collections.Implementation;

/// <summary>
/// Empty set and list.
/// </summary>
public class EmptySet<T> : ImmutableCollection<T>, ImmListSet<T>
{
    public static readonly EmptySet<T> Instance = new();

    internal override byte CascadingLevel => _0_;

    protected override string CollectionWord => "EmptySet";

    public bool IsNotEmpty => false;
    public bool IsEmpty    => true;

    public override int Count => 0;

    public Found<T> Find(Predicate<T>      predicate)                    => Found<T>.NotFound;
    public Found<T> FindFirst(Predicate<T> predicate, int fromIndex = 0) => Found<T>.NotFound;
    public Found<T> FindLast(Predicate<T>  predicate)                    => Found<T>.NotFound;

    public bool Contains(T            item)      => false;
    public bool Contains(Predicate<T> predicate) => false;

    public T At(int   index) => throw new IndexOutOfRangeException("Collection is empty");
    public T this[int index] => throw new IndexOutOfRangeException("Collection is empty");

    public T First => throw new IndexOutOfRangeException("Collection is empty");
    public T Last  => throw new IndexOutOfRangeException("Collection is empty");

    public int IndexOf(T     element, int notFound) => notFound;
    public int LastIndexOf(T element, int notFound) => notFound;

    public bool IsProperSubsetOf(IEnumerable<T>   other) => other.IsNotEmpty();
    public bool IsSubsetOf(IEnumerable<T>         other) => true;
    public bool IsProperSupersetOf(IEnumerable<T> other) => false;
    public bool IsSupersetOf(IEnumerable<T>       other) => other.IsEmpty();

    public bool Overlaps(IEnumerable<T>  other) => false;
    public bool SetEquals(IEnumerable<T> other) => other.IsEmpty();

    public override IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Array.Empty<T>().GetEnumerator();

    public override string ToString() => CollectionWord;
}



/// <summary>
/// Empty sorted set.
/// </summary>
public class EmptySortedSet<T> : EmptySet<T>, ImmSortedListSet<T>
    where T : IComparable<T>
{
    protected override string CollectionWord => "EmptySortedSet";

    public new static readonly EmptySortedSet<T> Instance = new();
}
