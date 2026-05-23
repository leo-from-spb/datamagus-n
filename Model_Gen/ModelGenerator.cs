using System;
using System.IO;
using Gena.CSharp;
using Model.Generation;

namespace Model;

internal static class ModelGenerator
{
    private static void Main()
    {
        Console.WriteLine("Model Generator");

        var mm = new MetaModel();
        var mc = new MetaCollector(mm);
        var mx = new MetaProcessor(mm);
        var mt = new MetaMason(mm);

        CheckDirectory();

        mc.CollectMetaData();
        mx.ProcessModel();
        mt.ConstructCsModel();

        Console.WriteLine("Matters:");
        foreach (var m in mm.Matters)
        {
            Console.WriteLine($"\t{m}");
        }

        foreach (var csFile in mt.Construction.Files)
        {
            var producer = new CsProducer();
            producer.ProduceFile(csFile);
            string text = producer.ResultText;
            WriteFile(csFile.FileName, text);

        }
    }


    private static void CheckDirectory()
    {
        if (!Directory.Exists(MetaConsts.ModuleDirPath))
        {
            Console.Error.WriteLine("Wrong current directory, should be the project root.");
            Environment.Exit(-1);
        }
        if (!Directory.Exists(MetaConsts.ImmDirPath))
        {
            Console.Error.WriteLine("Subdirectory Imp should exist.");
            Environment.Exit(-2);
        }
    }


    private static void WriteFile(string filePath, string text)
    {
        using (StreamWriter outputFile = new StreamWriter(filePath))
        {
            outputFile.Write(text);
        }
    }

}
