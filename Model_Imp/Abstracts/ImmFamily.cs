using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Essence.Abstracts;

namespace Model.Imp.Abstracts;

/// <summary>
/// Abstract immutable family.
/// </summary>
/// <typeparam name="M">matter type.</typeparam>
public class ImmFamily<M> : Family<M> 
    where M: Matter
{
    private protected readonly M[] matters;

    
    public static ImmFamily<M> of(params M[] matters) => new ImmFamily<M>(matters);

    public ImmFamily(M[] matters)
    {
        this.matters = matters;
    }

    
    public IEnumerator<M> GetEnumerator() => matters.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => matters.GetEnumerator();

    public int Count => matters.Length;
}



public class ImmNamingFamily<M> : ImmFamily<M>, NamingFamily<M> 
    where M : NamedMatter
{
    public new static ImmNamingFamily<M> of(params M[] matters) => new ImmNamingFamily<M>(matters);
    
    public ImmNamingFamily(M[] matters) 
        : base(matters)
    {
    }

    public M? this[string name]
    {
        get
        {
            foreach (M m in matters)
                if (m.Name == name) return m;
            return default(M?);
        }
    }
}
