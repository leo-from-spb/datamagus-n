using System.Drawing;
using Util.Structures;
using static Core.Gears.Settings.SettingsConversion;

namespace Core.Gears.Settings;


public class LocalWorkspaceSettings : WorkspaceSettings
{

    public Rectangle? MainWindowPlace { get; set; }


    public Named<string?>[] ExportEntries() =>
        new Named<string?>[]
        {
            ExportRectangle(MainWindowPlace).WithName("MainWindowPlace")
        };

    public void ImportEntry(Named<string> entry, out string? error)
    {
        string? str = entry.Thing;
        switch (entry.Name)
        {
            case "MainWindowPlace":
                MainWindowPlace = ImportRectangle(str, out error);
                break;
            default:
                error = $"Unknown setting name: {entry.Name}";
                break;
        }
    }
}
