using NUnit.Framework.Interfaces;
using Testing.Appliance.Assertions;
using Util.Extensions;

using static Gena.CSharp.CsVisibility;

namespace Gena.CSharp;

[TestFixture]
public class CsProducerTest
{
    /// <summary>
    /// Shared construction,
    /// that is cleared after every test.
    /// </summary>
    private readonly CsConstruction Construction = new();

    private CsFile File = null!;

    private string ProducedText  = "";
    private int    ProducedWidth = 0;


    [SetUp]
    public void Setup()
    {
        File = Construction.NewFile("TestNamespace", "TestClass");
        ProducedText = "";
    }

    [TearDown]
    public void FinalizeAndClear()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            if (ProducedText.Some)
            {
                int w = ProducedWidth;
                TestContext.Out.WriteLine("─── Produced text ───".PadRight(w, '─'));
                TestContext.Out.WriteLine(ProducedText);
                TestContext.Out.WriteLine("".PadRight(w, '─'));
            }
            else
            {
                TestContext.Out.WriteLine("No produced text!");
            }
        }
        Construction.ClearContent();
        File = null!;
        ProducedText = "";
    }

    [Test]
    public void HelloWorld()
    {
        var clazz  = File.NewClass("HelloWorld");
        var method = clazz.NewMethod("Main", isStatic: true);
        method.NewArgument("args", "string[]");
        method.ContentBuilder.Phrase("""System.Console.WriteLine("Hello World!");""");

        ProduceFileText();
        var expected = """
                       public class HelloWorld
                       {
                       	   public static void Main (string[] args)
                       	   {
                               System.Console.WriteLine("Hello World!");
                       	   }
                       }
                       """;
        ProducedText.ShouldContainWithoutWhitespace(expected);
    }

    [Test]
    public void HelloWorld_Documentation()
    {
        var clazz  = File.NewClass("HelloWorld");
        clazz.Documentation = """
                              This is a test class.
                              This class is for testing.
                              """;
        var method = clazz.NewMethod("Main", isStatic: true);
        method.Documentation = """
                               This is a test method.
                               This method is for testing.
                               """;
        method.NewArgument("args", "string[]");

        ProduceFileText();
        var expected = """
                       /// <summary>
                       /// This is a test class.
                       /// This class is for testing.
                       /// </summary>
                       public class HelloWorld
                       {
                           /// <summary>
                           /// This is a test method.
                           /// This method is for testing.
                           /// </summary>
                       	   public static void Main (string[] args)
                       	   {
                       """;
        ProducedText.ShouldContainWithoutWhitespace(expected);
    }

    [Test]
    public void Field_Basic()
    {
        var clazz = File.NewClass("MyClass");
        var f1    = clazz.NewField("X", "long", defaultValue: "0L");
        var f2    = clazz.NewField("Y", "float", defaultValue: "0.0");
        f1.Visibility = visPublic;
        f2.Visibility = visPrivate;

        f1.IsPlainField.ShouldBeTrue();
        f2.IsPlainField.ShouldBeTrue();

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public long X = 0L;
                                                    private float Y = 0.0;
                                                    """);
    }

    [Test]
    public void Field_PureExpression()
    {
        var clazz = File.NewClass("MyClass");
        var f = clazz.NewField("Count", "int", expression: "42 + 26", visibility: visPublic);

        f.IsPureExpression.ShouldBeTrue();
        f.IsPureProperty.ShouldBeFalse();

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public int Count => 42 + 26;
                                                    """);
    }

    [Test]
    public void Field_PureProperty_1()
    {
        var clazz = File.NewClass("MyClass");
        var f = clazz.NewField("Count", "int", expression: "", visibility: visPublic);
        f.IsOverride = true;

        f.IsPureProperty.ShouldBeTrue();
        f.IsPureExpression.ShouldBeFalse();

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public override int Count { get; }
                                                    """);
    }

    [Test]
    public void Field_PureProperty_2()
    {
        var clazz = File.NewClass("MyClass");
        var f = clazz.NewField("Count", "int", expression: "", visibility: visPublic);
        f.TheSetExpression = "";

        f.IsPureProperty.ShouldBeTrue();
        f.IsPureExpression.ShouldBeFalse();

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public int Count { get; set; }
                                                    """);
    }

    [Test]
    public void Field_Complex()
    {
        var clazz = File.NewClass("MyClass");
        var f = clazz.NewField("MyVar", "int", visibility: visPublic);
        f.TheGetExpression = "field - 1";
        f.TheSetExpression = "value + 1";

        f.IsPlainField.ShouldBeFalse();
        f.IsPureExpression.ShouldBeFalse();
        f.IsPureProperty.ShouldBeFalse();

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public int MyVar
                                                    {
                                                        get { return field - 1; }
                                                        set { field = value + 1; }
                                                    }
                                                    """);
        ProducedText.ShouldNotContain("};"); // on the last line
    }

    [Test]
    public void Field_ComplexWithDefault()
    {
        var clazz = File.NewClass("MyClass");
        var f = clazz.NewField("MyVar", "int", "100", visibility: visPublic);
        f.TheGetExpression = "field - 1";
        f.TheSetExpression = "value + 1";

        f.IsPlainField.ShouldBeFalse();
        f.IsPureExpression.ShouldBeFalse();

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public int MyVar
                                                    {
                                                        get { return field - 1; }
                                                        set { field = value + 1; }
                                                    } = 100;
                                                    """);
    }

    [Test]
    public void ClassConstructor()
    {
        var clazz = File.NewClass("MyClass");
        var ctr   = clazz.NewConstructor();
        ctr.NewArgument("x", "int");
        ctr.NewArgument("y", "float");

        ProduceFileText();
        ProducedText.ShouldContainInOrder("class", "{",
                                          "public MyClass", "(int x, float y)",
                                          "{", "}",
                                          "}");
    }

    [Test]
    public void ClassWithTypeParams()
    {
        var clazz = File.NewClass("MyGenericClass");
        clazz.TypoParams.Add("X");
        clazz.TypoParams.Add("Y");

        ProduceFileText();
        ProducedText.ShouldContain("MyGenericClass<X,Y>");
    }


    [Test]
    public void ClassWrappedWithRegion()
    {
        var clazz = File.NewClass("MyClassInRegion");
        clazz.WrappingRegion = "MyRegion";

        ProduceFileText();
        ProducedText.ShouldContainInOrder("#region MyRegion", "class MyClass", "{", "}", "#endregion");
    }


    [Test]
    public void Method_Basic()
    {
        var clazz = File.NewClass("MyClass");
        var m     = clazz.NewMethod("Tan", "float");

        m.IsExpression = false;
        m.ContentBuilder.Phrase("return Sin / Cos;");

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public float Tan()
                                                    {
                                                        return Sin / Cos;
                                                    }
                                                    """);
    }


    [Test]
    public void Method_Expression()
    {
        var clazz = File.NewClass("MyClass");
        var m     = clazz.NewMethod("Tan", "float");

        m.IsExpression = true;
        m.ContentBuilder.Phrase("Sin / Cos;");

        ProduceFileText();
        ProducedText.ShouldContainWithoutWhitespace("""
                                                    public float Tan() =>
                                                        Sin / Cos;
                                                    """);
    }


    private void ProduceFileText()
    {
        var producer = new CsProducer();
        producer.ProduceFile(File);
        ProducedText = producer.ResultText;
        ProducedWidth = producer.ContentBuilder.TextWidth;
    }
}
