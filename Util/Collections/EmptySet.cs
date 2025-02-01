using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

/// <summary>
/// Empty set and list.
/// </summary>
public class EmptySet<T> : Immutable, ImmOrderedSet<T>
{
    public static readonly EmptySet<T> Instance = new();

    public Immutable Imp => this;

    public bool IsNotEmpty => false;
    public bool IsEmpty    => true;
    public int  Count      => 0;

    public Found<T> Find(Predicate<T>      predicate)                    => Found<T>.NotFound;
    public Found<T> FindFirst(Predicate<T> predicate, int fromIndex = 0) => Found<T>.NotFound;
    public Found<T> FindLast(Predicate<T>  predicate)                    => Found<T>.NotFound;

    public bool Contains(T            item)      => false;
    public bool Contains(Predicate<T> predicate) => false;

    public T At(int   index) => throw new IndexOutOfRangeException("Collection is empty");
    public T this[int index] => throw new IndexOutOfRangeException("Collection is empty");

    public T First => throw new IndexOutOfRangeException("Collection is empty");
    public T Last  => throw new IndexOutOfRangeException("Collection is empty");

    public int IndexOf(T     element, int notFound = int.MinValue) => notFound;
    public int LastIndexOf(T element, int notFound = int.MinValue) => notFound;

    IEnumerator IEnumerable.GetEnumerator() => Array.Empty<T>().GetEnumerator();
    public IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();
}



/// <summary>
/// Empty sorted set.
/// </summary>
public class EmptySortedSet<T> : EmptySet<T>, ImmSortedSet<T>
    where T : IComparable<T>
{
    public new static readonly EmptySortedSet<T> Instance = new();
}
