using System.IO;

namespace Model.Generation;

internal abstract class MetaFileProducer
{

    protected static void WriteFile(string filePath, string text)
    {
        using (StreamWriter outputFile = new StreamWriter(filePath))
        {
            outputFile.Write(text);
        }
    }

}
