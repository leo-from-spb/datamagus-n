using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Gears.Files;


public static class FileService
{


    public static IEnumerable<FileLine> ReadFileByLines(string? basePath,
                                                        string fileName,
                                                        bool trim = false,
                                                        bool skipEmpty = false)
    {
        string path;
        if (basePath is not null)
        {
            path = Path.Combine(basePath, fileName);
        }
        else
        {
            path = fileName;
        }

        var    rawLines = File.ReadLines(path);
        try
        {
            uint nr = 0;
            foreach (string rawLine in rawLines)
            {
                uint   lineNr = ++nr;
                string str    = trim ? rawLine.Trim() : rawLine;
                if (str.Length == 0)
                    if (skipEmpty) continue;
                    else str = string.Empty;
                var fl = new FileLine(fileName, lineNr, str);
                yield return fl;
            }
        }
        finally
        {
            (rawLines as IDisposable)?.Dispose();
        }
    }


}
