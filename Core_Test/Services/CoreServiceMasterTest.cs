using Core.Gears.Settings;
using Core.Stationery;
using Core.TestingInCore;

namespace Core.Services;


[TestFixture]
public class CoreServiceMasterTest : CoreCase
{

    [OneTimeSetUp]
    public static void Setup()
    {
        DataMagusState.AppStarted(DataMagusState.ApplicationMode.dmamCommandLine);
    }


    [Test]
    public void Basic_SunriseAndShutdown()
    {
        CoreServiceMaster.IsUp().ShouldBeFalse();

        CoreServiceMaster.Startup();

        CoreServiceMaster.IsUp().ShouldBeTrue();

        var settingService = GetService<SettingService>();
        settingService.ShouldNotBeNull();

        CoreServiceMaster.Shutdown();

        CoreServiceMaster.IsUp().ShouldBeFalse();
    }

}
