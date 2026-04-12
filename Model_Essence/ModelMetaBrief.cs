using System;
using Model.Abstracts;
using Model.Concept;
using Model.Visuality;
using Util.Collections;

namespace Model;

/// <summary>
/// Model base meta info.
/// </summary>
public class ModelMetaBrief
{
    /// <summary>
    /// List of all model instantiable entities.
    /// </summary>
    public static readonly ImmListSet<Type> ActualModelMatters =
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

    //public static readonly ImmListSet<Type> AllMatters;
    //
    //
    //static ModelMetaBrief()
    //{
    //    var allMatterList = new List<Type>();
    //    var allMatterSet = new HashSet<Type>();
    //    foreach (var mt in ActualModelMatters)
    //    {
    //        var inheritedMatters = Trees.TraversDepthFirst(mt, x => x.GetInterfaces());
    //    }
    //}

}
