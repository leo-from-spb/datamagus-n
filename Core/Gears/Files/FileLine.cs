using System;
using static System.String;

namespace Core.Gears.Files;


/// <summary>
/// Represents one line of a file.
/// The purpose is to be red from a file.
/// </summary>
public readonly record struct FileLine : IComparable<FileLine>
{
    /// <summary>
    /// Name of the file
    /// </summary>
    public readonly string FileName;

    /// <summary>
    /// Line number, starting with 1.
    /// </summary>
    public readonly uint LineNr;

    /// <summary>
    /// The line itself.
    /// Can be empty, but not null.
    /// </summary>
    public readonly string Content;

    /// <summary>
    /// Instantiates a file string.
    /// </summary>
    /// <param name="fileName">name of the file.</param>
    /// <param name="lineNr">line number, starting with 1.</param>
    /// <param name="content">the line itself.</param>
    public FileLine(string fileName, uint lineNr, string content)
    {
        FileName = fileName;
        LineNr   = lineNr;
        Content  = content;
    }


    public int CompareTo(FileLine that)
    {
        int z         = Compare(this.FileName, that.FileName, StringComparison.Ordinal);
        if (z == 0) z = this.LineNr.CompareTo(that.LineNr);
        if (z == 0) z = Compare(this.Content, that.Content, StringComparison.Ordinal);
        return z;
    }

    public override string ToString() => $"{FileName}:{LineNr}|{Content}";

}
