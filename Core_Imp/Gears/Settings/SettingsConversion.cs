using System.Drawing;

namespace Core.Gears.Settings;



public static class SettingsConversion
{

    public static string ExportRectangle(Rectangle rectangle) =>
        $"({rectangle.X}, {rectangle.Y}, {rectangle.Width}, {rectangle.Height})";

}
