using System;
using System.Linq;
using Model.Abstracts;
using Shouldly;
using Util.Collections;
using Util.Extensions;

namespace Model;

[TestFixture]
public class ModelMetaBriefTest
{
    private readonly Type MatterType = typeof(Matter);

    [Test]
    public void ConcreteModelMatters_Brief()
    {
        foreach (var mt in ModelMetaBrief.ConcreteModelMatters)
        {
            mt.IsInterface.ShouldBeTrue();
            mt.IsAssignableTo(MatterType).ShouldBeTrue();
            mt.IsIn(ModelMetaBrief.AllModelMatters).ShouldBeTrue();
        }
    }

    [Test]
    public void AllModelMatters_OnlyMatters()
    {
        foreach (var mt in ModelMetaBrief.AllModelMatters)
        {
            mt.IsAssignableTo(MatterType).ShouldBeTrue();
        }
    }

    [Test]
    public void AllModelMatters_TheFirstIsMatter()
    {
        ModelMetaBrief.AllModelMatters.First().ShouldBeSameAs(MatterType);
    }

    [Test]
    public void AllModelMatters_TopologicallySorted()
    {
        ImmDict<Type, int> indices =
            ModelMetaBrief.AllModelMatters.Index().ToImmDict(keySelector: t => t.Item, valueSelector: t => t.Index);
        foreach (var mt in ModelMetaBrief.AllModelMatters)
        {
            if (mt == MatterType) continue;
            int index = indices[mt];
            index.ShouldBePositive();
            foreach (var baseIntf in mt.GetInterfaces().Where(i => i.IsAssignableTo(MatterType)))
            {
                if (baseIntf == MatterType) continue;
                int baseIndex = indices[baseIntf];
                baseIndex.ShouldBePositive(customMessage: $"The interface {baseIntf.Name} should be in the list.");
                baseIndex.ShouldBeLessThan(index, customMessage: $"The interface {baseIntf.Name} should not be mentioned before {mt.Name}.");
            }
        }
    }

}
