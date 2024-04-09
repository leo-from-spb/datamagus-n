using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Common.Fun;

namespace Model.Gen.Generation;


internal class MetaProducer (MetaModel mm)
{
    private const string ModuleDirPath      = "./Model_Imp";
    private const string ImmDirPath         = ModuleDirPath + "/Imm";
    private const string ImmCommonFilePath  = ImmDirPath + "/ModelCommonImmImp.cs";
    private const string ImmConceptFilePath = ImmDirPath + "/ModelConceptImmImp.cs";
    private const string ImmVisualityFilePath = ImmDirPath + "/ModelVisualityImmImp.cs";


    internal void CheckDirectory()
    {
        if (!Directory.Exists(ModuleDirPath))
        {
            Console.Error.WriteLine("Wrong current directory, should be the project root.");
            Environment.Exit(-1);
        }
        if (!Directory.Exists(ImmDirPath))
        {
            Console.Error.WriteLine("Subdirectory Imp should exist.");
            Environment.Exit(-2);
        }
    }


    internal void produceCode()
    {
        produceImmutableClasses(SegmentKind.soCommon, ImmCommonFilePath);
        produceImmutableClasses(SegmentKind.soConcept, ImmConceptFilePath);
        produceImmutableClasses(SegmentKind.soVisuality, ImmVisualityFilePath);
    }


    private void produceImmutableClasses(SegmentKind segmentKind, string filePath)
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Append("""
                  using System.Collections.Generic;
                  using System.Linq;
                  using Model.Essence.Abstracts;
                  using Model.Essence.Concept;
                  using Model.Essence.Visuality;
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
        cb.Phrase("///", m.Name, @"\\\");
        cb.Phrase("public", classModifier, "class", m.Imm.ClassName, ":", m.Imm.BaseClassName + ",", m.IntfName);
        cb.Block("{", "}", () =>
                           {
                               produceImmMatterConstructor(cb, m);
                               produceImmMatterFamilies(cb, m);
                               produceImmMatterProperties(cb, m);
                           });
        cb.EmptyLine();
    }

    private void produceImmMatterConstructor(CodeBuilder cb, MetaMatter m)
    {
        var familyParameters = 
            from f in m.AllFamilies.Values
            select $"IEnumerable<{f.Child.IntfName}> {f.FamilyVarName}";
        var propertyParameters =
            from p in m.AllProperties.Values
            where p.ProName != "Id"
            select $"{p.ProTypeName} {p.ProVarName}";
        var familyAssignments =
            from f in m.AllFamilies.Values
            select $"    this.{f.FamilyVarName} = new Imm{f.FamilyTypeName}<{f.Child.IntfName}>({f.FamilyVarName}.ToArray());";
        var propertyAssignments =
            from p in m.AllProperties.Values
            where !p.ImplementedInMatter
            select $"    this.{p.ProName} = {p.ProVarName};";
        
        var parameters = new List<string>();
        parameters.AddRange(familyParameters);
        parameters.AddRange(propertyParameters);

        var assignments = new List<string>();
        assignments.AddRange(familyAssignments);
        assignments.AddRange(propertyAssignments);
        
        var baseConstructorParameters = m.HasName ? "id, name" : "id";
        cb.Append($"""
                   // Constructor \\
                   public {m.Imm.ClassName}(uint id,
                       {parameters.JoinToString(",\n    ")})
                       : base ({baseConstructorParameters}) 
                   {'{'}
                   {assignments.JoinToString("\n")}    
                   {'}'}    
                   """);
        
    }

    private void produceImmMatterFamilies(CodeBuilder cb, MetaMatter m)
    {
        cb.EmptyLine();
        cb.Phrase(@"// Families \\");        
        foreach (var f in m.AllFamilies.Values)
        {
            MetaMatter child      = f.Child;
            string     immTypeStr = $"Imm{f.FamilyTypeName}<{child.IntfName}>";
            string     iTypeStr   = $"{f.FamilyTypeName}<{child.IntfName}>";
            cb.EmptyLine();
            cb.Phrase("//", child.Names);
            cb.Append($"private readonly {immTypeStr} {f.FamilyVarName};");
            cb.Append($"public {iTypeStr} {f.FamilyName} => {f.FamilyVarName};");
        }
    }
    
    private void produceImmMatterProperties(CodeBuilder cb, MetaMatter m)
    {
        cb.EmptyLine();
        cb.Phrase(@"// Properties \\");        
        foreach (var p in m.AllProperties.Values)
        {
            if (p.ImplementedInMatter) continue;
            cb.Phrase("public", p.ProTypeName, p.ProName, "{ get; }");
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