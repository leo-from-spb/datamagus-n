namespace Util.Fun;

using static NumberConstants;

[TestFixture]
public class NumberConstantsTest
{
    [Test]
    public void ByteConstants_Type()
    {
        Verify
        (
            () => _0_.GetType().ShouldBe(typeof(byte)),
            () => _1_.GetType().ShouldBe(typeof(byte)),
            () => _2_.GetType().ShouldBe(typeof(byte)),
            () => _3_.GetType().ShouldBe(typeof(byte)),
            () => _4_.GetType().ShouldBe(typeof(byte)),
            () => _5_.GetType().ShouldBe(typeof(byte)),
            () => _6_.GetType().ShouldBe(typeof(byte)),
            () => _7_.GetType().ShouldBe(typeof(byte)),
            () => _8_.GetType().ShouldBe(typeof(byte)),
            () => _9_.GetType().ShouldBe(typeof(byte)),
            () => _10_.GetType().ShouldBe(typeof(byte)),
            () => _11_.GetType().ShouldBe(typeof(byte)),
            () => _12_.GetType().ShouldBe(typeof(byte)),
            () => _13_.GetType().ShouldBe(typeof(byte)),
            () => _14_.GetType().ShouldBe(typeof(byte)),
            () => _15_.GetType().ShouldBe(typeof(byte)),
            () => _16_.GetType().ShouldBe(typeof(byte)),
            () => _17_.GetType().ShouldBe(typeof(byte)),
            () => _18_.GetType().ShouldBe(typeof(byte)),
            () => _19_.GetType().ShouldBe(typeof(byte)),
            () => _20_.GetType().ShouldBe(typeof(byte)),
            () => _21_.GetType().ShouldBe(typeof(byte)),
            () => _22_.GetType().ShouldBe(typeof(byte)),
            () => _23_.GetType().ShouldBe(typeof(byte)),
            () => _24_.GetType().ShouldBe(typeof(byte)),
            () => _25_.GetType().ShouldBe(typeof(byte)),
            () => _26_.GetType().ShouldBe(typeof(byte)),
            () => _27_.GetType().ShouldBe(typeof(byte)),
            () => _28_.GetType().ShouldBe(typeof(byte)),
            () => _29_.GetType().ShouldBe(typeof(byte)),

            () => _0_.ShouldBe(default(byte))
        );
    }


}
