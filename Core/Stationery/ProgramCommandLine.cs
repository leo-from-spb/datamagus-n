using System;
using System.Linq;
using Util.Collections;

namespace Core.Stationery;

public static class ProgramCommandLine
{
    public static readonly ImmSet<string> Options;


    static ProgramCommandLine()
    {
        Options = (
            from arg in Environment.GetCommandLineArgs()
            where arg.StartsWith('-')
            select arg[1..]
        ).ToImmSet();
    }

    public static bool hasOption(string option)
    {
        return Options.Contains(option);
    }

}
