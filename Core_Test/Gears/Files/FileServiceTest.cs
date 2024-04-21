using System.Linq;

namespace Core.Gears.Files;

public class FileServiceTest : FileTestCase
{

    [Test]
    public void ReadFileByLines_basic()
    {
        string fileName = "Core_Test/_data_/Settings/DumbSettings.txt";

        var lineByLine = FileService.ReadFileByLines(ProjectRootPath, fileName, trim: true, skipEmpty: true);
        var lines      = lineByLine.ToList();

        lines[0].Verify
        (
            line => line.FileName.ShouldBe(fileName),
            line => line.LineNr.ShouldBe(1u),
            line => line.Content.ShouldContain("Example settings file")
        );

        lines[1].Verify
        (
            line => line.LineNr.ShouldBe(4u),
            line => line.Content.ShouldStartWith("[")
        );
    }

}
