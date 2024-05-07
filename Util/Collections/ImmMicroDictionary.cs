using System.Collections.Generic;

namespace Util.Collections;

/// <summary>
/// A dictionary for very small amount of key-value pairs. Find complexity is O(n).
/// </summary>
/// <typeparam name="K">type of key.</typeparam>
/// <typeparam name="V">type of value.</typeparam>
public class ImmMicroDictionary<K,V> : ImmArrayDictionary<K,V>
{
    public ImmMicroDictionary(KeyValuePair<K,V>[] array)
        : base(array, toCopy: true) { }


    private int FindIndex(K key)
    {
        #nullable disable
        var h = comparer.GetHashCode(key);
        for (int i = Offset; i < Limit; i++)
        {
            K ki = EntriesArray[i].Key;
            if (comparer.GetHashCode(ki) == h && comparer.Equals(ki, key)) return i;
        }
        #nullable restore
        return int.MinValue;
    }


    public override bool ContainsKey(K key) => FindIndex(key) >= 0;

    public override Found<V> Find(K key)
    {
        int index = FindIndex(key);
        #nullable disable
        V   value  = index >= 0 ? EntriesArray[index].Value : default(V);
        var result = new Found<V>(index >= 0, value);
        #nullable restore
        return result;
    }

    public override V Get(K key,
                          #nullable disable
                          V noValue = default(V))
                          #nullable restore
    {
        int index = FindIndex(key);
        return index >= 0 ? EntriesArray[index].Value : noValue;
    }

}
