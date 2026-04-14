using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Util.Collections;
using Util.Extensions;
using Util.Fun;
using static Model.Generation.MetaConsts;

namespace Model.Generation;



internal class MetaProducer (MetaModel mm) : MetaFileProducer
{

    private readonly ImmSortedSet<string> usingsForAll =
        Imm.SortedSetOf
        (
            "System.Collections.Generic",
            "System.Linq",
            "Model.Abstracts",
            "Util.Collections"
        );

    private readonly ImmSortedSet<string> usingsForCommon =
        Imm.SortedSetOf
        (
            "Model.Concept",
            "Model.Visuality"
        );

    private readonly ImmSortedSet<string> usingsForConcept =
        Imm.SortedSetOf
        (
            "Model.Concept"
        );

    private readonly ImmSortedSet<string> usingsForDB =
        Imm.SortedSetOf
        (
            "Model.Concept"
        );

    private readonly ImmSortedSet<string> usingsForVisuality =
        Imm.SortedSetOf
        (
            "Model.Concept",
            "Model.Visuality",
            "Util.Geometry"
        );


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
        ProduceImmutableClasses(SegmentKind.soAbstracts, ImmAbstractsFilePath);
        ProduceImmutableClasses(SegmentKind.soConcept, ImmConceptFilePath);
        ProduceImmutableClasses(SegmentKind.soVisuality, ImmVisualityFilePath);
    }


    private void ProduceImmutableClasses(SegmentKind segmentKind, string filePath)
    {
        var usings = CollectUsings(segmentKind).ToImmSortedSet();

        CodeBuilder cb = new CodeBuilder();
        cb.Text(GeneratedFileHeader);
        cb.EmptyLine();

        foreach (var usingNamespace in usings)
            cb.Phrase("using ", usingNamespace, ";");
        cb.EmptyLine();
        cb.Phrase("namespace", SegmentNamespace(segmentKind), ";");
        cb.EmptyLine();

        var segmentMatters = from m in mm.Matters
                             where m.SegmKind == segmentKind
                                && m.Imm.ToImplement
                             select m;

        foreach (var matter in segmentMatters)
        {
            ProduceImmMatter(cb, matter);
        }
        
        WriteFile(filePath, cb.Result);
    }

    private ImmSet<string> CollectUsings(SegmentKind segmentKind) =>
        segmentKind switch
        {
            SegmentKind.soAbstracts => usingsForAll + usingsForCommon,
            SegmentKind.soConcept   => usingsForAll + usingsForConcept,
            SegmentKind.soDB        => usingsForAll + usingsForDB,
            SegmentKind.soVisuality => usingsForAll + usingsForVisuality,
            _                       => throw new ArgumentOutOfRangeException(nameof(segmentKind), segmentKind, null)
        };

    private string SegmentNamespace(SegmentKind segmentKind) =>
        segmentKind switch
        {
            SegmentKind.soAbstracts => "Model.Abstracts",
            SegmentKind.soConcept   => "Model.Concept",
            SegmentKind.soDB        => "Model.DB",
            SegmentKind.soVisuality => "Model.Visuality",
            _                       => throw new ArgumentOutOfRangeException(nameof(segmentKind), segmentKind, null)
        };

    private void ProduceImmMatter(CodeBuilder cb, MetaMatter m)
    {
        string classModifier = m.IsConcrete ? "sealed" : "abstract";
        cb.EmptyLine();
        cb.Phrase("#region", "matter class", m.Imm.ClassName);
        cb.EmptyLine();
        ProduceImmMatterGreatComment(cb, m);
        cb.Phrase("public", classModifier, "class", m.Imm.ClassName, ":", m.Imm.BaseClassName + ",", m.IntfName);

        using (cb.CurlyBlock(true))
        {
            ProduceLocalVariablesOnTop(cb, m);
            ProduceImmMatterConstructor(cb, m);
            ProduceImmMatterFamilies(cb, m);
            ProduceImmMatterReferences(cb, m);
            ProduceImmMatterProperties(cb, m);
        }
        
        cb.Phrase("#endregion");
        cb.EmptyLine();
    }

    private static void ProduceImmMatterGreatComment(CodeBuilder cb, MetaMatter m)
    {
        using (cb.Indenting("/// "))
        {
            cb.Phrase("<summary>");
            cb.Phrase("Immutable matter", m.Name.InTag("b"), "interface", $"<see cref='{m.IntfName}'/>", "<br/>");

            cb.Phrase("Base matters:");
            cb.Phrase("<ul>");
            foreach (var bm in m.DeclaredBaseMatters.OrderBy(b => b.OrderNum))
            {
                var interfaceStr = $"interface <see cref='{bm.Intf.Name}'/>";
                var imm          = bm.Imm;
                var classStr     = imm.ToImplement || imm.ManuallyImplemented ? $", class <see cref='{imm.ClassName}'/>" : null;
                cb.Phrase("    <li>", bm.Name.InTag("b"), ":", interfaceStr, classStr, "</li>");
            }
            cb.Phrase("</ul>");

            var families = m.AllFamilies.Values;
            if (families.IsNotEmpty())
            {
                cb.Phrase("Families:");
                cb.Phrase("<ul>");
                foreach (var f in families)
                    cb.Phrase("    <li>", f.FamilyName.InTag("b"), "</li>");
                cb.Phrase("</ul>");
            }

            cb.Phrase("</summary>");
        }
    }

    private void ProduceLocalVariablesOnTop(CodeBuilder cb, MetaMatter m)
    {
        if (m.IsMedium && m.IsConcrete)
        {
            cb.Text("public override ImmList<Family<Matter>> Families { get; }");
            cb.EmptyLine();
        }
    }

    private void ProduceImmMatterConstructor(CodeBuilder cb, MetaMatter m)
    {
        var familyParameters =
            from f in m.AllFamilies.Values
            select $"IEnumerable<{f.Child.IntfName}> {f.FamilyVarName}";
        var refParameters =
            from r in m.AllRefs.Values
            select $"{r.InnerTypeSpec} {r.RefLowName}";
        var propertyParameters =
            from p in m.AllProperties.Values
            where p.ProName != "Id" && p.ProName != "Version"
            select $"{p.ProTypeName} {p.ProVarName}";
        var familyAssignments =
            from f in m.AllFamilies.Values
            select $"this.{f.FamilyVarName} = new Imm{f.FamilyTypeName}<{f.Child.IntfName}>({f.FamilyVarName}.ToArray());";
        var refAssignments =
            from r in m.AllRefs.Values
            select $"this.{r.RefName} = new Imm{r.RefWord}<{r.TargetMatter.IntfName}>({r.RefLowName});";
        var propertyAssignments =
            from p in m.AllProperties.Values
            where !p.ImplementedInMatter
            select $"this.{p.ProName} = {p.ProVarName};";

        var parameters = new List<string>();
        parameters.AddRange(familyParameters);
        parameters.AddRange(refParameters);
        parameters.AddRange(propertyParameters);

        var assignments = new List<string>();
        assignments.AddRange(familyAssignments);
        assignments.AddRange(refAssignments);
        assignments.AddRange(propertyAssignments);

        var realConstructorParameters = m.HasName ? "uint id, uint version, string? name," : "uint id, uint version,";
        var baseConstructorParameters = m.HasName ? "id, version, name" : "id, version";
        var thisFamilies              = m.AllFamilies.Keys.Select(name => "this." + name.Decapitalized).JoinToString();

        cb.Phrase(@"// Constructor \\");
        cb.Phrase("public", m.Imm.ClassName, "(" + realConstructorParameters);

        using (cb.Indenting())
        {
            using (cb.Indenting(5 + m.Imm.ClassName.Length))
            {
                for (var i = 0; i < parameters.Count; i++)
                    cb.Phrase(parameters[i] + (i < parameters.Count - 1 ? "," : ")"));
            }
            cb.Phrase($": base({baseConstructorParameters})");
        }
        using (cb.CurlyBlock(true))
        {
            cb.Text(assignments);
            if (m.IsMedium && m.IsConcrete) cb.Phrase("Families = Imm.ListOf<Family<Matter>>(", thisFamilies, ");");
        }
    }

    private void ProduceImmMatterFamilies(CodeBuilder cb, MetaMatter m)
    {
        if (m.AllFamilies.IsEmpty()) return;
        foreach (var f in m.AllFamilies.Values)
        {
            MetaMatter child      = f.Child;
            string     immTypeStr = $"Imm{f.FamilyTypeName}<{child.IntfName}>";
            string     iTypeStr   = $"{f.FamilyTypeName}<{child.IntfName}>";
            cb.EmptyLine();
            cb.Phrase("// family", child.Names);
            cb.Text($"private readonly {immTypeStr} {f.FamilyVarName};",
                    $"public {iTypeStr} {f.FamilyName} => {f.FamilyVarName};");
        }
    }

    private void ProduceImmMatterReferences(CodeBuilder cb, MetaMatter m)
    {
        cb.EmptyLine();
        cb.Phrase(@"// References \\");

        if (!m.AllRefs.IsEmpty())
        {
            foreach (var r in m.AllRefs.Values)
            {
                var implType = (r.Poly ? "ImmPolyRef" : "ImmMonoRef") + '<' + r.TargetMatter.IntfName + '>';
                var intfType = (r.Poly ? "PolyRef" : "MonoRef") + '<' + r.TargetMatter.IntfName + '>';
                cb.Phrase("public", implType, r.RefName, "{ get; }");
                cb.Phrase(intfType, r.Matter.IntfName + '.' + r.RefName, "=>", r.RefName + ";");
            }
            cb.EmptyLine();
            var refNames = m.AllRefs.Values.JoinToString(r => r.RefName);
            cb.Phrase("public override ImmList<Ref<Matter>> AllRefs => Imm.ListOf<Ref<Matter>>(", refNames, ");");
        }
        else
        {
            cb.Phrase("public override ImmList<Ref<Matter>> AllRefs => AbstractConsts.NoRefs;");
        }
    }

    private void ProduceImmMatterProperties(CodeBuilder cb, MetaMatter m)
    {
        if (m.AllProperties.IsEmpty()) return;
        cb.EmptyLine();
        cb.Text(@"// Properties \\");        
        foreach (var p in m.AllProperties.Values)
        {
            if (p.ImplementedInMatter) continue;
            cb.Phrase("public", p.ProTypeName, p.ProName, "{ get; }");
        }
    }

}