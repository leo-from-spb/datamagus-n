using System;
using System.Collections;
using System.Collections.Generic;

namespace Util.Collections.Implementation;


public class ImmutableSingleton<T> : ImmutableCollection<T>, ImmOrderedSet<T>
{
    /// <summary>
    /// The element.
    /// </summary>
    private readonly T element;

    /// <summary>
    /// Trivial constructor.
    /// </summary>
    /// <param name="element">the element to hold.</param>
    public ImmutableSingleton(T element)
    {
        this.element = element;
    }

    public int  Count      => 1;
    public bool IsNotEmpty => true;
    public bool IsEmpty    => false;
    public T    First      => element;
    public T    Last       => element;

    public T At(int index)
    {
        if (index == 0) return element;
        else throw new IndexOutOfRangeException($"Requested an element at index {index} when the collection has one element only.");
    }

    public T this[int index] => At(index);

    public bool Contains(T element) => eq.Equals(this.element, element);

    public bool Contains(Predicate<T> predicate) => predicate(this.element);

    public Found<T> Find(Predicate<T> predicate) =>
        predicate(this.element)
            ? new Found<T>(true, element)
            : Found<T>.NotFound;

    public Found<T> FindFirst(Predicate<T> predicate, int fromIndex = 0) =>
        fromIndex == 0 && predicate(this.element)
            ? new Found<T>(true, element)
            : Found<T>.NotFound;

    public Found<T> FindLast(Predicate<T> predicate) => Find(predicate);

    public int IndexOf(T element, int notFound = int.MinValue) => eq.Equals(this.element, element) ? 0 : notFound;

    public int LastIndexOf(T element, int notFound = int.MinValue) => IndexOf(element, notFound);

    public IEnumerator<T>   GetEnumerator() => new ImmuatbleSingletonEnumerator<T>(element);
    IEnumerator IEnumerable.GetEnumerator() => new ImmuatbleSingletonEnumerator<T>(element);

}



public class ImmutableSortedSingleton<T> : ImmutableSingleton<T>, ImmSortedSet<T>
    where T: IComparable<T>
{
    public ImmutableSortedSingleton(T element) : base(element) { }
}


internal class ImmuatbleSingletonEnumerator<T> : IEnumerator<T>
{
    private readonly T element;

    /// <summary>
    /// Position:
    /// 0 - before the element,
    /// 1 - on the element;
    /// 2 - after the element.
    /// </summary>
    private byte state = 0;

    public ImmuatbleSingletonEnumerator(T element)
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
