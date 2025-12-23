using System;
using System.Collections.Generic;
using System.Linq;
using Util.Collections.Implementation;

namespace Util.Collections;

/// <summary>
/// Extension functions for different DotNet native structures
/// that make immutable copies.
/// </summary>
public static class ImmExtensions
{
    /// <summary>
    /// Extension functions for an array of elements
    /// that make immutable copies of the original array.
    /// </summary>
    /// <param name="array">this original array.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E> (E[] array)
    {
        /// <summary>
        /// Creates a snapshot of this array.
        /// </summary>
        /// <returns>the snapshot.</returns>
        public ImmList<E> ToImmList() =>
            array.Length switch
            {
                0 => EmptySet<E>.Instance,
                1 => new ImmutableSingleton<E>(array[0]),
                _ => new ImmutableArrayList<E>(array.CloneArray())
            };

        /// <summary>
        /// Deduplicates elements and creates a snapshot set of this array.
        /// </summary>
        /// <returns>the created immutable set.</returns>
        public ImmListSet<E> ToImmSet()
        {
            int n = array.Length;
            return n switch
                   {
                       0 => EmptySet<E>.Instance,
                       1 => new ImmutableSingleton<E>(array[0]),
                       _ => SortingLogic.DeduplicateAndPrepareSet<E>(array, n)
                   };
        }
    }


    /// <summary>
    /// Extension functions for an array of comparable elements
    /// that make immutable copies of the original array.
    /// </summary>
    /// <param name="array">this original array.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(E[] array)
        where E : IComparable<E>
    {
        /// <summary>
        /// Sorts and deduplicates elements and creates a snapshot set of this <paramref name="array"/>.
        /// </summary>
        /// <returns>the created immutable sorted set.</returns>
        public ImmSortedListSet<E> ToImmSortedSet()
        {
            int n = array.Length;
            if (n == 0) return EmptySortedSet<E>.Instance;
            if (n == 1) return new ImmutableSortedSet<E>(array);

            E[] newArray = new E[n];
            Array.Copy(array, newArray, n);

            return SortingLogic.SortDeduplicateAndPrepareSet(newArray);
        }
    }


