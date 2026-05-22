using Core.Gears.Settings;
using Core.Interaction.Commands;
using Core.Stationery;
using NLog;

namespace Core.Services;

/// <summary>
/// Controls core services.
/// </summary>
public static class CoreServiceMaster
{

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Startup()
    {
        Log.Debug("Core Service Master started");
        BigServiceMill.Init();
        BigServiceMill mill = BigServiceMill.GetTheMill();


        // instantiate and register all services
        Log.Debug("Instantiating services...");
        var theSettingsService = mill.Register(new LocalSettingService());
        var theCommandRegistry = mill.Register(new RealCommandRegistry());

        // set up the services
        Log.Debug("Init services...");
        theSettingsService.Init();
        theCommandRegistry.Init();

        Log.Debug("Core services are initialized.");
    }


    public static void Shutdown()
    {
        Log.Debug("Core Service Master shutting down...");

        var mill = BigServiceMill.GetTheMillWhenInitialized();
        if (mill is not null)
        {
            if (DataMagusState.AppMode == DataMagusState.ApplicationMode.dmamGui)
            {
                var ss = ServiceMill.GetService<SettingService>();
                ((LocalSettingService)ss).SaveAllSettings();
            }

            mill.ShutdownAllServices();
            mill.Dispose();
        }

        Log.Debug("Core Service Master shut down complete.");
    }

    public static bool IsUp() => BigServiceMill.GetTheMillWhenInitialized() is not null;


}
