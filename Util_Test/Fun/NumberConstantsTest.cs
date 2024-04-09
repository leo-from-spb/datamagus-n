namespace Util.Test.Fun;

using static Common.Fun.NumberConstants;

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

            () => _0_.ShouldBe(default(byte))
        );
    }


}
