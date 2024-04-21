namespace Core.Gears.Files;

public abstract class FileTestCase
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
