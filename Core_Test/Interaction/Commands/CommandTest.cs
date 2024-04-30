namespace Core.Interaction.Commands;

[TestFixture]
public class CommandTest
{

    [Test]
    public void BasicCommands_Action()
    {
        var bc = new BasicCommand("T1", "TestCommand", null, SetFlag);
        bc.Execute(null);
        Flag.ShouldBeTrue();
    }

    private void SetFlag() => Flag = true;

    private bool Flag = false;


    [Test]
    public void ObjectCommands_Ref_Action()
    {
        var bc = new ObjectCommand<string>("T2", "TestCommand", null, GetThingM, SetThing);
        bc.Refresh();
        bc.Execute(null);
        Thing.ShouldBe("M");
    }

    [Test]
    public void ObjectCommands_Ref_CanExecute()
    {
        var bc = new ObjectCommand<string>("T3", "TestCommand", null, GetThingM, SetThing);
        bc.CanExecute(null).ShouldBeFalse();
        bc.Refresh();
        bc.CanExecute(null).ShouldBeTrue();
    }

    private string GetThingM() => "M";

    private void SetThing(object t) => Thing = t;

    private object? Thing = null;


    [Test]
    public void ObjectCommands_Val_Action()
    {
        var bc = new ObjectCommand<long>("T4", "TestCommand", null, () => 42L, SetLongNumber);
        bc.Refresh();
        bc.Execute(null);
        LongNumber.ShouldBe(42L);
    }

    [Test]
    public void ObjectCommands_Val_CanExecute()
    {
        var bc = new ObjectCommand<long>("T5", "TestCommand", null, () => 42L, SetLongNumber);
        bc.CanExecute(null).ShouldBeFalse();
        bc.Refresh();
        bc.CanExecute(null).ShouldBeTrue();
    }

    private void SetLongNumber(long n) => LongNumber = n;

    private long LongNumber;

}
