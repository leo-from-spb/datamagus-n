using System;
using System.IO;

namespace Testing.Appliance.FileSystem;

/// <summary>
/// A disposable temporary directory for use in tests.
/// Creates a uniquely named directory under the system temp path on construction
/// and recursively deletes it on disposal.
/// </summary>
public class TempDirectory : IDisposable
{
    private static readonly Random _Random = new Random();

    /// <summary>
    /// The underlying <see cref="DirectoryInfo"/> of the temporary directory.
    /// </summary>
    public DirectoryInfo Dir { get; }

    /// <summary>
    /// The full absolute path of the temporary directory.
    /// </summary>
    public string FullPath => Dir.FullName;

    /// <summary>
    /// Creates a new temporary directory under <c>{TempPath}/DataMagusTesting/</c>
    /// with a name composed of the given <paramref name="prefix"/> and a random number.
    /// </summary>
    /// <param name="prefix">prefix for the directory name (default is <c>"Temp"</c>).</param>
    public TempDirectory(string prefix = "Temp")
    {
        string folderName;

        lock (_Random)
        {
            folderName = prefix + _Random.Next(1000000000);
        }

        Dir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "DataMagusTesting", folderName));
    }

    /// <summary>
    /// Recursively deletes the temporary directory and all its contents.
    /// </summary>
    public void Dispose()
    {
        Directory.Delete(Dir.FullName, true);
    }
}
