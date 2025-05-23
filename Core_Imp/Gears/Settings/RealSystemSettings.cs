using System;
using System.IO;
using Util.SystemStuff;
using static Core.Stationery.DataMagusEnvironmentVariables;

namespace Core.Gears.Settings;


internal abstract class RealSystemSettings : SystemSettings
{
    public abstract string UserName              { get; init; }
    public abstract string SystemPreferencesPath { get; init; }
    public abstract string SystemWorkspacePath   { get; init; }

    public string ActualPersonalSettingsPath { get; set; } = ".";
    public string ActualComputerSettingsPath { get; set; } = ".";

    private const string DataMagusDirectoryName = "DataMagus";


    internal static RealSystemSettings InstantiateSystemSettingsForCurrentOS()
    {
        return EnvironmentInfo.OS switch
               {
                   OS.osMac     => new MacSystemSettings(),
                   OS.osWindows => new WindowsSystemSettings(),
                   OS.osUnix    => new UnixSystemSettings(),
                   OS.osLinux   => new UnixSystemSettings(),
                   _            => new UnknownOperatingSystemSettings()
               };
    }


    internal void Setup()
    {
        string? evConfigDir    = Environment.GetEnvironmentVariable(EvarConfigDir);
        string? evWorkspaceDir = Environment.GetEnvironmentVariable(EvarWorkspaceDir);

        string baseConfigDir    = evConfigDir ?? SystemPreferencesPath;
        string baseWorkspaceDir = evWorkspaceDir ?? SystemWorkspacePath;

        ActualPersonalSettingsPath = Path.Combine(baseConfigDir, DataMagusDirectoryName);
        ActualComputerSettingsPath = Path.Combine(baseWorkspaceDir, DataMagusDirectoryName);
    }


    public SettingPair[] ExportEntries() =>
    [
        new (nameof(UserName), UserName),
        new (nameof(SystemPreferencesPath), SystemPreferencesPath),
        new (nameof(SystemWorkspacePath), SystemWorkspacePath),
        new (nameof(ActualPersonalSettingsPath), ActualPersonalSettingsPath),
        new (nameof(ActualComputerSettingsPath), ActualComputerSettingsPath)
    ];

    public void ImportEntry(SettingPair entry, out string? error) =>
        error = "SystemSettings cannot be imported";
}



internal sealed class MacSystemSettings : RealSystemSettings
{
    public override string UserName              { get; init; }
    public override string SystemPreferencesPath { get; init; }
    public override string SystemWorkspacePath   { get; init; }

    public MacSystemSettings()
    {
        UserName              = Environment.UserName;
        SystemPreferencesPath = $"/Users/{UserName}/Library/Preferences";
        SystemWorkspacePath   = $"/Users/{UserName}/Library/Application Support";
    }
}


internal sealed class WindowsSystemSettings : RealSystemSettings
{
    public override string UserName              { get; init; }
    public override string SystemPreferencesPath { get; init; }
    public override string SystemWorkspacePath   { get; init; }

    public WindowsSystemSettings()
    {
        UserName              = Environment.UserName;
        SystemPreferencesPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        SystemWorkspacePath   = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}


internal sealed class UnixSystemSettings : RealSystemSettings
{
    public override string UserName              { get; init; }
    public override string SystemPreferencesPath { get; init; }
    public override string SystemWorkspacePath   { get; init; }

    public UnixSystemSettings()
    {
        UserName = Environment.UserName;
        string homePath = Environment.GetEnvironmentVariable("HOME") ?? $"/home/{UserName}";

        SystemPreferencesPath = homePath;
        SystemWorkspacePath   = homePath;
    }
}


internal sealed class UnknownOperatingSystemSettings : RealSystemSettings
{
    public override string UserName              { get; init; }
    public override string SystemPreferencesPath { get; init; }
    public override string SystemWorkspacePath   { get; init; }

    public UnknownOperatingSystemSettings()
    {
        UserName              = Environment.UserName;
        SystemPreferencesPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        SystemWorkspacePath   = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}