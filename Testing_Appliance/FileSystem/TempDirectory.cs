using System;
using System.IO;

namespace Testing.Appliance.FileSystem;

public class TempDirectory : IDisposable
{
    private static readonly Random _Random = new Random();

    public DirectoryInfo Dir { get; }

    public string FullPath => Dir.FullName;

    public TempDirectory(string prefix = "Temp")
    {
        string folderName;

        lock (_Random)
        {
            folderName = prefix + _Random.Next(1000000000);
        }

        Dir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "DataMagusTesting", folderName));
    }

    public void Dispose()
    {
        Directory.Delete(Dir.FullName, true);
    }
}
