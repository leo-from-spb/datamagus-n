using System;
using System.Collections.Generic;
using Util.Structures;

namespace Core.Gears.Settings;


internal abstract class RealSystemSettings : SystemSettings
{
    public abstract string UserName             { get; init; }
    public abstract string PersonalSettingsPath { get; init; }

    public IEnumerable<Named<string>> ListEntries() =>
        new[]
        {
            UserName.WithName("UserName"),
            PersonalSettingsPath.WithName("PersonalSettingsPath")
        };
}



internal class MacSystemSettings : RealSystemSettings
{
    public override string UserName             { get; init; }
    public override string PersonalSettingsPath { get; init; }

    public MacSystemSettings()
    {
        UserName             = Environment.UserName;
        PersonalSettingsPath = $"/Users/{UserName}/Library/Preferences/DataMagus";
    }
}



internal class UnknownOperatingSystemSettings : RealSystemSettings
{
    public override string UserName             { get; init; }
    public override string PersonalSettingsPath { get; init; }

    public UnknownOperatingSystemSettings()
    {
        UserName             = Environment.UserName;
        PersonalSettingsPath = $"{Environment.SpecialFolder.ApplicationData}/DataMagus";
    }
}