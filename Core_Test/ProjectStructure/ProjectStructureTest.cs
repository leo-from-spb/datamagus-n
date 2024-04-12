using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Extensions;
using Util.Fun;

namespace Core.ProjectStructure;

public class ProjectStructureTest
{
    private static RealDirectory  Root => _root!;
    private static RealDirectory? _root;

    private static readonly string[] UninvolvedDirs  = ["yonder", ".git", ".idea"];
    private static readonly string[] UninvolvedFiles = ["DataMagus.sln.DotSettings.user"];



    private class RealDirectory
    {
        public readonly DirectoryInfo Info;

        public RealDirectory[] Directories = [];
        public FileInfo[]      Files       = [];

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

        // list all files
        _root = LoadProjectStructure(dir);
    }

    private static RealDirectory LoadProjectStructure(string rootPath)
    {
        RealDirectory root = new RealDirectory(new DirectoryInfo(rootPath));
        HandleProjectDirectoryRecursively(root);
        return root;
    }

    private static void HandleProjectDirectoryRecursively(RealDirectory dir)
    {
        var ds = from d in dir.Info.EnumerateDirectories()
                 where !d.Name.StartsWith('.')
                    && !UninvolvedDirs.Contains(d.Name)
                 orderby d.Name
                 select new RealDirectory(d);
        dir.Directories = ds.ToArray();
        var fs = from f in dir.Info.EnumerateFiles()
                 where !f.Name.StartsWith('.')
                    && !UninvolvedFiles.Contains(f.Name)
                 orderby f.Name
                 select f;
        dir.Files = fs.ToArray();
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
        var allDirs           = Trees.TraversDepthFirst(Root, d => d.Directories);
        var csprojFiles       = allDirs.SelectMany(d => d.Files).Where(f => f.Name.EndsWith(".csproj")).Select(f => f.FullName);
        var wrongProjectFiles = new List<string>();

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
