using System.Linq;
using Testing.Appliance.Assertions;

namespace Core.Interaction.Commands;


[TestFixture]
public class RealCommandRegistryTest
{

    [Test]
    public void Basic()
    {
        RealCommandRegistry registry = new RealCommandRegistry();
        registry.Sunrise();

        var cmdA = registry.NewBasicCommand("A", "Cmd A", null, DoNothing);
        var cmdB = registry.NewObjectCommand<long>("B", "Cmd B", null, () => 42L, DoNothing<long>);

        registry.ListAllCommands().ShouldContainAll(cmdA, cmdB);

        var registeredCommandNames = registry.ListAllCommands().Select(cmd => cmd.Name).ToArray();
        registeredCommandNames.ShouldContainAll("Cmd A", "Cmd B");

        registry["A"].ShouldBeSameAs(cmdA);
        registry["B"].ShouldBeSameAs(cmdB);
    }


    private static void DoNothing() { }
    private static void DoNothing<O>(O something) { }
}
