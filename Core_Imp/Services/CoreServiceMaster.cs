using System.Diagnostics.CodeAnalysis;
using Core.Gears.Settings;
using Core.Interaction.Commands;

namespace Core.Services;

/// <summary>
/// Controls core services.
/// </summary>
public static class CoreServiceMaster
{

    [SuppressMessage("ReSharper", "UnusedVariable")]
    public static void Sunrise()
    {
        HardServiceMill.CreateServiceMill();
        var mill = HardServiceMill.GetTheMill();

        // instantiate and register all services
        var theSettingsService = mill.Register(new LocalSettingService());
        var theCommandRegistry = mill.Register(new RealCommandRegistry());

        // set up the services
        theSettingsService.Sunrise();
        theCommandRegistry.Sunrise();
    }


    public static void Shutdown()
    {
        var mill = HardServiceMill.GetTheMillWhenInitialized();
        if (mill is not null)
        {
            var ss = ServiceMill.GetService<SettingService>();
            ((LocalSettingService)ss).SaveAllSettings();
            
            mill.ShutdownAllServices();
            mill.Dispose();
        }
    }

    public static bool IsUp() => HardServiceMill.GetTheMillWhenInitialized() is not null;


}
