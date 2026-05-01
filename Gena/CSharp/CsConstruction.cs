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
    private List<CsFile> Files = new();

    public CsFile NewFile(string fileNamespace, string fileName)
    {
        var file = new CsFile(this, fileNamespace, fileName);
        Files.Add(file);
        return file;
    }

    /// <summary>
    /// Forget all files.
    /// </summary>
    public void ClearContent()
    {
        Files.Clear();
    }

}
