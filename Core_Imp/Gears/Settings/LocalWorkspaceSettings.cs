using System.Drawing;
using static Core.Gears.Settings.SettingsConversion;

namespace Core.Gears.Settings;


public class LocalWorkspaceSettings : WorkspaceSettings
{

    public Rectangle? MainWindowPlace { get; set; }


    public SettingPair[] ExportEntries() =>
    [
        new SettingPair(nameof(MainWindowPlace), ExportRectangle(MainWindowPlace))
    ];

    public void ImportEntry(SettingPair entry, out string? error)
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
