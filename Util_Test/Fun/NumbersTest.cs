using System;

namespace Util.Fun;


[TestFixture]
public class NumbersTest
{

    [Test]
    public static void Byte_PredAndSucc() => Verify
    (
        () => _9_.Pred().ShouldBe(_8_),
        () => _1_.Pred().ShouldBe(_0_),

        () => _3_.Succ().ShouldBe(_4_),
        () => _254_.Succ().ShouldBe(_255_)
    );

    [Test]
    public static void Byte_Pred_Overflow()
    {
        Should.Throw<OverflowException>
        (
            () => _0_.Pred()
        );
    }

    [Test]
    public static void Byte_Succ_Overflow()
    {
        Should.Throw<OverflowException>
        (
            () => _255_.Succ()
        );
    }


}
