using System.Collections;
using System.Collections.Generic;

namespace Util.Collections;


/// <summary>
/// Immutable thing.
/// </summary>
public abstract class Immutable
{

}


/// <summary>
/// Base class for every immutable collection class.
/// </summary>
/// <typeparam name="T">type of the element.</typeparam>
public abstract class ImmutableCollection<T> : Immutable, IReadOnlyCollection<T>
{
    /// <summary>
    /// The essence word in the name of this collection.
    /// </summary>
    protected abstract string CollectionWord { get; }

    /// <summary>
    /// The equality comparer that should be used for check on equality.
    /// </summary>
    protected static readonly EqualityComparer<T> eq = EqualityComparer<T>.Default;

    /// <summary>
    /// The constructor is for internal usage only.
    /// </summary>
    internal ImmutableCollection() { }

    /// <summary>
    /// This property is for internal usage only.
    /// </summary>
    public ImmutableCollection<T> Imp => this;

    /// <summary>
    /// Count of elements in the collection.
    /// </summary>
    public abstract int Count { get; }

    /// <summary>
    /// Makes the enumerator, which supports a simple iteration over this collection.
    /// </summary>
    /// <returns>the enumerator.</returns>
    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() =>
        $"An immutable {CollectionWord} of {Count} elements";
}


/// <summary>
/// Base class for every immutable dictionary class.
/// </summary>
/// <typeparam name="K">type of the key.</typeparam>
/// <typeparam name="V">type of the value.</typeparam>
public abstract class ImmutableDictionary<K,V> : Immutable
{
    /// <summary>
    /// The essence word in the name of this dictionary.
    /// </summary>
    protected abstract string DictionaryWord { get; }

    /// <summary>
    /// The equality comparer for keys.
    /// </summary>
    protected static readonly EqualityComparer<K> keyEq = EqualityComparer<K>.Default;

    /// <summary>
    /// The equality comparer for keys.
    /// </summary>
    protected static readonly EqualityComparer<V> valueEq = EqualityComparer<V>.Default;

    /// <summary>
    /// Count of entries (key-value pairs) in this collection.
    /// </summary>
    public abstract int Count { get; }

    public override string ToString() =>
        $"An immutable {DictionaryWord} of {Count} elements";
}