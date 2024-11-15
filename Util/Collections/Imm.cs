using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;


/// <summary>
/// Methods to make immutable collections.
/// </summary>
public static class Imm
{

    /// <summary>
    /// Create a snapshot of this array.
    /// </summary>
    /// <param name="array">the original array.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the snapshot.</returns>
    public static ImmList<E> ToImmList<E>(this E[] array) => new ImmList<E>(array, true);

    /// <summary>
    /// Create a snapshot of this collection.
    /// </summary>
    /// <param name="collection">the original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    /// <returns>the snapshot.</returns>
    public static ImmList<E> ToImmList<E>(this IReadOnlyCollection<E> collection) => new ImmList<E>(collection.ToArray(), false);

}