    /// <summary>
    /// Extension functions for a span of elements
    /// that make immutable copies of the original span.
    /// </summary>
    /// <param name="span">this original span.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(ReadOnlySpan<E> span)
    {
        /// <summary>
        /// Creates a snapshot of this array.
        /// </summary>
        /// <returns>the snapshot.</returns>
        public ImmList<E> ToImmList() =>
            span.Length switch
            {
                0 => EmptySet<E>.Instance,
                1 => new ImmutableSingleton<E>(span[0]),
                _ => new ImmutableArrayList<E>(span)
            };

        /// <summary>
        /// Deduplicates elements and creates a snapshot set of them.
        /// </summary>
        /// <param name="span">elements.</param>
        /// <typeparam name="E">type of elements.</typeparam>
        /// <returns>the created immutable set.</returns>
        public ImmListSet<E> ToImmSet()
        {
            int n = span.Length;
            if (n == 0) return EmptySet<E>.Instance;
            if (n == 1) return new ImmutableSingleton<E>(span[0]);

            return SortingLogic.DeduplicateAndPrepareSet<E>(span, n);
        }

        /// <summary>
        /// Associates elements by their keys. The order is preserved.
        /// All keys must be unique.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <returns>the just created dictionary.</returns>
        public ImmListDict<K,E> ToImmDict<K>(Func<E,K> keySelector)
            where K : notnull
        {
            int n     = span.Length;
            if (n == 0) return EmptyDictionary<K,E>.Instance;
            if (n == 1) return new ImmutableSingletonDictionary<K,E>(keySelector(span[0]), span[0]);

            var pairs = ArrayLogic.PreparePairs(span, keySelector);
            return ImmutableArrayDictionary<K,E>.MakeListDict(pairs);
        }

        /// <summary>
        /// Converts a span or an array to a dictionary. The order is preserved.
        /// All keys must be unique.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <param name="valueSelector">the function that extract the value from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <typeparam name="V">type of the value.</typeparam>
        /// <returns>the just created dictionary.</returns>
        public ImmListDict<K,V> ToImmDict<K,V>(Func<E,K> keySelector, Func<E,V> valueSelector)
            where K : notnull
        {
            int n     = span.Length;
            if (n == 0) return EmptyDictionary<K,V>.Instance;
            if (n == 1) return new ImmutableSingletonDictionary<K,V>(keySelector(span[0]), valueSelector(span[0]));

            var pairs = ArrayLogic.PreparePairs(span, keySelector, valueSelector);
            return ImmutableArrayDictionary<K,V>.MakeListDict(pairs);
        }

        /// <summary>
        /// Converts a span or an array to a sorted dictionary.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <returns>the just created sorted dictionary.</returns>
        public ImmSortedDict<K,E> ToImmSortedDict<K>(Func<E,K> keySelector)
            where K : IComparable<K>
        {
            int n     = span.Length;
            if (n == 0) return EmptySortedDictionary<K,E>.Instance;
            if (n == 1) return new ImmutableSingletonSortedDictionary<K,E>(keySelector(span[0]), span[0]);

            var pairs = ArrayLogic.PreparePairs(span, keySelector);
            return SortingLogic.MakeImmSortedDict(pairs);
        }

        /// <summary>
        /// Converts a span or an array to a sorted dictionary.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <param name="valueSelector">the function that extract the value from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <typeparam name="V">type of the value.</typeparam>
        /// <returns>the just created sorted dictionary.</returns>
        public ImmSortedDict<K,V> ToImmSortedDict<K,V>(Func<E,K> keySelector, Func<E,V> valueSelector)
            where K : IComparable<K>
        {
            int n     = span.Length;
            if (n == 0) return EmptySortedDictionary<K,V>.Instance;

            var pairs = ArrayLogic.PreparePairs(span, keySelector, valueSelector);
            return SortingLogic.MakeImmSortedDict(pairs);
        }
    }


    /// <summary>
    /// Extension functions for a span of comparable elements
    /// that make immutable copies of the original span.
    /// </summary>
    /// <param name="span">this original span.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(ReadOnlySpan<E> span)
        where E : IComparable<E>
    {

        /// <summary>
        /// Sorts and deduplicates elements and creates a snapshot set of this <paramref name="span"/>.
        /// </summary>
        /// <returns>the created immutable sorted set.</returns>
        public ImmSortedListSet<E> ToImmSortedSet()
        {
            int n = span.Length;
            if (n == 0) return EmptySortedSet<E>.Instance;
            if (n == 1) return new ImmutableSortedSingleton<E>(span[0]);

            E[] newArray = span.ToArray();
            return SortingLogic.SortDeduplicateAndPrepareSet(newArray);
        }

    }


