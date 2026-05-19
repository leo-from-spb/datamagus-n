namespace Core.Stationery;

public sealed class ApplicationLocations
{
    /// <summary>
    /// Name of the subdirectory with settings.
    /// </summary>
    public const string ApplicationSettingsDirectoryName = "DataMagus";

    /// <summary>
    /// Path to the directory with settings related to this computer for this program.
    /// </summary>
    public string ComputerSettingsAppPath = ".";

    /// <summary>
    /// Path to the directory with settings related to this computer for all programs.
    /// </summary>
    public string ComputerSettingsSysPath = ".";

    /// <summary>
    /// Path to the directory with personal preferences for this program.
    /// </summary>
    public string PersonalSettingsAppPath = ".";

    /// <summary>
    /// Path to the directory with personal preferences for all programs.
    /// </summary>
    public string PersonalSettingsSysPath = ".";

    /// <summary>
    /// Path to the directory with automatic non-configurable settings for this program.
    /// </summary>
    public string WorkspaceSettingsAppPath = ".";

    /// <summary>
    /// Path to the directory with automatic non-configurable settings for all programs.
    /// </summary>
    public string WorkspaceSettingsSysPath = ".";

    /// <summary>
    /// Path where the application is started.
    /// </summary>
    public string StartingPath = ".";
}
