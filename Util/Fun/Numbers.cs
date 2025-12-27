using static Util.Fun.NumberConstants;

namespace Util.Fun;

public static class Numbers
{

    extension(byte b)
    {
        /// <summary>
        /// Next byte.
        /// </summary>
        /// <exception cref="System.OverflowException">when the result is too large to fit in byte.</exception>
        public byte Succ => checked( (byte)(b + _1_) );

        /// <summary>
        /// Prior byte.
        /// </summary>
        /// <exception cref="System.OverflowException">when the result is less than zero.</exception>
        public byte Pred => checked( (byte)(b - _1_) );
    }

}