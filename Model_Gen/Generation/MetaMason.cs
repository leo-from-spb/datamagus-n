using System.Linq;
using Gena.CSharp;
using Util.Extensions;

namespace Model.Generation;

/// <summary>
/// Builds C# model files and classes by MetaModel.
/// </summary>
/// <param name="mm"></param>
internal class MetaMason(MetaModel mm)
{
    internal readonly CsConstruction Construction = new();

    internal void ConstructCsModel()
    {
        BuildImmutableClasses(SegmentKind.soCommon, MetaConsts.ImmCommonFilePath);
        BuildImmutableClasses(SegmentKind.soConcept, MetaConsts.ImmConceptFilePath);
        BuildImmutableClasses(SegmentKind.soVisuality, MetaConsts.ImmVisualityFilePath);
    }


    private void BuildImmutableClasses(SegmentKind segmentKind, string filePath)
    {
        CsFile file = Construction.NewFile("Model.Imm2", filePath);

        file.Imports.Add("System.Collections.Generic");
        file.Imports.Add("System.Linq");
        file.Imports.Add("Model.Abstracts");
        file.Imports.Add("Model.Concept");
        file.Imports.Add("Model.Visuality");

        var segmentMatters = from m in mm.Matters
                             where m.SegmKind == segmentKind
                                && m.Imm.ToImplement
                             select m;

        foreach (var matter in segmentMatters)
        {
            BuildImmMatter(file, matter);
        }
    }


    private void BuildImmMatter(CsFile file, MetaMatter m)
    {
        var mClass = file.NewClass(m.Imm.ClassName,
                                   m.Imm.BaseClassName,
                                   isSealed: m.IsConcrete,
                                   isAbstract: !m.IsConcrete);
        mClass.WrappingRegion = "Matter Class " + m.Imm.ClassName;
        mClass.ImplementInterfaces.Add(m.IntfName);

        var ctr = mClass.NewConstructor(null);
        ctr.NewArgument("id", "uint");
        ctr.NewArgument("version", "uint");
        ctr.PassArguments.Add("id");
        ctr.PassArguments.Add("version");

        if (m.HasName)
        {
            ctr.NewArgument("name", "string?");
            ctr.PassArguments.Add("name");
        }

        // Families
        if (m.AllFamilies.Some)
        {
            mClass.NewField(null, "families", "Family<Matter>[]", isReadOnly: true);
            mClass.NewField(null, "Families", "IReadOnlyList<Family<Matter>>", isOverride: true, expression: "families");

            var gFam1 = mClass.NewGroup(null, "Families (fields)");
            var gFam2 = mClass.NewGroup(null, "Families (properties)");

            ctr.ContentBuilder.Text("// Families");

            foreach (var f in m.AllFamilies.Values)
            {
                // make the backfield and the property
                var childType = f.Child.IntfName;
                var type2     = f.FamilyTypeName + '<' + childType + '>';
                var type1     = "Imm" + type2;
                var varName   = f.FamilyVarName;
                mClass.NewField(gFam1, varName, type1, isReadOnly: true);
                mClass.NewField(gFam2, f.FamilyName, type2, expression: varName);

                // make constructor argument and assignment
                ctr.NewArgument(varName, $"IEnumerable<{childType}>");
                ctr.ContentBuilder.Text($"this.{varName} = new {type1}({varName}.ToArray());");
            }

            string allBackFields = m.AllFamilies.Values.JoinToString(f => $"this.{f.FamilyVarName}");
            ctr.ContentBuilder.Text($$"""families = new Family<Matter>[] { {{allBackFields}} };""");
        }

        // References
        var gRef1 = mClass.NewGroup(null, "References (properties)");
        var gRef2 = mClass.NewGroup(null, "References (interface)");
        if (m.AllRefs.Some)
        {
            ctr.ContentBuilder.Text("// References");

            foreach (var r in m.AllRefs.Values)
            {
                var implType = (r.Poly ? "ImmPolyRef" : "ImmMonoRef") + '<' + r.TargetMatter.IntfName + '>';
                var intfType = (r.Poly ? "PolyRef" : "MonoRef") + '<' + r.TargetMatter.IntfName + '>';

                // make the backfield and the property
                mClass.NewField(gRef1, r.RefName, implType, expression: "");
                mClass.NewField(gRef2, r.Matter.IntfName+'.'+r.RefName, intfType, expression: r.RefName);

                // cmake constructor argument and assignment
                ctr.NewArgument(r.RefLowName, "uint");
                ctr.ContentBuilder.Text($"this.{r.RefName} = new {implType}({r.RefLowName});");
            }

            var allRefNames = m.AllRefs.Values.JoinToString(r => r.RefName);
            var allRefExpr  = $$"""new Ref<Matter>[] { {{allRefNames}} }""";
            mClass.NewField(gRef2, "AllRefs", "IReadOnlyList<Ref<Matter>>", isOverride: true, expression: allRefExpr);
        }
        else
        {
            mClass.NewField(gRef2, "AllRefs", "IReadOnlyList<Ref<Matter>>", isOverride: true, expression: "AbstractConsts.NoRefs");
        }

        // Properties
        var gProp = mClass.NewGroup(null, "Properties");
        if (m.AllProperties.Some)
        {
            ctr.ContentBuilder.Text("// References");

            foreach (var p in m.AllProperties.Values)
            {
                // the property
                mClass.NewField(gProp, p.ProName, p.ProTypeName, expression: "");

                // cmake constructor argument and assignment
                ctr.NewArgument(p.ProVarName, p.ProTypeName);
                ctr.ContentBuilder.Text($"this.{p.ProName} = {p.ProVarName};");
            }
        }
    }

}
