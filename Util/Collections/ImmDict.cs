using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Util.Collections;


/// <summary>
/// Immutable dictionary.
/// </summary>
public interface ImmDict<K,V> : IReadOnlyDictionary<K,V>
{

    public bool IsNotEmpty { get; }

    public bool IsEmpty { get; }

    public Found<V> Find(K key);

    public V Find(K key, V notFound) =>
        #pragma warning disable CS8604
        Find(key) | notFound;
        #pragma warning restore CS8604

    bool IReadOnlyDictionary<K,V>.TryGetValue(K key, [MaybeNullWhen(false)] out V value)
    {
        var f = Find(key);
        value = f.Item;
        return f.Ok;
    }

    V IReadOnlyDictionary<K, V>.this[K key] => Find(key).Item;


    public new ImmSet<K> Keys { get; }

    public new ImmCollection<V> Values { get; }

    public ImmCollection<KeyValuePair<K,V>> Entries { get; }


    IEnumerable<K> IReadOnlyDictionary<K, V>.Keys   => Keys;
    IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

    IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() => Entries.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Entries.GetEnumerator();
}


