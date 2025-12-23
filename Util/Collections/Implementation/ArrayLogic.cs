using System;
using System.Collections.Generic;

namespace Util.Collections.Implementation;

internal class ArrayLogic
{

    internal static KeyValuePair<K,V>[] PreparePairs<K,V>(ReadOnlySpan<V> values, Func<V,K> keySelector)
    {
        int n = values.Length;
        KeyValuePair<K,V>[] result = new KeyValuePair<K,V>[n];

        for (int i = 0; i < n; i++)
        {
            V value = values[i];
            K key = keySelector(value);
            result[i] = new KeyValuePair<K,V>(key, value);
        }

        return result;
    }
    
    internal static KeyValuePair<K,V>[] PreparePairs<K,V>(IReadOnlyList<V> values, Func<V,K> keySelector)
    {
        int                 n      = values.Count;
        KeyValuePair<K,V>[] result = new KeyValuePair<K,V>[n];

        for (int i = 0; i < n; i++)
        {
            V value = values[i];
            K key   = keySelector(value);
            result[i] = new KeyValuePair<K,V>(key, value);
        }

        return result;
    }

    internal static KeyValuePair<K,V>[] PreparePairs<K,V>(IEnumerable<V> values, Func<V,K> keySelector)
    {
        IReadOnlyList<V> valuesList =
            values switch
            {
                IReadOnlyList<V> alreadyList => alreadyList,
                _                            => values.ToImmList()
            };
        return PreparePairs(valuesList, keySelector);
    }

    internal static KeyValuePair<K,V>[] PreparePairs<E,K,V>(ReadOnlySpan<E> elements, Func<E,K> keySelector, Func<E,V> valueSelector)
    {
        int n = elements.Length;
        KeyValuePair<K,V>[] result = new KeyValuePair<K,V>[n];

        for (int i = 0; i < n; i++)
        {
            E element = elements[i];
            K key = keySelector(element);
            V value = valueSelector(element);
            result[i] = new KeyValuePair<K,V>(key, value);
        }

        return result;
    }

    internal static KeyValuePair<K,V>[] PreparePairs<E,K,V>(IReadOnlyList<E> elements, Func<E,K> keySelector, Func<E,V> valueSelector)
    {
        int n = elements.Count;
        KeyValuePair<K,V>[] result = new KeyValuePair<K,V>[n];

        for (int i = 0; i < n; i++)
        {
            E element = elements[i];
            K key = keySelector(element);
            V value = valueSelector(element);
            result[i] = new KeyValuePair<K,V>(key, value);
        }

        return result;
    }

    internal static KeyValuePair<K,V>[] PreparePairs<E,K,V>(IEnumerable<E> elements, Func<E,K> keySelector, Func<E,V> valueSelector)
    {
        IReadOnlyList<E> elementList =
            elements switch
            {
                IReadOnlyList<E> alreadyList => alreadyList,
                _                            => elements.ToImmList()
            };
        return PreparePairs(elementList, keySelector, valueSelector);
    }

}
