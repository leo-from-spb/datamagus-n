using System;
using Util.Structures;

namespace Core.Gears.Settings;


internal abstract class RealSystemSettings : SystemSettings
{
    public abstract string UserName             { get; init; }
    public abstract string PersonalSettingsPath { get; init; }

    public Named<string?>[] ExportEntries() =>
        new Named<string?>[]
        {
            UserName.WithName("UserName")!,
            PersonalSettingsPath.WithName("PersonalSettingsPath")!
        };

    public void ImportEntry(Named<string> entry, out string? error) =>
        error = "SystemSettings cannot be imported";
}



internal sealed class MacSystemSettings : RealSystemSettings
{
    public override string UserName             { get; init; }
    public override string PersonalSettingsPath { get; init; }

    public MacSystemSettings()
    {
        UserName             = Environment.UserName;
        PersonalSettingsPath = $"/Users/{UserName}/Library/Preferences/DataMagus";
    }
}



internal sealed class UnknownOperatingSystemSettings : RealSystemSettings
{
    public override string UserName             { get; init; }
    public override string PersonalSettingsPath { get; init; }

    public UnknownOperatingSystemSettings()
    {
        UserName             = Environment.UserName;
        PersonalSettingsPath = $"{Environment.SpecialFolder.ApplicationData}/DataMagus";
    }
}