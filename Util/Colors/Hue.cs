namespace Util.Colors;

/// <summary>
/// Hue component of a color.
///
/// The regular hue component is a value of degree in the range of 0..359,
/// but this internal value could be decoded:
/// * 0 means no Hue (black and white),
/// * 1 .. 359 mean hue 1 .. 359,
/// * 360 means hue 0 or 360.
///
/// This struct is a value object (it is not modifiable).
/// </summary>
public readonly struct Hue
{
    /// <summary>
    /// The value in degree;
    /// 0 means black and white,
    /// 1..360 mean the hue component.
    /// </summary>
    public readonly ushort Degree;


    private const ushort __0__   = (ushort)0u;
    private const ushort __360__ = (ushort)360u;

    public static Hue Of(ushort degree)
    {
        ushort d;
        if (degree == 0)
        {
            d = __0__;
        }
        else
        {
            d = (ushort)(degree % __360__);
            if (d == __0__) d = __360__;
        }
        return new Hue(d);
    }

    public static Hue Of(uint degree)
    {
        ushort d;
        if (degree == 0)
        {
            d = __0__;
        }
        else
        {
            d = (ushort)(degree % __360__);
            if (d == __0__) d = __360__;
        }
        return new Hue(d);
    }


    public static Hue BW = new Hue(__0__);


    internal Hue(ushort degree)
    {
        Degree = degree;
    }

    /// <summary>
    /// Determines whether there's no hue but a shade of gray.
    /// </summary>
    public bool IsBW => Degree == __0__;

    /// <summary>
    /// Hexadecimal string form (just 6 hexadecimal digits without any prefices).
    /// </summary>
    public override string ToString() => Degree.ToString();
}
