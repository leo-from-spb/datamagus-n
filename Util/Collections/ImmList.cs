using System;
using System.Collections;
using System.Collections.Generic;

namespace Util.Collections;

/// <summary>
/// Immutable list.
/// </summary>
/// <typeparam name="E">type of elements.</typeparam>
/// <seealso cref="Imm"/>
public class ImmList<E> : IReadOnlyList<E>
{
    /// <summary>
    /// Array of elements.
    /// </summary>
    protected readonly ArraySegment<E> segment;

    /// <summary>
    /// Number of elements in this list (the back-field).
    /// </summary>
    protected readonly int count;


    internal ImmList(E[] array, bool copy)
    {
        count = array.Length;
        E[] theArray;
        if (copy)
        {
            theArray = new E[count];
            Array.Copy(array, theArray, count);
        }
        else
        {
            theArray = array;
        }
        segment = new ArraySegment<E>(theArray);
    }

    internal ImmList(ArraySegment<E> segment)
    {
        this.segment = segment;
        this.count = this.segment.Count;
    }


    /// <summary>
    /// Number of elements in this list.
    /// </summary>
    public int Count => count;

    /// <summary>
    /// Whether this list is not empty.
    /// </summary>
    public bool IsNotEmpty => count > 0;

    /// <summary>
    /// Whether this list is empty.
    /// </summary>
    public bool IsEmpty => count == 0;

    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index">index, zero-based.</param>
    /// <exception cref="IndexOutOfRangeException">when the index is wrong or the collection is empty.</exception>
    public E At(int index) =>
        index >= 0 && index < count
            ? segment[index]
            : throw new IndexOutOfRangeException(IsNotEmpty
                                                     ? $"Index value {index} is out of range (the list size is {count})."
                                                     : $"Index value {index} is out of range (the list is empty).");

    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index">index, zero-based.</param>
    E IReadOnlyList<E>.this[int index] => At(index);


    /// <summary>
    /// The first element.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty</exception>
    /// <seealso cref="Last"/>
    public E First =>
        IsNotEmpty
            ? segment[0]
            : throw new IndexOutOfRangeException("Cannot get the first element because the list is empty.");

    /// <summary>
    /// The last element.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty</exception>
    /// <seealso cref="First"/>
    public E Last =>
        IsNotEmpty
            ? segment[count - 1]
            : throw new IndexOutOfRangeException("Cannot get the first element because the list is empty.");

    /// <summary>
    /// Finds the first occurrence of the given item in the list.
    /// If the item is found, returns its index (zero-based), otherwise returns -1.
    /// </summary>
    /// <param name="item">the item to find.</param>
    /// <returns>index of the item, or -1 when not found.</returns>
    public virtual int IndexOf(E item)
    {
        for (int i = 0; i < count; i++)
            if (segment[i].Equals(item))
                return i;
        return -1;
    }

    /// <summary>
    /// Finds the last occurrence of the given item in the list.
    /// If the item is found, returns its index (zero-based), otherwise returns -1.
    /// </summary>
    /// <param name="item">the item to find.</param>
    /// <returns>index of the item, or -1 when not found.</returns>
    public virtual int LastIndexOf(E item)
    {
        for (int i = count - 1; i >= 0; i--)
            if (segment[i].Equals(item))
                return i;
        return -1;
    }

    /// <summary>
    /// Checks whether the given item presents in the list.
    /// </summary>
    /// <param name="item">the item to check.</param>
    public bool Contains(E item) => IndexOf(item) != -1;

    /// <summary>
    /// Returns a slice of this list.
    /// The slice shares the same underlying array (no copy is made).
    /// </summary>
    /// <param name="offset">the offset (zero-based).</param>
    /// <param name="count">the number of elements.</param>
    public ImmList<E> Slice(int offset, int count)
    {
        if (offset == 0 && count == this.count) return this;
        return new ImmList<E>(segment.Slice(offset, count));
    }


    /// <summary>
    /// Creates an enumerator.
    /// </summary>
    public IEnumerator<E> GetEnumerator() => segment.GetEnumerator();

    /// <summary>
    /// Creates an enumerator.
    /// </summary>
    /// <seealso cref="GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => segment.GetEnumerator();


    public sealed override string ToString()
    {
        return $"{CollectionWord} of {Count} elements";
    }

    protected virtual string CollectionWord => "List";

}




