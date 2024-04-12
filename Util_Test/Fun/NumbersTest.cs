using System;

namespace Util.Fun;

using static NumberConstants;


[TestFixture]
public class NumbersTest
{

    [Test]
    public static void Byte_PredAndSucc() => Verify
    (
        () => _9_.pred().ShouldBe(_8_),
        () => _1_.pred().ShouldBe(_0_),

        () => _3_.succ().ShouldBe(_4_),
        () => _254_.succ().ShouldBe(_255_)
    );

    [Test]
    public static void Byte_Pred_Overflow()
    {
        Should.Throw<OverflowException>
        (
            () => _0_.pred()
        );
    }

    [Test]
    public static void Byte_Succ_Overflow()
    {
        Should.Throw<OverflowException>
        (
            () => _255_.succ()
        );
    }


}
