using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Model.Abstracts;
using Util.Extensions;

namespace Model.Generation;

internal class MetaProcessor (MetaModel mm)
{
    private readonly ImmutableHashSet<string> ManualImplementedMatters =
    [
        nameof(Matter),
        nameof(MediumMatter),
        nameof(TermMatter),
        nameof(NamedMatter),
        nameof(NamedMediumMatter),
        nameof(NamedTermMatter)
    ];


    private readonly Type matterType      = typeof(Matter);
    private readonly Type namedMatterType = typeof(NamedMatter);
    private readonly Type familyType      = typeof(Family<Matter>);
    private readonly Type refType         = typeof(Ref<Matter>);
    private readonly Type polyRefType     = typeof(PolyRef<Matter>);

    internal void ProcessModel()
    {
        MetaMatter baseMediumMatter      = mm.Intfs[typeof(MediumMatter)],
                   baseTermMatter        = mm.Intfs[typeof(TermMatter)],
                   baseNamedMediumMatter = mm.Intfs[typeof(NamedMediumMatter)],
                   baseNamedTermMatter   = mm.Intfs[typeof(NamedTermMatter)];
        Debug.Assert(baseMediumMatter is not null);
        Debug.Assert(baseTermMatter is not null);
        Debug.Assert(baseNamedMediumMatter is not null);
        Debug.Assert(baseNamedTermMatter is not null);
        
        foreach (var m in mm.Matters)
        {
            m.IsMedium = m.Intf.IsAssignableTo(typeof(MediumMatter));
            m.HasName  = m.AllBaseIntfs.Contains(namedMatterType);

            // select the base type
            var baseMatter = m.HasName 
                ? m.IsMedium ? baseNamedMediumMatter : baseNamedTermMatter 
                : m.IsMedium ? baseMediumMatter : baseTermMatter;
            m.BaseMatter = baseMatter;
            
            // handle base interfaces
            foreach (var bi in m.DeclaredBaseIntfs)
            {
                var bm = mm.Intfs.Get(bi);
                if (bm is not null)
                {
                    m.DeclaredBaseMatters.Add(bm);
                }
            }  
            
            // base matters
            m.BaseMatters.AddRange(m.DeclaredBaseMatters);
            m.DeclaredBaseMatters.ForEach(b => b.DirectInheritors.Add(m));

            // families
            if (m.IsMedium)
            {
                var familyEntries =
                    from child in m.Intf.GetProperties()
                    where child.MemberType == MemberTypes.Property
                       && child.PropertyType.IsAssignableTo(familyType)
                    select child;
                foreach (var fe in familyEntries)
                    HandleFamily(m, fe);

                foreach (var bm in m.BaseMatters)
                {
                    foreach (var f in bm.OwnFamilies)
                        if (!m.AllFamilies.ContainsKey(f.FamilyName))
                        {
                            var newF = f.cloneFor(m);
                            m.AllFamilies[newF.FamilyName] = newF;
                        }

                    foreach (var f in m.OwnFamilies)
                    {
                        m.AllFamilies[f.FamilyName] = f;
                    }
                }
            }

            // references
            var refEntries =
                from r in m.Intf.GetProperties()
                where r.MemberType == MemberTypes.Property
                   && r.PropertyType.IsAssignableTo(refType)
                select r;
            foreach (var pe in refEntries)
                HandleRef(m, pe);

            foreach (var bm in m.BaseMatters)
            {
                foreach (var r in bm.OwnRefs)
                {
                    m.AllRefs[r.RefName] = r;
                }
            }
            foreach (var r in m.OwnRefs)
            {
                m.AllRefs[r.RefName] = r;
            }

            // properties
            var proEntries =
                from p in m.Intf.GetProperties()
                where p.MemberType == MemberTypes.Property
                   && p.GetCustomAttribute<MatterPropertyAttribute>() != null
                   && !p.PropertyType.IsAssignableTo(familyType)
                   && !p.PropertyType.IsAssignableTo(refType)
                select p;
            foreach (var pe in proEntries)
                HandleProperty(m, pe);

            foreach (var bm in m.BaseMatters)
            {
                foreach (var p in bm.OwnProperties)
                    if (!m.AllProperties.ContainsKey(p.ProName))
                    {
                        var newP = p.cloneFor(m);
                        m.AllProperties[newP.ProName] = newP;
                    }
            }
            foreach (var p in m.OwnProperties)
            {
                m.AllProperties[p.ProName] = p;
                p.ImplementedInMatter      = m.IntfName.IsIn(ManualImplementedMatters);
            }
            
            bool hasChildren = m.AllFamilies.IsNotEmpty();
            if (m.IsConcrete && m.IsMedium != hasChildren)
                Console.Error.WriteLine($"Matter {m.Name} families: {m.AllFamilies.Count}, but IsMedium: {m.IsMedium}");

            // Imm
            m.Imm.ManuallyImplemented = m.Name.IsIn(ManualImplementedMatters);
            m.Imm.ClassName           = "Imm" + m.IntfName;
            m.Imm.BaseClassName       = baseMatter.Imm.ClassName;
        }
        
    }


    private void HandleFamily(MetaMatter host, PropertyInfo prop)
    {
        var propType = prop.PropertyType;
        var propIntf = propType.GenericTypeArguments[0];
        Debug.Assert(propIntf is not null);
        Debug.Assert(propIntf.IsIn(ModelMetaBrief.AllModelMatters));
        var child = mm.Intfs[propIntf];
        Debug.Assert(child is not null);
        var family = new MetaFamily(host, child, propType, prop.Name);
        host.AddFamily(family);
    }

    private void HandleRef(MetaMatter host, PropertyInfo prop)
    {
        var refName         = prop.Name;
        var refPropType     = prop.PropertyType;
        var refTargetIntf   = refPropType.GenericTypeArguments[0];
        var refTargetMatter = mm.Intfs[refTargetIntf];
        Debug.Assert(refTargetMatter is not null);
        var poly = refPropType.IsAssignableTo(polyRefType);
        var r    = new MetaRef(host, refName, poly, refTargetMatter);
        host.OwnRefs.Add(r);
    }

    private void HandleProperty(MetaMatter host, PropertyInfo prop)
    {
        var proName = prop.Name;
        var proType = prop.PropertyType;

        bool nullWrapped = proType.IsValueType && proType.IsGenericType && proType.Name.StartsWith("Nullable`");
        if (nullWrapped)
            proType = proType.GenericTypeArguments[0];

        var p = new MetaProperty(host, proName, proType, nullWrapped);
        host.OwnProperties.Add(p);
    }

}
