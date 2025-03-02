using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Util.Collections.ImmConst;
using static Util.Collections.Implementation.CollectionLogic;


namespace Util.Collections.Implementation;


public class ImmutableSingleton<T> : ImmutableCollection<T>, ImmListSet<T>
{
    /// <summary>
    /// The element.
    /// </summary>
    private readonly T Element;

    protected override string CollectionWord => "Singleton";

    /// <summary>
    /// Trivial constructor.
    /// </summary>
    /// <param name="element">the element to hold.</param>
    public ImmutableSingleton(T element)
    {
        this.Element = element;
    }

    public override int Count => 1;

    public bool IsNotEmpty => true;
    public bool IsEmpty    => false;
    public T    First      => Element;
    public T    Last       => Element;

    public T At(int index)
    {
        if (index == 0) return Element;
        else throw new IndexOutOfRangeException($"Requested an element at index {index} when the collection has one element only.");
    }

    public T this[int index] => At(index);

    public bool Contains(T element) => eq.Equals(this.Element, element);

    public bool Contains(Predicate<T> predicate) => predicate(this.Element);

    public Found<T> Find(Predicate<T> predicate) =>
        predicate(this.Element)
            ? new Found<T>(true, Element)
            : Found<T>.NotFound;

    public Found<T> FindFirst(Predicate<T> predicate, int fromIndex = 0) =>
        fromIndex == 0 && predicate(this.Element)
            ? new Found<T>(true, Element)
            : Found<T>.NotFound;

    public Found<T> FindLast(Predicate<T> predicate) => Find(predicate);

    public int IndexOf(T     element)               => eq.Equals(this.Element, element) ? 0 : notFoundIndex;
    public int IndexOf(T     element, int notFound) => eq.Equals(this.Element, element) ? 0 : notFound;
    public int LastIndexOf(T element)               => eq.Equals(this.Element, element) ? 0 : notFoundIndex;
    public int LastIndexOf(T element, int notFound) => eq.Equals(this.Element, element) ? 0 : notFound;

    public bool IsSubsetOf(IEnumerable<T>         other) => IsTheSingletonSubsetOf(Element, eq, other, false);
    public bool IsProperSubsetOf(IEnumerable<T>   other) => IsTheSingletonSubsetOf(Element, eq, other, true);
    public bool IsProperSupersetOf(IEnumerable<T> other) => IsTheSingletonSupersetOf(Element, eq, other, true);
    public bool IsSupersetOf(IEnumerable<T>       other) => IsTheSingletonSupersetOf(Element, eq, other, false);
    public bool Overlaps(IEnumerable<T>           other) => other.Contains(Element);
    public bool SetEquals(IEnumerable<T>          other) => IsTheSingletonEqualTo(Element, eq, other);

    public override IEnumerator<T> GetEnumerator() => new ImmutableSingletonEnumerator<T>(Element);

}



public class ImmutableSortedSingleton<T> : ImmutableSingleton<T>, ImmSortedListSet<T>
    where T: IComparable<T>
{
    protected override string CollectionWord => "SortedSingleton";

    public ImmutableSortedSingleton(T element) : base(element) { }
}


internal class ImmutableSingletonEnumerator<T> : IEnumerator<T>
{
    private readonly T element;

    /// <summary>
    /// Position:
    /// 0 - before the element,
    /// 1 - on the element;
    /// 2 - after the element.
    /// </summary>
    private byte state = 0;

    public ImmutableSingletonEnumerator(T element)
    {
        this.element = element;
    }

    public void Reset()
    {
        state = 0;
    }

    public bool MoveNext()
    {
        if (state == 0 || state == 1) state++;
        return state == 1;
    }

    object? IEnumerator.Current => Current;

    public T Current => state == 1 ? element : default!;

    public void Dispose() { }

}
