using System;

namespace Util.Collections.Implementation;


[TestFixture]
public class ImmutableHashSetTest
{
    /// <summary>
    /// A structure with a BAD hash code
    /// </summary>
    private readonly struct SmallStruct : IEquatable<SmallStruct>
    {
        public readonly int A;
        public readonly int B;

        public SmallStruct(int a, int b)
        {
            A = a;
            B = b;
        }

        public override int GetHashCode() => A + B;

        public bool Equals(SmallStruct other) => this.A == other.A && this.B == other.B;

        public override bool Equals(object? obj) => obj is SmallStruct other && this.Equals(other);
    }


    [Test]
    public void HashCollision1()
    {
        SmallStruct str1 = new(10, 90),
                    str2 = new(20, 80),
                    str3 = new(30, 70);
        Verify(
            () => str1.GetHashCode().ShouldBe(100),
            () => str2.GetHashCode().ShouldBe(100),
            () => str3.GetHashCode().ShouldBe(100)
        );

        var set = new ImmutableHashSet<SmallStruct>([str1, str2, str3]);

        set.Verify(
            s => s.IndexOf(new SmallStruct(10,90)).ShouldBe(0),
            s => s.IndexOf(new SmallStruct(20,80)).ShouldBe(1),
            s => s.IndexOf(new SmallStruct(30,70)).ShouldBe(2),
            s => s.IndexOf(new SmallStruct(40,60)).ShouldBeNegative()
        );
    }


}
