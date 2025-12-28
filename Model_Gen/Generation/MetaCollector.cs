using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Model.Abstracts;
using Util.Collections;
using Util.Extensions;
using Util.Fun;

namespace Model.Generation;

internal class MetaCollector (MetaModel mm)
{

    private readonly Type matterType = typeof(Matter);
    private readonly Type familyType = typeof(Family<Matter>);
    private readonly Type refType = typeof(Ref<Matter>);
    private readonly Type polyRefType = typeof(PolyRef<Matter>);

    internal void CollectMetaData()
    {
        HandleInterface(matterType, false, 0);
        HandleInterface(typeof(NamedMediumMatter), false, 0);
        HandleInterface(typeof(NamedTermMatter), false, 0);
        HandleInterface(typeof(Root), true, 0);
        
        mm.Matters.Sort((m1, m2) => (byte)(m1.SegmKind) - (byte)(m2.SegmKind));
    }

    private void HandleInterface(Type intf, bool isConcrete, byte level)
    {
        Debug.Assert(intf.IsInterface);

        var m = new MetaMatter(intf, isConcrete, level);
        HandleBaseInterfaces(intf,
                             level,
                             out m.DeclaredBaseIntfs,
                             out m.IndirectBaseIntfs,
                             out m.AllBaseIntfs);
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

            var refEntries =
                from r in intf.GetProperties()
                where r.MemberType == MemberTypes.Property
                   && r.PropertyType.IsAssignableTo(refType)
                select r;
            foreach (var pe in refEntries)
                HandleRef(m, pe);

            var proEntries =
                from p in intf.GetProperties()
                where p.MemberType == MemberTypes.Property
                   && p.GetCustomAttribute<MatterPropertyAttribute>() != null
                   && !p.PropertyType.IsAssignableTo(familyType)
                   && !p.PropertyType.IsAssignableTo(refType)
                select p;
            foreach (var pe in proEntries) 
                HandleProperty(m, pe);
        }
    }

    private void HandleBaseInterfaces(Type intf,
                                      byte level,
                                      out ImmList<Type> declaredBaseIntfs,
                                      out ImmSet<Type> indirectBaseIntfs,
                                      out ImmSet<Type> allBaseIntfs)
    {
        ImmList<Type> allBaseInterfaces =
            intf.GetInterfaces()
                .Where(i => i.IsAssignableTo(matterType))
                .ToImmList();
        ImmSet<Type> indirectBaseInterfaces =
            allBaseInterfaces
               .SelectMany(i => i.GetInterfaces())
               .ToImmSet();
        ImmList<Type> declaredBaseInterfaces =
            allBaseInterfaces
               .Where(i => i.IsNotIn(indirectBaseInterfaces))
               .ToImmList();
        foreach (var i in declaredBaseInterfaces)
            if (!mm.Intfs.ContainsKey(i))
                HandleInterface(i, false, level);
        declaredBaseIntfs = declaredBaseInterfaces;
        indirectBaseIntfs = indirectBaseInterfaces;
        allBaseIntfs = allBaseInterfaces.ToImmSet();
    }

    private void HandleFamily(MetaMatter parent, PropertyInfo prop, byte parentLevel)
    {
        var propType = prop.PropertyType;
        var propIntf = propType.GenericTypeArguments[0];
        if (!mm.Intfs.ContainsKey(propIntf)) 
            HandleInterface(propIntf, true, parentLevel.Succ);
        var child = mm.Intfs[propIntf];
        Debug.Assert(child is not null);
        var family = new MetaFamily(parent, child, propType, prop.Name);
        parent.AddFamily(family);
    }

    private void HandleRef(MetaMatter m, PropertyInfo prop)
    {
        var refName         = prop.Name;
        var refPropType     = prop.PropertyType;
        var refTargetIntf   = refPropType.GenericTypeArguments[0];
        var refTargetMatter = mm.Intfs[refTargetIntf];
        Debug.Assert(refTargetMatter is not null);
        var poly = refPropType.IsAssignableTo(polyRefType);
        var r    = new MetaRef(m, refName, poly, refTargetMatter);
        m.OwnRefs.Add(r);
    }

    private void HandleProperty(MetaMatter m, PropertyInfo prop)
    {
        var proName = prop.Name;
        var proType = prop.PropertyType;
        var p       = new MetaProperty(m, proName, proType);
        m.OwnProperties.Add(p);
    }
}