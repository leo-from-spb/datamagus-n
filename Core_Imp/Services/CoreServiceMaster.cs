using Core.Gears.Settings;
using Core.Interaction.Commands;
using Core.Stationery;

namespace Core.Services;

/// <summary>
/// Controls core services.
/// </summary>
public static class CoreServiceMaster
{

    public static void Startup()
    {
        BigServiceMill.Init();
        BigServiceMill mill = BigServiceMill.GetTheMill();

        // instantiate and register all services
        var theSettingsService = mill.Register(new LocalSettingService());
        var theCommandRegistry = mill.Register(new RealCommandRegistry());

        // set up the services
        theSettingsService.Init();
        theCommandRegistry.Init();
    }


    public static void Shutdown()
    {
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
    }

    public static bool IsUp() => BigServiceMill.GetTheMillWhenInitialized() is not null;


}
