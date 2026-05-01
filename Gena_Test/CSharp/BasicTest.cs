namespace Gena.CSharp;


[TestFixture]
public class BasicTest
{
    [Test]
    public void HelloWorld()
    {
        var con = new CsConstruction();
        var file = con.NewFile("HelloNamespace", "HelloWorld");
        var clazz = file.NewClass("HelloWorld");
        var method = clazz.NewMethod("Main", isStatic: true);
        method.NewArgument("args", "string[]");
        method.ContentBuilder.Phrase("""System.Console.WriteLine("Hello World!");""");

        var producer = new CsProducer();
        producer.ProduceFile(file);
        var text = producer.ResultText;

        //System.Console.WriteLine(text);
    }
}
