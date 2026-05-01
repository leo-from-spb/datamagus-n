using System.Linq;
using Gena.Code;
using Gena.Fun;
using Util.Extensions;
using static Gena.CSharp.CsVisibility;

namespace Gena.CSharp;

/// <summary>
/// Produces C# code by the given model.
/// </summary>
public class CsProducer
{

    private readonly CodeBuilder B;


    public CsProducer()
    {
        B = new CodeBuilder();

        B.Indentation = "    ";
    }


    public void ProduceFile(CsFile file)
    {
        B.Text(file.Header);
        B.EmptyLine();

        foreach (string imp in file.Imports)
            B.Phrase("using", imp, ";");
        B.EmptyLine();

        B.Phrase("namespace", file.Namespace, ";");
        B.EmptyLine();

        foreach (CsClass clazz in file.Classes)
        {
            ProduceClass(clazz);
        }
    }


    private void ProduceClass(CsClass clazz)
    {
        B.EmptyLine();
        WriteRegionBegin(clazz.WrappingRegion);

        ProduceDocumentation(clazz);
        string typeParams = clazz.TypoParams.JoinToString(prefix: "<", suffix: ">", separator: ",");
        B.Phrase(clazz.Visibility.Word,
                 "static".TakeIf(clazz.IsStatic),
                 "sealed".TakeIf(clazz.IsSealed),
                 "abstract".TakeIf(clazz.IsAbstract),
                 "class", clazz.Name + typeParams,
                 ":".TakeIf(clazz.BaseNames is not null),
                 clazz.BaseNames);

        using (B.CurlyBlock(thenSkipLine: true))
        {
            ProduceClassContent(clazz);
        }

        WriteRegionEnd(clazz.WrappingRegion);
    }

    private void ProduceClassContent(CsClass clazz)
    {
        // Fields
        foreach (var f in clazz.Fields)
        {
            string? getter = f.TheGetExpression;
            string? setter = f.TheSetExpression;
            bool isSingleLine = f.IsPlainField ||
                                f.IsPureProperty ||
                                f.IsPureExpression && getter!.Length + B.CurPos < 60;
            string? purePropertyBody = f.IsPureProperty
                ? "{ get;" + (setter == "" ? " set;" : "") + " }"
                : null;

            ProduceDocumentation(f);
            B.Phrase(f.Visibility.Word,
                     f.IsStatic ? "static" : null,
                     f.IsOverride ? "override" : null,
                     f.IsReadOnly ? "readonly" : null,
                     f.Type,
                     f.Name,
                     f.DefaultValue.Wrap(prefix: "= ").TakeIf(isSingleLine),
                     purePropertyBody,
                     "=>".TakeIf(f.IsPureExpression),
                     getter.TakeIf(f.IsPureExpression && isSingleLine),
                     ";".TakeIf(isSingleLine));
            if (isSingleLine) continue;

            if (f.IsPureExpression)
            {
                using (B.Indenting())
                {
                    B.Text(f.TheGetExpression);
                }
            }
            else
            {
                if (getter is not null || setter is not null)
                {
                    B.Phrase("{");
                    using (B.Indenting())
                    {
                        if (getter is not null)
                            B.Phrase("get { return", getter, "; }");
                        if (setter is not null)
                            B.Phrase("set { field =", setter, "; }");
                    }

                    B.Phrase("}", f.DefaultValue.Wrap("= ", ";"));
                }
            }
        }

        if (clazz.Fields.Some) B.EmptyLine();

        // Constructors
        foreach (var ctr in clazz.Constructors)
        {
            var arguments = ctr.Arguments
                               .Select(a => a.Spec)
                               .JoinToString();
            var visibility = ctr.Visibility != visAuto
                ? ctr.Visibility
                : clazz.IsAbstract ? visProtected : visPublic;

            ProduceDocumentation(ctr);
            B.Phrase(visibility.Word,
                     clazz.Name,
                     "(", arguments, ")" );
            using (B.CurlyBlock())
            {
                B.Text(ctr.Content);
            }

        }

        // Methods
        foreach (var m in clazz.Methods)
        {
            var arguments = m.Arguments
                             .Select(a => a.Spec)
                             .JoinToString();
            ProduceDocumentation(m);
            B.Phrase(m.Visibility.Word,
                     "static".TakeIf(m.IsStatic),
                     "sealed".TakeIf(m.IsSealed),
                     "abstract".TakeIf(m.IsAbstract),
                     m.ReturnType ?? "void",
                     m.Name,
                     "(", arguments, ")",
                     "=>".TakeIf(m.IsExpression) );
            if (m.IsExpression)
            {
                using (B.Indenting())
                {
                    B.Text(m.Content);
                }
            }
            else
            {
                using (B.CurlyBlock())
                {
                    B.Text(m.Content);
                }
            }
        }
    }

    private void WriteRegionBegin(string? region)
    {
        if (region != null)
        {
            B.Phrase("#region", region);
            B.EmptyLine();
        }
    }

    private void WriteRegionEnd(string? region)
    {
        if (region != null)
        {
            B.EmptyLine();
            B.Phrase("#endregion", "//", region);
        }
    }

    private void ProduceDocumentation(CsEntity entity)
    {
        string? doc = entity.Documentation;
        if (doc.SomeNotBlank)
        {
            using (B.Indenting("/// "))
            {
                B.Phrase("<summary>");
                B.Text(doc);
                B.Phrase("</summary>");
            }
        }
    }

    /// <summary>
    /// Generated text.
    /// </summary>
    public string ResultText => B.Result;

    /// <summary>
    /// Reference to the content builder.
    /// </summary>
    public CodeBuilder ContentBuilder => B;


}
