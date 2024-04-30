using System.IO;

namespace Core.Gears.Settings;

[TestFixture]
public class RealSystemSettingsTest
{

    private readonly RealSystemSettings settings = RealSystemSettings.InstantiateSystemSettingsForCurrentOS();

    [OneTimeSetUp]
    public void SetupService()
    {
        settings.Setup();
    }

    [Test]
    public void UserNameExists()
    {
        settings.UserName.ShouldNotBeNullOrEmpty();
    }

    [Test]
    public void SystemPreferencesPath_Exists()
    {
        Directory.Exists(settings.SystemPreferencesPath).ShouldBeTrue();
    }

    [Test]
    public void SystemWorkspacePath_Exists()
    {
        Directory.Exists(settings.SystemWorkspacePath).ShouldBeTrue();
    }

}
