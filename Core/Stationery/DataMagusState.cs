using System;
using static Util.Fun.NumberConstants;

namespace Core.Stationery;

/// <summary>
/// Application state.
/// </summary>
public static class DataMagusState
{
    /// <summary>
    /// How the DataMagus application was started.
    /// </summary>
    public static ApplicationMode AppMode { get; private set; }

    public static void AppStarted(ApplicationMode mode)
    {
        if (AppMode != ApplicationMode.dmamNone)
            throw new InvalidOperationException($"Cannot re-start DataMagus Application in mode {mode}, it's already started in mode {AppMode}");
        AppMode = mode;
    }


    /// <summary>
    /// How a DataMagus application could be started.
    /// </summary>
    public enum ApplicationMode : byte
    {
        dmamNone        = _0_,
        dmamAvalonia    = _1_,
        dmamCommandLine = _2_,
        dmamGui         = _3_
    }

}



