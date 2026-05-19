using System.IO;
using Core.Stationary;

namespace Core.Stationery;

[TestFixture]
public class ApplicationLocationsDetectorTest {

    private readonly ApplicationLocations Locations = new();

    [OneTimeSetUp]
    public void SetupService()
    {
        ApplicationLocationsDetector.DetectLocations(Locations);
    }

    [Test]
    public void ComputerSettingsSysPath_Exists()
    {
        Directory.Exists(Locations.ComputerSettingsSysPath).ShouldBeTrue();
    }

    [Test]
    public void PersonalSettingsSysPath_Exists()
    {
        Directory.Exists(Locations.PersonalSettingsSysPath).ShouldBeTrue();
    }

    [Test]
    public void WorkspaceSettingsSysPath_Exists()
    {
        Directory.Exists(Locations.WorkspaceSettingsSysPath).ShouldBeTrue();
    }

}
