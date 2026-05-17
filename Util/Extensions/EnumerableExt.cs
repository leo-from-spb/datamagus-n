using System;
using System.Collections.Generic;
using System.Text;
using Util.Collections;

namespace Util.Extensions;

public static class EnumerableExt
{

    extension<E>(IEnumerable<E>? seq)
    {
        /// <summary>
        /// Joins elements from this sequence into a string (skipping some elements).
        /// Every element is transformed into a string by given function, and null values are skipped.
        /// </summary>
        /// <param name="fun">the function to convert an element into a string; null values are skipped.</param>
        /// <param name="separator">the separator between elements.</param>
        /// <param name="prefix">the prefix (only when the result is not empty).</param>
        /// <param name="suffix">the suffix (only when the result is not empty).</param>
        /// <param name="empty">what to return when no applicable elements.</param>
        /// <returns>the result string.</returns>
        public string JoinToString(Func<E, string?> map,
                                   string           separator = ", ",
                                   string           prefix    = "",
                                   string           suffix    = "",
                                   string           empty     = "")
        {
            if (seq is null) return empty;
            var b     = new StringBuilder();
            var begin = true;
            foreach (var item in seq)
            {
                var s = map(item);
                if (s is null) continue;
                b.Append(begin ? prefix : separator);
                b.Append(s);
                begin = false;
            }

            b.Append(begin ? empty : suffix);
            return b.ToString();
        }
    }


    extension<E>(IEnumerable<E> seq)
    {
        /// <summary>
        /// Walks through this sequence and try every element to match the given predicate.
        /// </summary>
        /// <param name="predicate">the predicate.</param>
        /// <returns>result of the search.</returns>
        public Found<E> Find(Predicate<E> predicate)
        {
            foreach (var e in seq)
                if (predicate(e)) return new Found<E>(true, e);
            return Found<E>.NotFound;
        }

        /// <summary>
        /// Transforms every item using the given <see cref="selector"/>
        /// and return ones that are not null.
        /// </summary>
        /// <param name="selector">the transformation to apply;
        ///                        this function should return a non-null value to get it in the result sequence,
        ///                        or null to skip it.</param>
        /// <typeparam name="R">type of the result (should be not null).</typeparam>
        /// <returns>a sequence of transformed and filtered items.</returns>
        public IEnumerable<R> SelectNotNull<R>(Func<E,R?> selector)
            where R : class
        {
            foreach (var item in seq)
            {
                var result = selector(item);
                if (result != null)
                    yield return result;
            }
        }


        /// <summary>
        /// Add all items from this sequence to the given list.
        /// </summary>
        /// <param name="list">a list to add elements to.</param>
        public void Into(List<E> list)
        {
            list.AddRange(seq);
        }
    }



    extension(IEnumerable<string>? seq)
    {
        /// <summary>
        /// Joins strings from this sequence.
        /// Every element is transformed into a string by given function, and null values are skipped.
        /// </summary>
        /// <param name="separator">the separator between elements.</param>
        /// <param name="prefix">the prefix (only when the result is not empty).</param>
        /// <param name="suffix">the suffix (only when the result is not empty).</param>
        /// <param name="empty">what to return when no applicable elements.</param>
        /// <returns>the result string.</returns>
        public string JoinToString(string separator = ", ",
                                   string prefix    = "",
                                   string suffix    = "",
                                   string empty     = "") =>
            seq.JoinToString(map: s => s,
                             separator: separator,
                             prefix: prefix,
                             suffix: suffix,
                             empty: empty);
    }

}
