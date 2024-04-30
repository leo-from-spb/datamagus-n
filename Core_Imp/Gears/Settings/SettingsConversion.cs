using System.Drawing;
using System.Text.RegularExpressions;
using static Util.Fun.RegexFun;

namespace Core.Gears.Settings;



public static class SettingsConversion
{

    public static string? ExportRectangle(Rectangle? rectangle) =>
        rectangle.HasValue ? ExportRectangle(rectangle.Value) : null;

    public static string ExportRectangle(Rectangle rectangle) =>
        $"({rectangle.X}, {rectangle.Y}, {rectangle.Width}, {rectangle.Height})";


    public static Rectangle? ImportRectangle(string? str, out string? error)
    {
        if (str is null) { error = null; return null; }

        var m = RectangleRegex.Match(str);
        if (m.Success)
        {
            int x, y = 0, w = 0, h = 0;
            bool ok = int.TryParse(m.Groups[1].Value, out x)
                   && int.TryParse(m.Groups[2].Value, out y)
                   && int.TryParse(m.Groups[3].Value, out w)
                   && int.TryParse(m.Groups[4].Value, out h);
            error = ok ? null : $"Wrong number values in the rectangle {str}";
            return ok ? new Rectangle(x, y, w, h) : null;
        }
        else
        {
            error = $"Cannot parse the rectangle {str}";
            return null;
        }
    }

    private static readonly Regex RectangleRegex = RegEx(@"^ \( ([+-]?\d+) , ([+-]?\d+) , ([+-]?\d+) , ([+-]?\d+) \) $");




}
