using System;
using System.IO;

namespace Testing.Appliance;


public static class ProjectStructure
{

    public static string LocateProjectRootDirectory()
    {
        var currDir = Directory.GetCurrentDirectory();
        var dir     = currDir;
        while (!IsOurProjectRootDir(dir))
        {
            if (dir.Length < 3) throw new Exception($"Cannot find the project root directory when being in {currDir}");
            var parentDirInfo = Directory.GetParent(dir);
            if (parentDirInfo is null) throw new Exception($"Cannot get parent of the directory {dir}");
            dir = parentDirInfo.FullName;
        }
        return dir;
    }

    private static bool IsOurProjectRootDir(string path)
    {
        return File.Exists($"{path}/DataMagus.sln");
    }

}
