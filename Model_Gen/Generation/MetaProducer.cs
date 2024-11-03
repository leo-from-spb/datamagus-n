using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Extensions;
using Util.Fun;

namespace Model.Generation;


internal class MetaProducer (MetaModel mm)
{
    private const string ModuleDirPath        = "./Model_Imp";
    private const string ImmDirPath           = ModuleDirPath + "/_generated_";
    private const string ImmCommonFilePath    = ImmDirPath + "/ModelCommonImmImp.cs";
    private const string ImmConceptFilePath   = ImmDirPath + "/ModelConceptImmImp.cs";
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


    internal void ProduceCode()
    {
        ProduceImmutableClasses(SegmentKind.soCommon, ImmCommonFilePath);
        ProduceImmutableClasses(SegmentKind.soConcept, ImmConceptFilePath);
        ProduceImmutableClasses(SegmentKind.soVisuality, ImmVisualityFilePath);
    }


    private void ProduceImmutableClasses(SegmentKind segmentKind, string filePath)
    {
        CodeBuilder cb = new CodeBuilder();
        cb.Append("""
                  using System.Collections.Generic;
                  using System.Linq;
                  using Model.Abstracts;
                  using Model.Concept;
                  using Model.Visuality;
                  
                  namespace Model.Imm;
                  """);
        cb.EmptyLine();

        var segmentMatters = from m in mm.Matters
                             where m.SegmKind == segmentKind
                                && !m.Imm.ManuallyImplemented
                                && !m.IsAbstract   
                             select m;

        foreach (var matter in segmentMatters)
        {
            ProduceImmMatter(cb, matter);
        }
        
        WriteFile(filePath, cb.Result);
    }

    private void ProduceImmMatter(CodeBuilder cb, MetaMatter m)
    {
        string classModifier = m.IsConcrete ? "sealed" : "abstract";
        cb.EmptyLine();
        cb.Phrase("///", m.Name, @"\\\");
        cb.Phrase("public", classModifier, "class", m.Imm.ClassName, ":", m.Imm.BaseClassName + ",", m.IntfName);
        cb.Block("{", "}", () =>
                           {
                               ProduceLocalVariablesOnTop(cb, m);
                               ProduceImmMatterConstructor(cb, m);
                               ProduceImmMatterFamilies(cb, m);
                               ProduceImmMatterProperties(cb, m);
                           });
        cb.EmptyLine();
    }

    private void ProduceLocalVariablesOnTop(CodeBuilder cb, MetaMatter m)
    {
        if (m.IsMedium && m.IsConcrete)
        {
            cb.Phrase("private readonly Family<Matter>[] families;");
            cb.Phrase("public override IReadOnlyList<Family<Matter>> Families => families;");
            cb.EmptyLine();
        }
    }

    private void ProduceImmMatterConstructor(CodeBuilder cb, MetaMatter m)
    {
        var familyParameters =
            from f in m.AllFamilies.Values
            select $"IEnumerable<{f.Child.IntfName}> {f.FamilyVarName}";
        var propertyParameters =
            from p in m.AllProperties.Values
            where p.ProName != "Id" && p.ProName != "Version"
            select $"{p.ProTypeName} {p.ProVarName}";
        var familyAssignments =
            from f in m.AllFamilies.Values
            select $"this.{f.FamilyVarName} = new Imm{f.FamilyTypeName}<{f.Child.IntfName}>({f.FamilyVarName}.ToArray());";
        var propertyAssignments =
            from p in m.AllProperties.Values
            where !p.ImplementedInMatter
            select $"this.{p.ProName} = {p.ProVarName};";

        var parameters = new List<string>();
        parameters.AddRange(familyParameters);
        parameters.AddRange(propertyParameters);

        var assignments = new List<string>();
        assignments.AddRange(familyAssignments);
        assignments.AddRange(propertyAssignments);

        var realConstructorParameters = m.HasName ? "uint id, uint version, string? name," : "uint id, uint version,";
        var baseConstructorParameters = m.HasName ? "id, version, name" : "id, version";
        var thisFamilies              = m.AllFamilies.Keys.Select(name => "this." + name.Decap()).JoinToString();

        cb.Phrase(@"// Constructor \\");
        cb.Phrase("public", m.Imm.ClassName, "(" + realConstructorParameters);
        cb.Indent();
        for (var i = 0; i < parameters.Count; i++) cb.Phrase(parameters[i] + (i < parameters.Count - 1 ? "," : ")"));
        cb.Phrase(": base(", baseConstructorParameters, ")");
        cb.Unindent();
        cb.Phrase("{");
        cb.Indent();
        assignments.ForEach(a => cb.Phrase(a));
        if (m.IsMedium && m.IsConcrete) cb.Phrase("families = new Family<Matter>[] {", thisFamilies, "};");
        cb.Unindent();
        cb.Phrase("}");
    }

    private void ProduceImmMatterFamilies(CodeBuilder cb, MetaMatter m)
    {
        if (m.AllFamilies.IsEmpty()) return;
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

    private void ProduceImmMatterProperties(CodeBuilder cb, MetaMatter m)
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