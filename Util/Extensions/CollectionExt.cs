using System.Collections.Generic;

namespace Util.Extensions;


/// <summary>
/// Utility functions for working with collections.
/// </summary>
public static class CollectionExt
{
    /// <param name="collection">the collection to check.</param>
    extension<E>(IReadOnlyCollection<E> collection)
    {
        /// <summary>
        /// Checks whether this collection is empty.
        /// </summary>
        /// <returns>true when empty.</returns>
        public bool IsEmpty() => collection.Count == 0;

        /// <summary>
        /// Checks whether this collection contains something (is not empty).
        /// </summary>
        /// <returns>true when it is not empty.</returns>
        public bool IsNotEmpty() => collection.Count > 0;
    }

}
