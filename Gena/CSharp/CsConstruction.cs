using System.Collections.Generic;

namespace Gena.CSharp;

/// <summary>
/// C# elements model, the root node.
/// </summary>
public class CsConstruction
{
    /// <summary>
    /// C# files.
    /// </summary>
    public IReadOnlyList<CsFile> Files => MyFiles;
    private List<CsFile> MyFiles = new();

    /// <summary>
    /// Prepares a new C# file.
    /// </summary>
    /// <param name="fileNamespace">the file namespace.</param>
    /// <param name="fileName">file name, possible with path relative to the base directory.</param>
    /// <returns>just created C# file.</returns>
    public CsFile NewFile(string fileNamespace, string fileName)
    {
        var file = new CsFile(this, fileNamespace, fileName);
        MyFiles.Add(file);
        return file;
    }

    /// <summary>
    /// Forget all files.
    /// </summary>
    public void ClearContent()
    {
        MyFiles.Clear();
    }

}