    /// <summary>
    /// Extension functions for enumerable elements
    /// that make immutable copies of them.
    /// </summary>
    /// <param name="source">original elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IEnumerable<E> source)
    {
        /// <summary>
        /// Collects all elements from this <paramref name="source"/> and make an immutable list of them.
        /// </summary>
        /// <returns>the immutable list.</returns>
        public ImmList<E> ToImmList()
        {
            if (source is IReadOnlyCollection<E> collection) return collection.ToImmList();
            E[] array = source.ToArray();
            return array.ToImmList();
        }

        /// <summary>
        /// Collects and deduplicates all elements from this <paramref name="source"/> and creates an immutable set.
        /// </summary>
        /// <returns>the created immutable set.</returns>
        public ImmSet<E> ToImmSet()
        {
            return source switch
                   {
                       ImmSet<E> alreadyImmSet     => alreadyImmSet,
                       IReadOnlySet<E> set         => set.ToImmSet(),
                       ImmutableArrayList<E> immAL => SortingLogic.DeduplicateAndPrepareSet(immAL.ShareElementsArray(), immAL.Count),
                       E[] array                   => SortingLogic.DeduplicateAndPrepareSet(array, array.Length),
                       _                           => SortingLogic.DeduplicateAndPrepareSet<E>(source, 0)
                   };
        }

        /// <summary>
        /// Collect elements from this source and associate them by their keys. The order is preserved.
        /// All keys must be unique.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <returns>the just created dictionary.</returns>
        public ImmListDict<K,E> ToImmDict<K>(Func<E,K> keySelector)
            where K : notnull
        {
            IReadOnlyList<E> list =
                source switch
                {
                    IReadOnlyList<E> alreadyList => alreadyList,
                    _                            => source.ToList()
                };
            return list.ToImmDict(keySelector);
        }

        /// <summary>
        /// Collect elements from this source and make a dictionary from them. The order is preserved.
        /// All keys must be unique.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <param name="valueSelector">the function that extract the value from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <typeparam name="V">type of the value.</typeparam>
        /// <returns>the just created dictionary.</returns>
        public ImmListDict<K,V> ToImmDict<K,V>(Func<E,K> keySelector, Func<E,V> valueSelector)
            where K : notnull
        {
            IReadOnlyList<E> list =
                source switch
                {
                    IReadOnlyList<E> alreadyList => alreadyList,
                    _                            => source.ToList()
                };
            return list.ToImmDict(keySelector, valueSelector);
        }

        /// <summary>
        /// Collect elements from this source, sort them and make a dictionary from them.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <returns>the just created sorted dictionary.</returns>
        public ImmSortedDict<K,E> ToImmSortedDict<K>(Func<E,K> keySelector)
            where K : IComparable<K>
        {
            var pairs = ArrayLogic.PreparePairs(source, keySelector);
            return SortingLogic.MakeImmSortedDict(pairs);
        }

        /// <summary>
        /// Collect elements from this source, sort them and make a dictionary from them.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <param name="valueSelector">the function that extract the value from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <typeparam name="V">type of the value.</typeparam>
        /// <returns>the just created sorted dictionary.</returns>
        public ImmSortedDict<K,V> ToImmSortedDict<K,V>(Func<E,K> keySelector, Func<E,V> valueSelector)
            where K : IComparable<K>
        {
            var pairs = ArrayLogic.PreparePairs(source, keySelector, valueSelector);
            return SortingLogic.MakeImmSortedDict(pairs);
        }

    }


    /// <summary>
    /// Extension functions for somehow ordered enumerable elements
    /// that make immutable copies of them.
    /// </summary>
    /// <param name="source">somehow ordered original elements.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IOrderedEnumerable<E> source)
    {
        /// <summary>
        /// Collects and deduplicates all elements from this <paramref name="source"/> and creates an immutable set.
        /// Preserves the original order.
        /// </summary>
        /// <returns>the created immutable set.</returns>
        public ImmListSet<E> ToImmSet()
        {
            return SortingLogic.DeduplicateAndPrepareSet<E>(source, 0);
        }
    }


    /// <summary>
    /// Extension functions for a collection of elements
    /// that make immutable copies of the original collection.
    /// </summary>
    /// <param name="collection">this original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IReadOnlyCollection<E> collection)
    {
        /// <summary>
        /// Creates a snapshot of this collection.
        /// </summary>
        /// <returns>the snapshot.</returns>
        public ImmList<E> ToImmList()
        {
            if (collection is ImmutableArrayList<E> ia) return ia;
            return collection.Count switch
                   {
                       0 => EmptySet<E>.Instance,
                       1 => new ImmutableSingleton<E>(collection.First()),
                       _ => new ImmutableArrayList<E>(collection.ToArray())
                   };
        }

        /// <summary>
        /// Deduplicates elements and creates a snapshot set of this <paramref name="collection"/>.
        /// </summary>
        /// <returns>the created immutable set.</returns>
        public ImmSet<E> ToImmSet()
        {
            if (collection is ImmSet<E> immSet) return immSet;

            int n = collection.Count;
            if (n == 0) return EmptySet<E>.Instance;
            if (n == 1) return new ImmutableSingleton<E>(collection.First());

            if (collection is IReadOnlySet<E> set) return set.ToImmSet();
            if (collection is IReadOnlyList<E> immList) return immList.ToImmSet();

            return SortingLogic.DeduplicateAndPrepareSet(collection, n);
        }
    }


