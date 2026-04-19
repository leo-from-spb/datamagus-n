using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util.Extensions;

namespace Model.Abstracts;


public class ModFamily<M,MM> : Family<M>
    where M: class, Matter
    where MM: ModMatter<M>, M
{
    protected readonly ModBaseObject Host;
    protected readonly Family<M>?    OriginFamily;
    protected readonly List<M>       Matters;

    private readonly Func<MM> ChildInstantiation;

    public ModFamily(ModBaseObject host, Family<M> originFamily, Func<MM> childInstantiation)
    {
        this.Host = host;
        this.OriginFamily = originFamily;
        this.Matters      = new List<M>(originFamily.AsList());
        this.ChildInstantiation = childInstantiation;
    }

    public ModFamily(ModBaseObject host, Func<MM> childInstantiation)
    {
        this.Host = host;
        this.OriginFamily = null;
        this.Matters      = new List<M>();
        this.ChildInstantiation = childInstantiation;
    }

    public MM New()
    {
        MM newOne = ChildInstantiation.Invoke();
        Matters.Add(newOne);
        return newOne;
    }

    public bool Any     => Matters.IsNotEmpty();
    public bool IsEmpty => Matters.IsEmpty();
    public int  Count   => Matters.Count;

    public M?     ById(uint id) => Matters.Find(m => m.Id == id);
    public uint[] GetAllIds()   => Matters.Select(m => m.Id).ToArray();

    public M[]              ToArray() => Matters.ToArray();
    public IReadOnlyList<M> AsList()  => Matters;

    public IEnumerator<M>   GetEnumerator() => Matters.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Matters.GetEnumerator();
    
}



public class ModNamingFamily<M,MM> : ModFamily<M,MM>, NamingFamily<M>
    where M : class, NamedMatter
    where MM: ModNamedMatter<M>, M
{
    public ModNamingFamily(ModBaseObject host, NamingFamily<M> originFamily, Func<MM> childInstantiation)
        : base(host, originFamily, childInstantiation)
    { }

    public ModNamingFamily(ModBaseObject host, Func<MM> childInstantiation)
        : base(host, childInstantiation)
    { }

    public MM New(string? name)
    {
        MM newOne = this.New();
        newOne.Name = name;
        return newOne;
    }

    public M? this[string name] => Matters.Find(m => m.Name == name);

    public string[] GetAllNames() => Matters.Where(m => m.Name is not null)
                                            .Select(m => m.Name!)
                                            .ToArray();

}

