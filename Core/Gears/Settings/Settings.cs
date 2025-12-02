using System.Drawing;

namespace Core.Gears.Settings;

/// <summary>
/// Base class for all classes with settings.
/// </summary>
public interface AbstractSettings
{

    /// <summary>
    /// All settings entries, including with null or non-changed ones.
    /// </summary>
    /// <returns></returns>
    public SettingPair[] ExportEntries();

    /// <summary>
    /// Imports one entry.
    /// </summary>
    /// <param name="entry">the entry to import.</param>
    /// <param name="error">null when success, the error text when error is occurred.</param>
    public void ImportEntry(SettingPair entry, out string? error);

}


/// <summary>
/// Settings that relate to the computer on which this software is installed.
/// These settings are computed automatically and could not be changed by the user.
/// </summary>
public interface SystemSettings : AbstractSettings
{
    public string UserName                   { get; }
    public string SystemPreferencesPath      { get; }
    public string SystemWorkspacePath        { get; }
    public string ActualPersonalSettingsPath { get; }
    public string ActualComputerSettingsPath { get; }
}


/// <summary>
/// Settings that relate to the user's workspace, for example, position of windows, last typed text, etc.
/// These settings are remembered automatically and transparently to the user; there's no UI for changing these settings.
/// </summary>
public interface WorkspaceSettings : AbstractSettings
{

    public Rectangle? MainWindowPlace { get; set; }

}
