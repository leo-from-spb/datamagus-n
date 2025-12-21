using System;

namespace Util.Fun;

/// <summary>
/// Generic extension functions and properties.
/// </summary>
public static class AnyFun
{
    extension<A>(A self)
    {
        /**
         * When the condition is true, it returns <see cref="self"/> modified by <see cref="modifier"/>
         * otherwise returns <see cref="self"/> as is.
         */
        public A ModifyIf(bool condition, Func<A, A> modifier)
            => condition
                ? modifier(self)
                : self;
    }
}
