using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Util.Extensions;


/// <summary>
/// Utility functions for working with collections.
/// </summary>
public static class CollectionExt
{
    extension<E>([NotNullWhen(true)] IReadOnlyCollection<E>? collection)
    {
        /// <summary>
        /// Checks whether this collection contains something.
        /// </summary>
        /// <seealso cref="Nothing"/>
        [OverloadResolutionPriority(10)]
        public bool Some => collection is not null && collection.Count != 0;
    }


    extension<E>(IReadOnlyCollection<E>? collection)
    {
        /// <summary>
        /// Checks whether this collection is null or empty.
        /// <p>
        /// The difference between <c>Nothing</c> and <c>IsEmpty</c> is
        /// that <c>Nothing</c> checks the collection for null but <c>IsEmpty</c> requires a non-null reference.
        /// </p>
        /// </summary>
        /// <seealso cref="Some"/>
        [OverloadResolutionPriority(10)]
        public bool Nothing => collection is null || collection.Count == 0;
    }


    extension<E>(IReadOnlyCollection<E> collection)
    {
        /// <summary>
        /// Checks whether this collection is empty.
        /// </summary>
        /// <returns>true when empty.</returns>
        public bool IsEmpty => collection.Count == 0;
    }

}
