using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Model.Abstracts;

/// <summary>
/// Abstract immutable family.
/// </summary>
/// <typeparam name="M">matter type.</typeparam>
public class ImmFamily<M> : Family<M>
    where M: class, Matter
{
    private protected readonly M[] matters;

    
    public static ImmFamily<M> Of(params M[] matters) => new ImmFamily<M>(matters);

    public ImmFamily(M[] matters)
    {
        this.matters = matters;
    }

    public bool IsNotEmpty => matters.Length > 0;
    public bool IsEmpty    => matters.Length == 0;
    public int  Count      => matters.Length;

    public M? ById(uint id)
    {
        foreach (var matter in matters)
            if (matter.Id == id)
                return matter;
        return null;
    }

    public uint[] GetAllIds()
    {
        int    n   = matters.Length;
        uint[] arr = new uint[n];

        for (int i = 0; i < n; i++) arr[i] = matters[i].Id;
        return arr;
    }

    public M[] ToArray()
    {
        int n   = matters.Length;
        M[] arr = new M[n];
        matters.CopyTo(arr, 0);
        return arr;
    }

    public IReadOnlyList<M> AsList()
    {
        return matters.AsReadOnly();
    }

    public IEnumerator<M> GetEnumerator() => matters.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => matters.GetEnumerator();

}



public class ImmNamingFamily<M> : ImmFamily<M>, NamingFamily<M> 
    where M : class, NamedMatter
{
    public new static ImmNamingFamily<M> Of(params M[] matters) => new ImmNamingFamily<M>(matters);
    
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

    public string[] GetAllNames() => matters
                                    .Where(m => m.Name is not null)
                                    .Select(m => m.Name!)
                                    .ToArray();
}
