using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Model.Essence.Abstracts;
using Util.Fun;

namespace Model.Gen.Generation;

internal class MetaCollector (MetaModel mm)
{

    private readonly Type matterType = typeof(Matter);
    private readonly Type familyType = typeof(Family<Matter>);
    
    internal void CollectMetaData()
    {
        HandleInterface(matterType, false, 0);
        HandleInterface(typeof(NamedMediumMatter), true, 0);
        HandleInterface(typeof(NamedTermMatter), true, 0);
        HandleInterface(typeof(Root), true, 0);
        
        mm.Matters.Sort((m1, m2) => (byte)(m1.SegmKind) - (byte)(m2.SegmKind));
    }

    private void HandleInterface(Type intf, bool isConcrete, byte level)
    {
        Debug.Assert(intf.IsInterface);

        List<Type> baseIntfs;
        HandleBaseInterfaces(intf, level, out baseIntfs);
        
        var m = new MetaMatter(intf, isConcrete, level);
        m.AllBaseIntfs.UnionWith(baseIntfs);
        mm.Add(m);

        if (intf.IsAssignableTo(matterType))
        {
            var familyEntries =
                from child in intf.GetProperties()
                where child.MemberType == MemberTypes.Property 
                   && child.PropertyType.IsAssignableTo(familyType)
                select child;
            foreach (var fe in familyEntries) 
                HandleFamily(m, fe, level);

            var proEntries =
                from p in intf.GetProperties()
                where p.MemberType == MemberTypes.Property
                   && !p.PropertyType.IsAssignableTo(familyType)
                select p;
            foreach (var pe in proEntries) 
                HandleProperty(m, pe);
        }
    }

    private void HandleBaseInterfaces(Type intf, byte level, out List<Type> intfs)
    {
        List<Type> baseInterfaces = (
            from i in intf.GetInterfaces()
            where i.IsAssignableTo(matterType)
            select i
        ).ToList();
        foreach (var i in baseInterfaces)
            if (!mm.Intfs.ContainsKey(i))
                HandleInterface(i, false, level);
        intfs = baseInterfaces;
    }

    private void HandleFamily(MetaMatter parent, PropertyInfo prop, byte parentLevel)
    {
        var propType = prop.PropertyType;
        var propIntf = propType.GenericTypeArguments[0];
        if (!mm.Intfs.ContainsKey(propIntf)) 
            HandleInterface(propIntf, true, parentLevel.Succ());
        var child = mm.Intfs[propIntf];
        Debug.Assert(child is not null);
        var family = new MetaFamily(parent, child, propType, prop.Name);
        parent.AddFamily(family);
    }

    private void HandleProperty(MetaMatter m, PropertyInfo prop)
    {
        var proName = prop.Name;
        var proType = prop.PropertyType;
        var p       = new MetaProperty(m, proName, proType);
        m.OwnProperties.Add(p);
    }
}