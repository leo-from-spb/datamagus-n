namespace Util.Collections;

[TestFixture]
public class ImmDictBuilderTest
{
    [Test]
    public void N4_Basic()
    {
        var builder = new ImmDictBuilder<ulong,string>();
        builder.Add(1uL, "One")
               .Add(2uL, "Two")
               .Add(3uL, "Three")
               .Add(4uL, "Four");
        ImmListDict<ulong,string> dict = builder.Build();
        dict.Verify(
            d => d[1ul].ShouldBe("One"),
            d => d[2ul].ShouldBe("Two"),
            d => d[3ul].ShouldBe("Three"),
            d => d[4ul].ShouldBe("Four"),
            d => d.Count.ShouldBe(4)
        );
    }

    [Test]
    public void N4_Duplicates()
    {
        var builder = new ImmDictBuilder<ulong,string>();
        builder.Add(0uL, "Zero")
               .Add(1uL, "One")
               .Add(2uL, "Two")
               .Add(2uL, "AnotherTwo");
        KeyDuplicationException exception = Should.Throw<KeyDuplicationException>(() =>
                                                                                      builder.Build()
        );
        exception.Verify(
            e => e.Index1.ShouldBe(2),
            e => e.Index2.ShouldBe(3)
        );
    }

    [Test]
    public void N7_Basic()
    {
        var builder = new ImmDictBuilder<ulong,string>();
        builder.Add(1uL, "One")
               .Add(2uL, "Two")
               .Add(3uL, "Three")
               .Add(4uL, "Four")
               .Add(5uL, "Five")
               .Add(6uL, "Six")
               .Add(7uL, "Seven");
        ImmListDict<ulong,string> dict = builder.Build();
        dict.Verify(
            d => d[1ul].ShouldBe("One"),
            d => d[2ul].ShouldBe("Two"),
            d => d[3ul].ShouldBe("Three"),
            d => d[4ul].ShouldBe("Four"),
            d => d[5ul].ShouldBe("Five"),
            d => d[6ul].ShouldBe("Six"),
            d => d[7ul].ShouldBe("Seven"),
            d => d.Count.ShouldBe(7)
        );
    }

    [Test]
    public void N7_Duplicates()
    {
        var builder = new ImmDictBuilder<ulong,string>();
        builder.Add(0uL, "Zero")
               .Add(1uL, "One")
               .Add(2uL, "Two")
               .Add(3uL, "Three")
               .Add(4uL, "Four")
               .Add(5uL, "Five")
               .Add(6uL, "Six")
               .Add(6uL, "AnotherSix");
        KeyDuplicationException exception = Should.Throw<KeyDuplicationException>(() =>
                                                                                      builder.Build()
        );
        exception.Verify(
            e => e.Index1.ShouldBe(6),
            e => e.Index2.ShouldBe(7)
        );
    }



}
