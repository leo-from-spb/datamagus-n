using System.IO;

namespace Core.Gears.Settings;

[TestFixture]
public class RealSystemSettingsTest
{

    private readonly RealSystemSettings service = RealSystemSettings.InstantiateSystemSettingsForCurrentOS();

    [OneTimeSetUp]
    public void SetupService()
    {
        service.Setup();
    }

    [Test]
    public void SystemPreferencesPath_Exists()
    {
        Directory.Exists(service.SystemPreferencesPath).ShouldBeTrue();
    }

    [Test]
    public void SystemWorkspacePath_Exists()
    {
        Directory.Exists(service.SystemWorkspacePath).ShouldBeTrue();
    }

}
