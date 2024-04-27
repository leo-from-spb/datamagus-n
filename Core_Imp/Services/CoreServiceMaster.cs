using Core.Gears.Settings;

namespace Core.Services;

/// <summary>
/// Controls core services.
/// </summary>
public static class CoreServiceMaster
{

    public static void Sunrise()
    {
        HardServiceMill.CreateServiceMill();
        var mill = HardServiceMill.GetTheMill();

        // instantiate and register all services
        var theSettingsService = mill.Register(new LocalSettingService());

        // set up the services
        theSettingsService.Sunrise();
    }


    public static void Shutdown()
    {
        var mill = HardServiceMill.GetTheMillWhenInitialized();
        mill?.ShutdownAllServices();
        mill?.Dispose();
    }

    public static bool IsUp() => HardServiceMill.GetTheMillWhenInitialized() is not null;


}
