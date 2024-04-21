using Core.Gears.Settings;

namespace Core.Services;


[TestFixture]
public class CoreServiceMasterTest
{

    [Test]
    public void Basic_SunriseAndShutdown()
    {
        CoreServiceMaster.IsUp().ShouldBeFalse();

        CoreServiceMaster.Sunrise();

        CoreServiceMaster.IsUp().ShouldBeTrue();

        var settingService = ServiceMill.GetService<SettingService>();
        settingService.ShouldNotBeNull();
        settingService.SystemSettings.ActualComputerSettingsPath.Length.ShouldBeGreaterThan(3);
        settingService.SystemSettings.ActualPersonalSettingsPath.Length.ShouldBeGreaterThan(3);

        CoreServiceMaster.Shutdown();

        CoreServiceMaster.IsUp().ShouldBeFalse();
    }

}
