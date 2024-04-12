using static Util.Fun.NumberConstants;

namespace Util.Fun;

public static class Numbers
{

    /// <summary>
    /// Next byte.
    /// </summary>
    /// <exception cref="System.OverflowException">when the result is too large to fit in byte.</exception>
    public static byte succ(this byte b) => checked( (byte)(b + _1_) );

    /// <summary>
    /// Prior byte.
    /// </summary>
    /// <exception cref="System.OverflowException">when the result is less than zero.</exception>
    public static byte pred(this byte b) => checked( (byte)(b - _1_) );
    
    
}