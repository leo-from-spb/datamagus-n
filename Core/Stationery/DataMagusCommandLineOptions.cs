using Util.Collections;

namespace Core.Stationery;



public class DataMagusCommandLineOptions : CommandLineOptions
{

    public static readonly CommandLineOption OptionDebug        = new("debug", "Work in the debug mode (including the logging level)");
    public static readonly CommandLineOption OptionLogToConsole = new("logToConsole", "Log to the console");

    public static readonly ImmOrdSet<CommandLineOption> AllCommandLineOptions =
        Imm.SetOf(OptionDebug, OptionLogToConsole);


    public DataMagusCommandLineOptions()
        : base(AllCommandLineOptions)
    { }

}