    /// <summary>
    /// Extension functions for a collection of comparable elements
    /// that make immutable copies of the original collection.
    /// </summary>
    /// <param name="collection">this original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IReadOnlyCollection<E> collection)
        where E : IComparable<E>
    {
        /// <summary>
        /// Sorts and deduplicates elements and creates a snapshot set of this <paramref name="collection"/>.
        /// </summary>
        /// <returns>the created immutable sorted set.</returns>
        public ImmSortedListSet<E> ToImmSortedSet()
        {
            if (collection is ImmSortedListSet<E> immSortedSet) return immSortedSet;

            int n = collection.Count;
            if (n == 0) return EmptySortedSet<E>.Instance;
            if (n == 1) return new ImmutableSortedSingleton<E>(collection.First());

            if (collection is SortedSet<E> alreadySortedSet) return alreadySortedSet.ToImmSortedSet();
            if (collection is IReadOnlySet<E> alreadySet) return alreadySet.ToImmSortedSet();

            E[] array = collection.ToArray();
            return SortingLogic.SortDeduplicateAndPrepareSet(array);
        }
    }


    /// <summary>
    /// Extension functions for a list of elements
    /// that make immutable copies of the original list.
    /// </summary>
    /// <param name="list">this original collection.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IReadOnlyList<E> list)
    {
        /// <summary>
        /// Deduplicates elements and creates a snapshot set of this <paramref name="list"/>.
        /// </summary>
        /// <returns>the created immutable set.</returns>
        public ImmListSet<E> ToImmSet()
        {
            if (list is ImmListSet<E> immSet) return immSet;
            if (list is ImmutableArrayList<E> immArrayList) immArrayList.ToSet();

            int n = list.Count;
            if (n == 0) return EmptySet<E>.Instance;

            List<E> distinct = list.Deduplicate();
            int     m        = distinct.Count;
            if (m == 1) return new ImmutableSingleton<E>(distinct[0]);

            E[] newArray = distinct.ToArray();
            return m <= 3
                ? new ImmutableMiniSet<E>(newArray)
                : new ImmutableHashSet<E>(newArray);
        }

        /// <summary>
        /// Associates elements by their keys. The order is preserved.
        /// All keys must be unique.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <returns>the just created dictionary.</returns>
        public ImmListDict<K,E> ToImmDict<K>(Func<E,K> keySelector)
            where K : notnull
        {
            int n     = list.Count;
            if (n == 0) return EmptyDictionary<K,E>.Instance;
            if (n == 1) return new ImmutableSingletonDictionary<K,E>(keySelector(list[0]), list[0]);

            var pairs = ArrayLogic.PreparePairs(list, keySelector);
            return ImmutableArrayDictionary<K,E>.MakeListDict(pairs);
        }

        /// <summary>
        /// Converts a list to a dictionary. The order is preserved.
        /// All keys must be unique.
        /// </summary>
        /// <param name="keySelector">the function that extract the key from the given element.</param>
        /// <param name="valueSelector">the function that extract the value from the given element.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <typeparam name="V">type of the value.</typeparam>
        /// <returns>the just created dictionary.</returns>
        public ImmListDict<K,V> ToImmDict<K,V>(Func<E,K> keySelector, Func<E,V> valueSelector)
            where K : notnull
        {
            int n = list.Count;
            if (n == 0) return EmptyDictionary<K,V>.Instance;
            if (n == 1) return new ImmutableSingletonDictionary<K,V>(keySelector(list[0]), valueSelector(list[0]));

            var pairs = ArrayLogic.PreparePairs(list, keySelector, valueSelector);
            return ImmutableArrayDictionary<K,V>.MakeListDict(pairs);
        }

    }


