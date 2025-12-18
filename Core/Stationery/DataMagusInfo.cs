using System;

namespace Core.Stationery;

/// <summary>
/// Top-level information about the software.
/// </summary>
public static class DataMagusInfo
{
    /// <summary>
    /// Current version.
    /// </summary>
    public static readonly Version ProductVersion = new Version(0, 1);

    /// <summary>
    /// The command-line options.
    /// </summary>
    public static readonly DataMagusCommandLineOptions CommandLineOptions = new();

    /// <summary>
    /// The application is in the debug mode.
    /// </summary>
    public static readonly bool InDebug = CommandLineOptions.Has(DataMagusCommandLineOptions.OptionDebug);
}
