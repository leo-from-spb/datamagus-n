using Core.TestingInCore;

namespace Core.Gears.Files;



public abstract class FileTestCase : CoreCase
{

    protected static string? ProjectRootPath;

    [OneTimeSetUp]
    public void Init()
    {
        if (ProjectRootPath is null)
        {
            ProjectRootPath = Testing.Appliance.ProjectStructure.LocateProjectRootDirectory();
        }
    }


}
