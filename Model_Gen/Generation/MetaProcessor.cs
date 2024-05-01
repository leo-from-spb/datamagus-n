using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Model.Essence.Abstracts;
using Util.Fun;

namespace Model.Gen.Generation;

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

    private readonly Type namedMatterType = typeof(NamedMatter);
    

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
            m.IsMedium = m.OwnFamilies.Count > 0;
            m.HasName  = m.AllBaseIntfs.Contains(namedMatterType);
            
            // select the base type
            var baseMatter = m.HasName 
                ? m.IsMedium ? baseNamedMediumMatter : baseNamedTermMatter 
                : m.IsMedium ? baseMediumMatter : baseTermMatter;
            m.BaseMatter = baseMatter;
            
            // handle base interfaces
            foreach (var bi in m.AllBaseIntfs)
            {
                var bm = mm.Intfs.Get(bi);
                if (bm is not null)
                {
                    m.DeclaredBaseMatters.Add(bm);
                }
            }  
            
            // base matters
            m.BaseMatters.AddRange(m.DeclaredBaseMatters);
            
            // families
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
            
            // properties
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
                p.ImplementedInMatter      = m.IntfName.isIn(ManualImplementedMatters);
            }
            
            // Imm
            m.Imm.ManuallyImplemented = m.Name.isIn(ManualImplementedMatters);
            m.Imm.ClassName           = "Imm" + m.IntfName;
            m.Imm.BaseClassName       = baseMatter.Imm.ClassName;
        }
        
    }
    
    
}
