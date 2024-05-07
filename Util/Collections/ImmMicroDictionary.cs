using System.Collections.Generic;

namespace Util.Collections;

/// <summary>
/// A dictionary for very small amount of key-value pairs. Find complexity is O(n).
/// </summary>
/// <typeparam name="K">type of key.</typeparam>
/// <typeparam name="V">type of value.</typeparam>
public sealed class ImmMicroDictionary<K,V> : ImmArrayDictionary<K,V>
{
    public ImmMicroDictionary(KeyValuePair<K,V>[] array)
        : base(array, toCopy: true) { }


    protected override int FindEntryIndex(K key)
    {
        var h = HashOf(key);
        for (int i = Offset; i < Limit; i++)
        {
            K ki = EntriesArray[i].Key;
            if (HashOf(ki) == h && comparer.Equals(ki, key)) return i;
        }
        return int.MinValue;
    }

}
