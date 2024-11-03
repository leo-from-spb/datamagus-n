using System;
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
        var mp = new MetaProducer(mm);

        mp.CheckDirectory();
        mc.CollectMetaData();
        mx.ProcessModel();
        
        Console.WriteLine("Matters:");
        foreach (var m in mm.Matters)
        {
            Console.WriteLine($"\t{m}");
        }
        
        mp.ProduceCode();
        
    }
    
}
