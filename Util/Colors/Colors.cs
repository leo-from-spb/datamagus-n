using System;

namespace Util.Colors;

/// <summary>
/// Functions for working with colors.
///
/// Functions in this class don't do something with GUI and don't relate to a kind of GUI framework.
/// They just functions about colors.
/// </summary>
public static class Colors
{

    /// <summary>
    /// Converts a color from the HSL form into the RGB for.
    /// </summary>
    /// <param name="hue">hue.</param>
    /// <param name="sat">saturation in percent, from 0 to 100.</param>
    /// <param name="lum">luminance (lightning) in percent, from 0 to 100.</param>
    /// <returns>the color in RGB.</returns>
    public static Rgb ConvertHslToRgb(Hue hue, byte sat, byte lum)
    {
        // Normalize saturation and luminance to 0-1 range
        double s = sat / 100.0;
        double l = lum / 100.0;

        // If saturation is 0, it's a shade of gray
        if (hue.IsBW || sat == 0)
        {
            byte gray = (byte)Math.Round(lum * 255.0 / 100.0);
            uint value = ((uint)gray << 16) | ((uint)gray << 8) | gray;
            return Rgb.Of(value);
        }

        // Calculate chroma
        double c = (1 - Math.Abs(2 * l - 1)) * s;

        // Get hue in 0-360 range (note: hue.Degree of 360 should be treated as 0)
        double h = hue.Degree == 360 ? 0 : hue.Degree;

        // Calculate intermediate value
        double hPrime = h / 60.0;
        double x = c * (1 - Math.Abs(hPrime % 2 - 1));

        double r1, g1, b1;

        if (h < 60)
        {
            r1 = c; g1 = x; b1 = 0;
        }
        else if (h < 120)
        {
            r1 = x; g1 = c; b1 = 0;
        }
        else if (h < 180)
        {
            r1 = 0; g1 = c; b1 = x;
        }
        else if (h < 240)
        {
            r1 = 0; g1 = x; b1 = c;
        }
        else if (h < 300)
        {
            r1 = x; g1 = 0; b1 = c;
        }
        else
        {
            r1 = c; g1 = 0; b1 = x;
        }

        // Calculate m (amount to add to each component)
        double m = l - c / 2;

        // Convert to 0-255 range
        byte r = (byte)Math.Round((r1 + m) * 255);
        byte g = (byte)Math.Round((g1 + m) * 255);
        byte b = (byte)Math.Round((b1 + m) * 255);

        uint rgbValue = ((uint)r << 16) | ((uint)g << 8) | b;
        return Rgb.Of(rgbValue);
    }



}
