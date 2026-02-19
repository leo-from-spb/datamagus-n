using System.Diagnostics;

namespace Util.Colors;

/// <summary>
/// Color in the RGB form.
/// It doesn't contain alpha component.
/// </summary>
public readonly struct Rgb
{
    /// <summary>
    /// The color value,
    /// where 0xFF0000 is the red component, 0x00FF00 is the green one and 0x0000FF is the blue one.
    /// </summary>
    public readonly uint Value;

    public static Rgb Of(uint value)
    {
        Debug.Assert(value <= 0xFFFFFF);
        return new Rgb(value);
    }

    public static Rgb Of(int value)
    {
        Debug.Assert(value is >= 0 and <= 0xFFFFFF);
        return new Rgb((uint)value);
    }



    internal Rgb(uint value)
    {
        Value = value;
    }

    public byte Red => (byte)(Value >> 16);
    public byte Green => (byte)((Value >> 8) & 0xFF);
    public byte Blue => (byte)(Value & 0xFF);

    public override string ToString() => Value.ToString("X6");
}
