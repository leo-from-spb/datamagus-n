namespace Util.Common.Fun;


/// <summary>
/// Utility functions for working with dictionaries.
/// </summary>
public static class DictionaryExt
{

    /// <summary>
    /// Safe get method: find the given key and return the corresponded value
    /// or the given <paramref name="missing"/> value when the key is not found.
    /// </summary>
    /// <param name="dictionary">the dictionary to get from.</param>
    /// <param name="key">the key to find.</param>
    /// <param name="missing">returns when the key not found.</param>
    /// <typeparam name="K">key type.</typeparam>
    /// <typeparam name="V">value type.</typeparam>
    /// <returns>found value or the specified <paramref name="missing"/> value when the key is not found.</returns>
    public static V Get<K, V>(this IDictionary<K, V> dictionary, K key, V missing)
    {
        bool ok = dictionary.TryGetValue(key, out var result);
        return ok ? result! : missing;
    }

    /// <summary>
    /// Safe get method: find the given key and return the corresponded value
    /// or the default of the value type <typeparamref name="V"/> value when the key is not found.
    /// </summary>
    /// <param name="dictionary">the dictionary to get from.</param>
    /// <param name="key">the key to find.</param>
    /// <typeparam name="K">key type.</typeparam>
    /// <typeparam name="V">value type.</typeparam>
    /// <returns>found value or the default one when the key is not found.</returns>
    public static V? Get<K, V>(this IDictionary<K, V> dictionary, K key)
    {
        bool ok = dictionary.TryGetValue(key, out var result);
        return ok ? result : default(V);
    }

}
