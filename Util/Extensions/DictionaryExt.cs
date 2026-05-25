using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Util.Extensions;


/// <summary>
/// Utility functions for working with dictionaries.
/// </summary>
public static class DictionaryExt
{
    /// <param name="dictionary">the dictionary to get from.</param>
    /// <typeparam name="K">key type.</typeparam>
    /// <typeparam name="V">value type.</typeparam>
    extension<K,V>(IDictionary<K,V> dictionary)
    {
        /// <summary>
        /// Safe get method: find the given key and return the corresponded value
        /// or the given <paramref name="missing"/> value when the key is not found.
        /// </summary>
        /// <param name="key">the key to find.</param>
        /// <param name="missing">returns when the key not found.</param>
        /// <returns>found value or the specified <paramref name="missing"/> value when the key is not found.</returns>
        public V Get(K key, V missing)
        {
            bool ok = dictionary.TryGetValue(key, out var result);
            return ok ? result! : missing;
        }

        /// <summary>
        /// Safe get method: find the given key and return the corresponded value
        /// or the default of the value type <typeparamref name="V"/> value when the key is not found.
        /// </summary>
        /// <param name="key">the key to find.</param>
        /// <returns>found value or the default one when the key is not found.</returns>
        public V? Get(K key)
        {
            bool ok = dictionary.TryGetValue(key, out var result);
            return ok ? result : default(V);
        }
    }

}
