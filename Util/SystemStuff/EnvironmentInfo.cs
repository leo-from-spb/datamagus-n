using System;
using System.IO;
using Util.Extensions;
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
        else if (Environment.OSVersion.VersionString.StartsWith("Unix") && Directory.Exists("/Volumes")) return osMac;
        else return TellLinuxFromUnix();
    }

    private static OS TellLinuxFromUnix()
    {
        const string fileName = "/proc/version";
        if (File.Exists(fileName))
        {
            string versionText = File.ReadAllText(fileName);
            return "Linux".IsIn(versionText) ? osLinux : osUnix;
        }
        else
        {
            return osUnix;
        }
    }
}



public enum OS : byte
{
    osUnknown = _0_,
    osMac     = _1_,
    osWindows = _2_,
    osUnix    = _3_,
    osLinux   = _4_
}