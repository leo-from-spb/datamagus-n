using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    protected readonly int Offset;
    protected readonly int Limit;
    protected readonly int N;

    protected readonly ArraySegment<T> ElementsSegment;


    internal ImmArrayCollection(T[] elements, bool copy)
    {
        N = elements.Length;

        if (copy)
        {
            Elements = new T[N];
            elements.CopyTo(Elements, 0);
        }
        else
        {
            Elements = elements;
        }

        ElementsSegment = Elements;
        Offset          = 0;
        Limit           = N;
    }

    internal ImmArrayCollection(T[] elements, int offset, int limit, bool copy)
    {
        Debug.Assert(limit <= elements.Length);
        Debug.Assert(offset <= limit);
        N = limit - offset;

        if (copy)
        {
            Elements = new T[N];
            Array.Copy(elements, offset, Elements, 0, N);
            ElementsSegment = Elements;
            Offset          = 0;
            Limit           = N;
        }
        else
        {
            Elements        = elements;
            ElementsSegment = new ArraySegment<T>(Elements, offset, N);
            Offset          = offset;
            Limit           = limit;
        }
    }

    internal ImmArrayCollection(int n)
    {
        Elements = new T[n];
        N        = n;
        Offset   = 0;
        Limit    = n;

        ElementsSegment = Elements;
    }



    public override int  Count      => N;
    public override bool IsNotEmpty => N != 0;
    public override bool IsEmpty    => N == 0;

    public override bool Contains(T element)
    {
        for (int i = Offset; i < Limit; i++)
            if (Eqr.Equals(Elements[i], element))
                return true;

        return false;
    }

    public override bool Contains(Predicate<T> predicate)
    {
        for (int i = Offset; i < Limit; i++)
            if (predicate(Elements[i]))
                return true;
        return false;
    }

    public override IEnumerator<T> GetEnumerator() => ElementsSegment.GetEnumerator();

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
        for (int i = Offset; i < Limit; i++)
        {
            T e = Elements[i];
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

    internal ImmList(T[] elements, int offset, int limit, bool copy)
        : base(elements, offset, limit, copy)
    { }


    public T First => N > 0
        ? Elements[Offset]
        : throw new IndexOutOfRangeException($"Attempted to get the first element from an empty list");

    public T Last => N > 0
        ? Elements[Limit - 1]
        : throw new IndexOutOfRangeException($"Attempted to get the last element from an empty list");

    public T At(int index) => 0 <= index && index < N
        ? Elements[Offset + index]
        : throw new IndexOutOfRangeException($"Attempted to get the element {index} from a list of {N} elements");

    public T this[int index] => At(index);

    public int IndexOf(T element, int notFound = Int32.MinValue)
    {
        for (int i = Offset; i < Limit; i++)
            if (Eqr.Equals(Elements[i], element))
                return i - Offset;
        return notFound;
    }

    public int LastIndexOf(T element, int notFound = Int32.MinValue)
    {
        for (int i = Limit - 1; i >= Offset; i--)
            if (Eqr.Equals(Elements[i], element))
                return i - Offset;
        return notFound;
    }

    public int IndexOf(Predicate<T> predicate, int notFound = Int32.MinValue)
    {
        for (int i = Offset; i < Limit; i++)
            if (predicate(Elements[i]))
                return i - Offset;
        return notFound;
    }

    public int LastIndexOf(Predicate<T> predicate, int notFound = Int32.MinValue)
    {
        for (int i = Limit - 1; i >= Offset; i--)
            if (predicate(Elements[i]))
                return i - Offset;
        return notFound;
    }
}