using System;
using System.Collections.Generic;
using Util.Collections;

namespace Util.Extensions;

public static class EnumerableExt
{

    extension<E>(IEnumerable<E> enumerable)
    {
        /// <summary>
        /// Walks through this sequence and try every element to match the given predicate.
        /// </summary>
        /// <param name="predicate">the predicate.</param>
        /// <returns>result of the search.</returns>
        public Found<E> Find(Predicate<E> predicate)
        {
            foreach (var e in enumerable)
                if (predicate(e)) return new Found<E>(true, e);
            return Found<E>.NotFound;
        }
    }


}
