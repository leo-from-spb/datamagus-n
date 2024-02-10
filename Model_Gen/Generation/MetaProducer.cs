using System.Collections.Immutable;
using Model.Essence.Abstracts;
using Util.Common.Fun;

namespace Model.Gen.Generation;


internal class MetaProducer (MetaModel mm)
{
    private const string ModuleDirPath      = "./Model_Imp";
    private const string ImmDirPath         = ModuleDirPath + "/Imm";
    private const string ImmCommonFilePath  = ImmDirPath + "/ModelCommonImmImp.cs";
    private const string ImmConceptFilePath = ImmDirPath + "/ModelConceptImmImp.cs";

    private readonly ImmutableHashSet<string> ManualImplementedMatters = [ nameof(Matter), 
                                                                           nameof(MediumMatter),
                                                                           nameof(TermMatter),
                                                                           nameof(NamedMatter),
                                                                           nameof(NamedMediumMatter),
                                                                           nameof(NamedTermMatter) ]; 
    
    
    internal void CheckDirectory()
    {
        if (!Directory.Exists(ModuleDirPath))
        {
            Console.Error.WriteLine("Wrong current directory, should be the project root.");
            Environment.Exit(-1);
        }
        if (!Directory.Exists(ImmDirPath))
        {
            Console.Error.WriteLine("Subdirectory Imp shoudl exist.");
            Environment.Exit(-2);
        }
    }


    internal void produceCode()
    {
        produceImmutableClasses(SegmentKind.soCommon, ImmCommonFilePath);
        produceImmutableClasses(SegmentKind.soConcept, ImmConceptFilePath);
    }


    private void produceImmutableClasses(SegmentKind segmentKind, string filePath)
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Append("""
                  using Model.Essence.Abstracts;
                  using Model.Essence.Concept;
                  using Model.Imp.Abstracts;
                  
                  namespace Model.Imp.Imm;
                  """);
        cb.EmptyLine();

        var segmentMatters = from m in mm.Matters
                             where m.SegmKind == segmentKind
                                && !m.Imm.ManuallyImplemented
                                && !m.IsAbstract   
                             select m;

        foreach (var matter in segmentMatters)
        {
            produceImmMatter(cb, matter);
        }
        
        WriteFile(filePath, cb.Result);
    }

    private void produceImmMatter(CodeBuilder cb, MetaMatter m)
    {
        string classModifier = m.IsConcrete ? "sealed" : "abstract";

        cb.EmptyLine();
        cb.Phrase("public", classModifier, "class", m.Imm.ClassName, ":", m.Imm.BaseClassName+",", m.IntfName);
        cb.Phrase("{");
        cb.Indent();
        produceImmMatterFamilies(cb, m);
        cb.Unindent();
        cb.Phrase("}");
        cb.EmptyLine();
    }

    private void produceImmMatterFamilies(CodeBuilder cb, MetaMatter m)
    {
        
    }


    private static void WriteFile(string filePath, string text)
    {
        using (StreamWriter outputFile = new StreamWriter(filePath))
        {
            outputFile.Write(text);
        }        
    }
}