    /// <summary>
    /// Extension functions for a set of elements
    /// that make immutable copies of the original set.
    /// </summary>
    /// <param name="set">this original set.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IReadOnlySet<E> set)
    {
        /// <summary>
        /// Makes an immutable snapshot of this <paramref name="set"/>.
        /// </summary>
        /// <returns>the created immutable set.</returns>
        public ImmSet<E> ToImmSet()
        {
            if (set is ImmSet<E> immSet) return immSet;

            int n = set.Count;
            return n switch
                   {
                       0    => EmptySet<E>.Instance,
                       1    => new ImmutableSingleton<E>(set.First()),
                       <= 3 => new ImmutableMiniSet<E>(set.ToArray()),
                       _    => new ImmutableHashSet<E>(set.ToArray())
                   };
        }
    }


    /// <summary>
    /// Extension functions for a set of comparable elements
    /// that make immutable copies of the original set.
    /// </summary>
    /// <param name="set">this original set.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(IReadOnlySet<E> set)
        where E : IComparable<E>
    {
        /// <summary>
        /// Sorts elements and creates a snapshot of this <paramref name="set"/>.
        /// </summary>
        /// <param name="set">elements.</param>
        /// <typeparam name="E">type of elements.</typeparam>
        /// <returns>the created immutable sorted set.</returns>
        public ImmSortedListSet<E> ToImmSortedSet()
        {
            if (set is ImmSortedListSet<E> immSortedSet) return immSortedSet;

            int n = set.Count;
            if (n == 0) return EmptySortedSet<E>.Instance;
            if (n == 1) return new ImmutableSortedSingleton<E>(set.First());

            E[] newArray = set.ToArray();
            Array.Sort(newArray);
            return new ImmutableSortedSet<E>(newArray);
        }
    }


    /// <summary>
    /// Extension functions for a sorted set of comparable elements
    /// that make immutable copies of the original set.
    /// </summary>
    /// <param name="sortedSet">this original set.</param>
    /// <typeparam name="E">type of elements.</typeparam>
    extension<E>(SortedSet<E> sortedSet)
        where E : IComparable<E>
    {
        /// <summary>
        /// Creates a snapshot of this <paramref name="sortedSet"/>.
        /// </summary>
        /// <returns>the created immutable sorted set.</returns>
        public ImmSortedListSet<E> ToImmSortedSet()
        {
            return sortedSet.Count switch
                   {
                       0 => EmptySortedSet<E>.Instance,
                       1 => new ImmutableSortedSingleton<E>(sortedSet.Min!),
                       _ => new ImmutableSortedSet<E>(sortedSet.ToArray())
                   };
        }
    }


    /// <summary>
    /// Extension functions for a dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<K,V>(IReadOnlyDictionary<K,V> dictionary)
    {
        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmListDict<K,V> ToImmDict()
        {
            if (dictionary is ImmListDict<K,V> immDict) return immDict;

            int n = dictionary.Count;
            if (n == 0) return EmptyDictionary<K,V>.Instance;

            KeyValuePair<K,V>[] pairs = dictionary.ToArray();
            return ImmutableArrayDictionary<K,V>.MakeListDict(pairs);
        }
    }


    /// <summary>
    /// Extension functions for a dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<K,V>(IReadOnlyDictionary<K,V> dictionary)
        where K : IComparable<K>
    {
        /// <summary>
        /// Sorts the entries and makes an immutable snapshot of them.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmSortedDict<K,V> ToImmSortedDict()
        {
            if (dictionary is ImmSortedDict<K,V> alreadyDict) return alreadyDict;
            if (dictionary.Count == 0) return EmptySortedDictionary<K,V>.Instance;
            KeyValuePair<K,V>[] pairs = dictionary.ToArray();
            return SortingLogic.MakeImmSortedDict(pairs);
        }
    }


