using System;
using System.Linq;
using System.Threading;
using Core.Services;
using Core.Stationery;
using NLog;
using NLog.Config;
using NLog.Targets;
using Util.Extensions;

namespace DataMagus.App;


/// <summary>
/// DataMagus Application bootstrap class.
/// </summary>
public static class Program
{
    private static Logger? Log = null;

    [STAThread]
    public static void Main(string[] args)
    {
        SetupLogger();
        Sunrise();

        try
        {
            Gui.Application.Program.Main(args);
        }
        finally
        {
            Thread.Sleep(1);
            Shutdown();
            SayGoodbye();
        }
    }


    private static void SetupLogger()
    {
        bool debug = DataMagusInfo.InDebug;

        var config     = new LoggingConfiguration();
        var logLevel = debug ? LogLevel.Trace : LogLevel.Info;

        var logConsole = new ConsoleTarget("console")
                         {
                             Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${logger:shortName=true}: ${message}"
                         };

        config.AddRule(logLevel, LogLevel.Fatal, logConsole);
        LogManager.Configuration = config;

        Log = LogManager.GetLogger("Application.Boot");
        Log.Info($"DataMagus version {DataMagusInfo.ProductVersion} {(debug ? "in debug mode" : "")}");
        Log.Debug($"Command-line options: {DataMagusInfo.CommandLineOptions.ActualOptions.Select(o => o.Code).JoinToString()}");
    }


    private static void Sunrise()
    {
        CoreServiceMaster.Sunrise();
    }


    private static void Shutdown()
    {
        CoreServiceMaster.Shutdown();
    }


    private static void SayGoodbye()
    {
        Log?.Info("Goodbye!");
    }
    
}
