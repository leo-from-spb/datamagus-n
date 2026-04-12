using System;
using System.Collections.Generic;
using Model.Abstracts;
using Model.Concept;
using Model.Visuality;
using Util.Collections;
using Util.Extensions;
using Util.Fun;

namespace Model;

/// <summary>
/// Model base meta info.
/// </summary>
public static class ModelMetaBrief
{
    /// <summary>
    /// List of model concrete (instantiable) entities,
    /// in the proper order.
    /// </summary>
    public static readonly ImmListSet<Type> ConcreteModelMatters =
        Imm.SetOf
        (
            typeof(Root),
            typeof(ConModel),
            typeof(ConSubjArea),
            typeof(ConDomain),
            typeof(ConEntity),
            typeof(ConAttribute),
            typeof(DiaAlbum),
            typeof(DiaTemplate),
            typeof(DiaPage),
            typeof(DiaShape)
        );

    /// <summary>
    /// List of all model classes, both instantiatable and medium,
    /// topologically sorted.
    /// </summary>
    public static readonly ImmListSet<Type> AllModelMatters;


    private static readonly Type MatterType = typeof(Matter);


    static ModelMetaBrief()
    {
        var allMatterList = new List<Type>();
        var allMatterSet  = new HashSet<Type>();

        // first — add very common abstract interfaces
        allMatterList.Add(MatterType);
        allMatterList.Add(typeof(MediumMatter));
        allMatterList.Add(typeof(TermMatter));
        allMatterList.Add(typeof(NamedMatter));
        allMatterList.Add(typeof(NamedMediumMatter));
        allMatterList.Add(typeof(NamedTermMatter));
        allMatterList.Add(typeof(AbSegment));
        allMatterList.Add(typeof(AbSection));

        // then — add all other interfaces
        foreach (var mt in ConcreteModelMatters)
        {
            var inheritedMatters = Trees.TraversDepthChildrenFirst(mt, x => x.GetInterfaces());
            foreach (var im in inheritedMatters)
            {
                if (im.IsAssignableTo(MatterType) && im.IsNotIn(allMatterSet))
                {
                    allMatterList.Add(im);
                    allMatterSet.Add(im);
                }
            }
        }

        AllModelMatters = allMatterList.ToImmSet();
    }

}
