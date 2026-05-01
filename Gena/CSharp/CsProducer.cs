using System.Linq;
using Gena.Code;
using Util.Extensions;

namespace Gena.CSharp;

/// <summary>
/// Produces C# code by the given model.
/// </summary>
public class CsProducer
{

    private readonly CodeBuilder B =  new CodeBuilder();


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
        B.Phrase("public",
                 "static".TakeIf(clazz.IsStatic),
                 "sealed".TakeIf(clazz.IsSealed),
                 "abstract".TakeIf(clazz.IsAbstract),
                 "class", clazz.Name,
                 ":".TakeIf(clazz.BaseNames is not null),
                 clazz.BaseNames);

        using (B.CurlyBlock(thenSkipLine: true))
        {
            ProduceClassContent(clazz);
        }
    }


    private void ProduceClassContent(CsClass clazz)
    {
        // Methods
        foreach (var m in clazz.Methods)
        {
            var arguments = m.Arguments
                             .Select(a => a.Spec)
                             .JoinToString();

            B.Phrase("public",
                     m.ReturnType ?? "void",
                     m.Name,
                     "(", arguments, ")" );
            using (B.CurlyBlock())
            {
                B.Text(m.Content);
            }
        }
    }


    /// <summary>
    /// Generated text.
    /// </summary>
    public string ResultText => B.Result;

}
