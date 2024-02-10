using System.Collections.Immutable;
using System.Diagnostics;
using Model.Essence.Abstracts;
using Util.Common.Fun;

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
            m.IsMedium = m.Families.Count > 0;
            
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
            
            // families
            
            
            // Imm
            m.Imm.ManuallyImplemented = m.Name.isIn(ManualImplementedMatters);
            m.Imm.ClassName           = "Imm" + m.IntfName;
            m.Imm.BaseClassName       = baseMatter.Imm.ClassName;
        }
        
    }
    
    
}
