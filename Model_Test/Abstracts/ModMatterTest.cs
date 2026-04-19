using System.Linq;
using Shouldly;
using Testing.Appliance.Assertions;

namespace Model.Abstracts;


[TestFixture]
public class ModMatterTest
{

    [Test]
    public void BrandNew()
    {
        ModelMaster master = new ModelMaster(1);

        ModTestYard yard = new ModTestYard(master);

        ModTestRabbit rabbit1 = yard.Rabbits.New("Rabbit 1");
        ModTestRabbit rabbit2 = yard.Rabbits.New("Rabbit 2");

        ModTestGuineaPig pig1 = yard.GuineaPigs.New("Pig 1");
        ModTestGuineaPig pig2 = yard.GuineaPigs.New("Pig 2");

        yard.Rabbits.GetAllNames().ShouldBe(["Rabbit 1", "Rabbit 2"]);
        yard.GuineaPigs.GetAllNames().ShouldBe(["Pig 1", "Pig 2"]);

        Matter[] actualInnerMatters = yard.AllInnerMatters.ToArray();
        Matter[] expectedInnerMatters = [rabbit1, rabbit2, pig1, pig2];
        actualInnerMatters.ShouldBe(expectedInnerMatters);
    }

}
