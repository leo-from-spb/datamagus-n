namespace Core.Stationery;

public class DataMagusInfoTest
{
    [Test]
    public void Test1() => Verify
    (
        () => DataMagusInfo.ProductVersion.Minor.ShouldBePositive()
    );
}
