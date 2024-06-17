using System;
using System.Collections;
using System.Collections.Generic;

namespace Util.Collections;

public static class Empties
{

    public class EmptyCollection<T> : IReadOnlyCollection<T>
    {
        public int Count => 0;

        public IEnumerator<T> GetEnumerator() => new EmptyEnumerator<T>();

        IEnumerator IEnumerable.GetEnumerator() => new EmptyEnumerator<object>();
    }


    public class EmptyEnumerable<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new EmptyEnumerator<T>();

        IEnumerator IEnumerable.GetEnumerator() => new EmptyEnumerator<object>();
    }


    public class EmptyEnumerator<T> : IEnumerator<T>
    {
        public bool MoveNext() => false;
        public T    Current    => throw new InvalidOperationException("Attempted to iterate over an empty collection.");
        public void Reset()    { }
        public void Dispose()  { }

        #pragma warning disable CS8603 // Possible null reference return.
        object IEnumerator.Current => Current;

    }


}
