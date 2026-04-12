using System;
using Model.Abstracts;
using Shouldly;

namespace Model;

[TestFixture]
public class ModelMetaBriefTest
{
    private readonly Type MatterType = typeof(Matter);

    [Test]
    public void MatterList_OnlyModelMatters()
    {
        foreach (var mt in ModelMetaBrief.ActualModelMatters)
        {
            mt.IsInterface.ShouldBeTrue();
            mt.IsAssignableTo(MatterType);
        }
    }

}
