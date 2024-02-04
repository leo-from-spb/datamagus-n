using Model.Gen.Generation;

namespace Model.Gen;

internal static class ModelGenerator
{
    private static void Main()
    {
        Console.WriteLine("Model Generator");

        var mm = new MetaModel();
        var mc = new MetaCollector(mm);
        
        mc.CollectMetaData();

        Console.WriteLine("Matters:");
        foreach (var m in mm.Matters)
        {
            Console.WriteLine($"\t{m}");
        }
    }
    
}
