using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Extensions;

public static class AnyExt
{

    extension<T>(T item)
    {
        /**
         * Check whether this item is in the given collection.
         */
        public bool IsIn(IEnumerable<T> collection) => collection.Contains(item);

        /**
         * Check whether this item is not in the given collection.
         */
        public bool IsNotIn(IEnumerable<T> collection) => !collection.Contains(item);

        /**
         * When the condition is true, it returns <see cref="item"/> modified by <see cref="modifier"/>
         * otherwise returns <see cref="item"/> as is.
         */
        public T ModifyIf(bool condition, Func<T, T> modifier)
            => condition
                ? modifier(item)
                : item;

    }


    extension<T>(T? item)
    {
        /**
         * Takes the item only if the given condition is true, otherwise it returns null.
         */
        public T? TakeIf(bool condition) => item is not null && condition ? item : default(T?);

        /**
         * Takes the item only if the given predicate matches, otherwise it returns null.
         *
         * Note: when the original item is already null, the predicate is not called.
         */
        public T? TakeIf(Predicate<T> predicate) => item is not null && predicate(item) ? item : default(T?);
    }
}
