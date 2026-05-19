using System;
using System.IO;
using Core.Stationery;
using Util.SystemStuff;
using static Util.SystemStuff.OS;

namespace Core.Stationary;

public static class ApplicationLocationsDetector
{
    public static void DetectLocations()
    {
        var locations = DataMagusState.Locations;
        DetectLocations(locations);
    }

    internal static void DetectLocations(ApplicationLocations locations)
    {
        locations.StartingPath = Directory.GetCurrentDirectory();

        switch(EnvironmentInfo.OS)
        {
            case osMac:
                DetectLocationsInMacOS(locations);
                break;
            case osWindows:
                DetectLocationsInWindows(locations);
                break;
            case osLinux:
                DetectLocationsInLinux(locations);
                break;
            case osUnix:
                DetectLocationsInUnix(locations);
                break;
            default:
                DetectLocationsInUnknownSystem(locations);
                break;
        }

        locations.ComputerSettingsAppPath =
            Path.Combine(locations.ComputerSettingsAppPath, ApplicationLocations.ApplicationSettingsDirectoryName);
        locations.PersonalSettingsAppPath =
            Path.Combine(locations.PersonalSettingsSysPath, ApplicationLocations.ApplicationSettingsDirectoryName);
        locations.WorkspaceSettingsAppPath =
            Path.Combine(locations.WorkspaceSettingsSysPath, ApplicationLocations.ApplicationSettingsDirectoryName);
    }


    internal static void DetectLocationsInMacOS(ApplicationLocations locations)
    {
        var userName = Environment.UserName;
        locations.PersonalSettingsSysPath  = $"/Users/{userName}/Library/Preferences";
        locations.ComputerSettingsSysPath  = locations.PersonalSettingsSysPath; // TODO find the shared path
        locations.WorkspaceSettingsSysPath = $"/Users/{userName}/Library/Application Support";
    }

    internal static void DetectLocationsInWindows(ApplicationLocations locations)
    {
        var userName = Environment.UserName;
        locations.PersonalSettingsSysPath  = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        locations.ComputerSettingsSysPath  = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        locations.WorkspaceSettingsSysPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }

    internal static void DetectLocationsInLinux(ApplicationLocations locations)
    {
        DetectLocationsInUnix(locations);
    }

    internal static void DetectLocationsInUnix(ApplicationLocations locations)
    {
        string userName = Environment.UserName;
        string homePath = Environment.GetEnvironmentVariable("HOME") ?? $"/home/{userName}";
        locations.PersonalSettingsSysPath  = homePath;
        locations.ComputerSettingsSysPath  = homePath;
        locations.WorkspaceSettingsSysPath = homePath;
    }

    internal static void DetectLocationsInUnknownSystem(ApplicationLocations locations)
    {
        var userName = Environment.UserName;
        locations.PersonalSettingsSysPath  = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        locations.ComputerSettingsSysPath  = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        locations.WorkspaceSettingsSysPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }


}
