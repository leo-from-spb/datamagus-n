using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Common.Fun;

namespace Core.ProjectStructure;

public class ProjectStructureTest
{
    private static RealDirectory  Root     = null!;
    private static string         RootPath = null!;
    private static List<FileInfo> AllFiles = new List<FileInfo>();

    private static string[] UniterestedDirs =  ["yonder", ".git", ".idea"];
    private static string[] UniterestedFiles = ["DataMagus.sln.DotSettings.user"];



    private class RealDirectory
    {
        public readonly DirectoryInfo Info;

        public RealDirectory[] Directories;
        public FileInfo[]      Files;

        public RealDirectory(DirectoryInfo info) => Info = info;

        public override string ToString() => $"{Info.FullName} + {Directories.Length} + {Files.Length}";
    }



    [OneTimeSetUp]
    public static void LocateAndLoadProjectStructure()
    {
        // look for the root directory
        var currDir = Directory.GetCurrentDirectory();
        var dir     = currDir;
        while (!isOurProjectRootDir(dir))
        {
            if (dir.Length < 3) throw new Exception($"Cannot find the project root directory when being in {currDir}");
            var parentDirInfo = Directory.GetParent(dir);
            if (parentDirInfo is null) throw new Exception($"Cannot get parent of the directory {dir}");
            dir = parentDirInfo.FullName;
        }
        RootPath = dir;

        // list all files
        LoadProjectStructure();
    }

    private static void LoadProjectStructure()
    {
        RealDirectory root = new RealDirectory(new DirectoryInfo(RootPath));
        HandleProjectDirectoryRecursively(root);
        Root = root;
    }

    private static void HandleProjectDirectoryRecursively(RealDirectory dir)
    {
        var ds = from d in dir.Info.EnumerateDirectories()
                 where !d.Name.StartsWith('.')
                    && !UniterestedDirs.Contains(d.Name)
                 orderby d.Name
                 select new RealDirectory(d);
        dir.Directories = ds.ToArray();
        var fs = from f in dir.Info.EnumerateFiles()
                 where !f.Name.StartsWith('.')
                    && !UniterestedFiles.Contains(f.Name)
                 orderby f.Name
                 select f;
        dir.Files = fs.ToArray();
        AllFiles.AddRange(dir.Files);
        foreach (var d in dir.Directories)
        {
            HandleProjectDirectoryRecursively(d);
        }
    }

    private static bool isOurProjectRootDir(string path)
    {
        return File.Exists($"{path}/DataMagus.sln");
    }


    [Test]
    public void NoImplicitUsing()
    {
        var wrongProjectFiles = new List<string>();
        var csprojFiles = from f in AllFiles
                          where f.Name.EndsWith(".csproj")
                          select f.FullName;
        foreach (var f in csprojFiles)
        {
            string text = File.ReadAllText(f);
            if (text.Contains("<ImplicitUsings>enable</ImplicitUsings>", StringComparison.OrdinalIgnoreCase))
                wrongProjectFiles.Add(f);
        }

        if (wrongProjectFiles.Count > 0)
        {
            var message = wrongProjectFiles.JoinToString(prefix: "The following project files have implicit usings enabled: \n",
                                                         separator: "\n");
            Assert.Fail(message);
        }
    }
}
