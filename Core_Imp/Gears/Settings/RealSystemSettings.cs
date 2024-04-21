using System;
using System.IO;
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
        new SettingPair(nameof(UserName), UserName),
        new SettingPair(nameof(SystemPreferencesPath), SystemPreferencesPath),
        new SettingPair(nameof(SystemWorkspacePath), SystemWorkspacePath),
        new SettingPair(nameof(ActualPersonalSettingsPath), ActualPersonalSettingsPath),
        new SettingPair(nameof(ActualComputerSettingsPath), ActualComputerSettingsPath)
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
        SystemPreferencesPath = $"/Users/{UserName}/Library/Preferences/DataMagus";
        SystemWorkspacePath   = $"/Users/{UserName}/Library/Preferences/DataMagus";
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
        SystemPreferencesPath = $"{Environment.SpecialFolder.ApplicationData}/DataMagus";
        SystemWorkspacePath   = $"{Environment.SpecialFolder.ApplicationData}/DataMagus";
    }
}