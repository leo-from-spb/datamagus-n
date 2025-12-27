using System;
using System.Collections.Generic;
using Util.Collections.Implementation;

namespace Util.Collections;


/// <summary>
/// Immutable dictionary builder.
/// </summary>
/// <typeparam name="K">type fo the key.</typeparam>
/// <typeparam name="V">type of the value.</typeparam>
public sealed class ImmDictBuilder<K,V>
    where K: IEquatable<K>
{
    private readonly List<KeyValuePair<K,V>> Entries = new();

    public int Count => Entries.Count;


    public ImmDictBuilder<K,V> Add(K key, V value)
    {
        Entries.Add(new KeyValuePair<K,V>(key, value));
        return this;
    }

    public ImmDictBuilder<K,V> Add(K key1, V value1, K key2, V value2)
    {
        Entries.Add(new KeyValuePair<K,V>(key1, value1));
        Entries.Add(new KeyValuePair<K,V>(key2, value2));
        return this;
    }

    public ImmDictBuilder<K,V> Add(K key1, V value1, K key2, V value2, K key3, V value3)
    {
        Entries.Add(new KeyValuePair<K,V>(key1, value1));
        Entries.Add(new KeyValuePair<K,V>(key2, value2));
        Entries.Add(new KeyValuePair<K,V>(key3, value3));
        return this;
    }

    public ImmDictBuilder<K,V> Add(KeyValuePair<K,V> entry)
    {
        Entries.Add(entry);
        return this;
    }

    public ImmDictBuilder<K,V> Add(params KeyValuePair<K,V>[] entries)
    {
        Entries.AddRange(entries);
        return this;
    }

    /// <summary>
    /// Builds an immutable dictionary with the prepared data.
    /// </summary>
    /// <returns>the built immutable dictionary.</returns>
    public ImmListDict<K,V> Build()
    {
        var entriesArray = Entries.ToArray();
        return ImmutableArrayDictionary<K,V>.MakeListDict(entriesArray, true);
    }

}
