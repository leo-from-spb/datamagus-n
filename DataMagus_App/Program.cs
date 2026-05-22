using System;
using System.Linq;
using System.Threading;
using Core.Services;
using Core.Stationary;
using Core.Stationery;
using NLog;
using NLog.Config;
using NLog.Targets;
using Util.Extensions;
using static Core.Stationery.DataMagusState.ApplicationMode;

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
        ApplicationLocationsDetector.DetectLocations();
        RunInGuiMode(args);
    }

    private static void RunInGuiMode(string[] args)
    {
        DataMagusState.AppStarted(dmamGui);
        SetupLogger();
        Sunrise();

        try
        {
            DataMagus.Main.Program.RunAvaloniaApp(args);
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

        var config   = new LoggingConfiguration();
        var logLevel = debug ? LogLevel.Debug : LogLevel.Info;

        var logConsole = new ConsoleTarget("console")
                         {
                             Layout = @"${date:format=HH\:mm\:ss} ${pad:padding=5:inner=${level:uppercase=false}}  ${logger:shortName=true}: ${message}"
                         };

        config.AddRule(logLevel, LogLevel.Fatal, logConsole);
        LogManager.Configuration = config;

        Log = LogManager.GetCurrentClassLogger();
        Log.Info("DataMagus version {0} {1}", DataMagusInfo.ProductVersion, debug ? "in debug mode" : "");
        Log.Debug("Command-line options: {0}", DataMagusInfo.CommandLineOptions.ActualOptions.Select(o => o.Code).JoinToString());
    }


    private static void Sunrise()
    {
        CoreServiceMaster.Startup();
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
