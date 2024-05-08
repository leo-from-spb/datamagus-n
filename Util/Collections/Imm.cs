using System.Collections.Generic;

namespace Util.Collections;

public static class Imm
{

    /// <summary>
    /// Creates an immutables dictionary with entries from the given <paramref name="originalDictionary"/>, with a copy of all its entries.
    /// <br/>
    /// If the given <paramref name="originalDictionary"/> is already an instance of <see cref="ImmDictionary{K,V}"/>>, returns it as is.
    /// </summary>
    /// <param name="originalDictionary">the original dictionary.</param>
    /// <typeparam name="K">type of key.</typeparam>
    /// <typeparam name="V">type of value.</typeparam>
    /// <returns>the immutable dictionary.</returns>
    public static ImmDictionary<K, V> Dictionary<K, V>(IReadOnlyDictionary<K,V> originalDictionary)
    {
        if (originalDictionary is ImmDictionary<K, V> originalImmDictionary) return originalImmDictionary;

        int n = originalDictionary.Count;
        return n switch
               {
                   0    => ImmEmptyDictionary<K, V>.Empty,
                   <= 4 => new ImmMicroDictionary<K, V>(originalDictionary),
                   _    => new ImmCompactHashDictionary<K, V>(originalDictionary, n)
               };
    }




    /// <summary>
    /// Creates an immutable snapshot of this dictionary.
    /// The original one remains unchanged.
    /// <br/>
    /// If this is already an instance of <see cref="ImmDictionary{K,V}"/>>, returns this as is.
    /// <br/>
    /// Otherwise, a new instance is created and all entries from the original dictionary are copied.
    /// In this case, the created instance doesn't reference the original one.
    /// </summary>
    /// <param name="originalDictionary">the original dictionary.</param>
    /// <typeparam name="K">type of key.</typeparam>
    /// <typeparam name="V">type of value.</typeparam>
    /// <returns>the immutable dictionary.</returns>
    public static ImmDictionary<K, V> ToImm<K, V>(this IReadOnlyDictionary<K, V> originalDictionary) => Dictionary(originalDictionary);


}
