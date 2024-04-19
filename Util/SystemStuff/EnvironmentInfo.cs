using System;
using System.IO;
using static Util.Fun.NumberConstants;
using static Util.SystemStuff.OS;

namespace Util.SystemStuff;


public static class EnvironmentInfo
{

    public static readonly OS OS;

    static EnvironmentInfo()
    {
        OS = DetectOS();
    }

    private static OS DetectOS()
    {
        if (Environment.NewLine == "\r\n") return osWindows;
        else if (Environment.OSVersion.VersionString.StartsWith("Unix")) return Directory.Exists("/Volumes") ? osMac : osUnix;
        else return osUnknown;
    }
}



public enum OS : byte
{
    osUnknown = _0_,
    osMac     = _1_,
    osWindows = _2_,
    osUnix    = _3_
}