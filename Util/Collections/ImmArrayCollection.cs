using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Collections;


/// <summary>
/// Array-based immutable collection.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public abstract class ImmArrayCollection<T> : ImmCollection<T>
{
    protected readonly T[] Elements;
    protected readonly int N;


    internal ImmArrayCollection(T[] elements, bool copy)
    {
        if (copy)
        {
            N = elements.Length;
            Elements = new T[N];
            elements.CopyTo(Elements, 0);
        }
        else
        {
            Elements = elements;
            N        = elements.Length;
        }
    }

    internal ImmArrayCollection(int n)
    {
        this.Elements = new T[n];
        this.N        = n;
    }


    public override int  Count      => N;
    public override bool IsNotEmpty => N != 0;
    public override bool IsEmpty    => N == 0;

    public override bool Contains(T element)
    {
        foreach (T e in Elements)
            if (Eqr.Equals(e, element))
                return true;
        return false;
    }

    public override bool Contains(Predicate<T> predicate)
    {
        foreach (T e in Elements)
            if (predicate(e))
                return true;
        return false;
    }

    public override IEnumerator<T> GetEnumerator() => Elements.AsEnumerable().GetEnumerator();

    public override string ToString()
    {
        string cls = this.GetType().Name;
        if (IsEmpty) return $"Empty {cls}";

        string parens = this switch
                        {
                            LSet<T> _  => "{}",
                            LList<T> _ => "[]",
                            _          => "()"
                        };
        var word = N == 1 ? " element) = " : " elements) = ";

        var b = new StringBuilder();
        b.Append(cls).Append('(').Append(N).Append(word);
        b.Append(parens, 0, 1);

        bool was = false;
        foreach (T e in Elements)
        {
            b.Append(e is not null ? e.ToString() : "null");
            if (was) b.Append(", ");
            else was = true;
        }

        b.Append(parens, 1, 1);
        return b.ToString();
    }
}


/// <summary>
/// Immutable list.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public sealed class ImmList<T> : ImmArrayCollection<T>, RList<T>
{

    public ImmList(IEnumerable<T> elements)
        : base(elements.ToArray(), false)
    { }

    internal ImmList(T[] elements, bool copy)
        : base(elements, copy)
    { }


    public T First => N > 0
        ? Elements[0]
        : throw new IndexOutOfRangeException($"Attempted to get the first element from an empty list");

    public T Last => N > 0
        ? Elements[N - 1]
        : throw new IndexOutOfRangeException($"Attempted to get the last element from an empty list");

    public T At(int index) => 0 <= index && index < N
        ? Elements[index]
        : throw new IndexOutOfRangeException($"Attempted to get the element {index} from a list of {N} elements");

    public T this[int index] => At(index);

    public int IndexOf(T element, int notFound = Int32.MinValue)
    {
        for (int i = 0; i < N; i++)
            if (Eqr.Equals(Elements[i], element))
                return i;
        return notFound;
    }

    public int LastIndexOf(T element, int notFound = Int32.MinValue)
    {
        for (int i = N - 1; i >= 0; i--)
            if (Eqr.Equals(Elements[i], element))
                return i;
        return notFound;
    }

    public int IndexOf(Predicate<T> predicate, int notFound = Int32.MinValue)
    {
        for (int i = 0; i < N; i++)
            if (predicate(Elements[i]))
                return i;
        return notFound;
    }

    public int LastIndexOf(Predicate<T> predicate, int notFound = Int32.MinValue)
    {
        for (int i = N - 1; i >= 0; i--)
            if (predicate(Elements[i]))
                return i;
        return notFound;
    }
}