    /// <summary>
    /// Extension functions for a dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<K,V>(IDictionary<K,V> dictionary)
    {
        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmListDict<K,V> ToImmDict()
        {
            int n = dictionary.Count;
            if (n == 0) return EmptyDictionary<K,V>.Instance;

            KeyValuePair<K,V>[] pairs = dictionary.ToArray();
            return ImmutableArrayDictionary<K,V>.MakeListDict(pairs);
        }
    }


    /// <summary>
    /// Extension functions for a dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<K,V>(IDictionary<K,V> dictionary)
        where K : IComparable<K>
    {
        /// <summary>
        /// Sorts the entries and makes an immutable snapshot of them.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmSortedDict<K,V> ToImmSortedDict()
        {
            if (dictionary.Count == 0) return EmptySortedDictionary<K,V>.Instance;
            KeyValuePair<K,V>[] pairs = dictionary.ToArray();
            return SortingLogic.MakeImmSortedDict(pairs);
        }
    }


    /// <summary>
    /// Extension functions for a dictionary.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key, not null.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<K,V>(Dictionary<K,V> dictionary)
        where K : notnull
    {
        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// </summary>
        /// <param name="dictionary">the original dictionary.</param>
        /// <typeparam name="K">type of the key.</typeparam>
        /// <typeparam name="V">type of the value.</typeparam>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmListDict<K,V> ToImmDict()
        {
            int n = dictionary.Count;
            if (n == 0) return EmptyDictionary<K,V>.Instance;

            KeyValuePair<K,V>[] pairs = dictionary.ToArray();
            return ImmutableArrayDictionary<K,V>.MakeListDict(pairs);
        }
    }


    /// <summary>
    /// Extension functions for a dictionary with a comparable key.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="K">type of the key, not null.</typeparam>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<K,V>(Dictionary<K,V> dictionary)
        where K : IComparable<K>
    {
        /// <summary>
        /// Sorts the entries and makes an immutable snapshot of them.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmSortedDict<K,V> ToImmSortedDict()
            => ((IDictionary<K,V>)dictionary).ToImmSortedDict();
    }


    /// <summary>
    /// Extension functions for a dictionary with key type <b>uint</b>.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<V>(IReadOnlyDictionary<uint,V> dictionary)
    {
        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// Specialization for <c>uint</c> keys.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmSortedDict<uint,V> ToImmSortedDict()
        {
            if (dictionary is ImmSortedDict<uint,V> immDict) return immDict;
            return SortingLogic.MakeImmSortedDict(dictionary, dictionary.Count);
        }

        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// Specialization for <c>uint</c> keys.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmDict<uint,V> ToImmDict()
        {
            if (dictionary is ImmDict<uint,V> immDict) return immDict;
            return SortingLogic.MakeImmSortedDict(dictionary, dictionary.Count);
        }
    }


    /// <summary>
    /// Extension functions for a dictionary with key type <b>uint</b>.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<V>(IDictionary<uint,V> dictionary)
    {
        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// Specialization for <c>uint</c> keys.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmSortedDict<uint,V> ToImmSortedDict()
            => SortingLogic.MakeImmSortedDict(dictionary, dictionary.Count);

        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// Specialization for <c>uint</c> keys.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmDict<uint,V> ToImmDict()
            => SortingLogic.MakeImmSortedDict(dictionary, dictionary.Count);
    }


    /// <summary>
    /// Extension functions for a dictionary with key type <b>uint</b>.
    /// </summary>
    /// <param name="dictionary">the original dictionary.</param>
    /// <typeparam name="V">type of the value.</typeparam>
    extension<V>(Dictionary<uint,V> dictionary)
    {
        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// Specialization for <c>uint</c> keys.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmSortedDict<uint,V> ToImmSortedDict()
            => SortingLogic.MakeImmSortedDict(dictionary, dictionary.Count);

        /// <summary>
        /// Makes an immutable snapshot of this dictionary.
        /// Specialization for <c>uint</c> keys.
        /// </summary>
        /// <returns>the just created immutable snapshot.</returns>
        public ImmDict<uint,V> ToImmDict()
            => SortingLogic.MakeImmSortedDict(dictionary, dictionary.Count);
    }

}
