using Core.Gears.Settings;
using Core.Stationery;

namespace Core.Services;


[TestFixture]
public class CoreServiceMasterTest
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

        CoreServiceMaster.Sunrise();

        CoreServiceMaster.IsUp().ShouldBeTrue();

        var settingService = GetService<SettingService>();
        settingService.ShouldNotBeNull();

        CoreServiceMaster.Shutdown();

        CoreServiceMaster.IsUp().ShouldBeFalse();
    }

}
