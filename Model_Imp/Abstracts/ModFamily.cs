using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Util.Extensions;

namespace Model.Abstracts;


// ReSharper disable InconsistentNaming  while the bug RIDER-138537 is not fixed


public class ModFamily<M,MM> : Family<M>
    where M: class, Matter
    where MM: ModMatter<M>, M
{
    protected readonly ModBaseObject Host;
    protected readonly Family<M>?    OriginFamily;
    protected readonly List<M>       Matters;

    private readonly ConstructorInfo ChildConstructor;

    private readonly Func<MM> ChildInstantiation;

    public ModFamily(ModBaseObject host, Family<M> originFamily, Func<MM> childInstantiation)
    {
        this.Host             = host;
        this.OriginFamily     = originFamily;
        this.Matters          = new List<M>(originFamily.AsList());
        this.ChildConstructor = getChildConstructor();
    }

    public ModFamily(ModBaseObject host, Func<MM> childInstantiation)
    {
        this.Host             = host;
        this.OriginFamily     = null;
        this.Matters          = new List<M>();
        this.ChildConstructor = getChildConstructor();
    }

    private static ConstructorInfo getChildConstructor()
    {
        ConstructorInfo? constructor = typeof(MM).GetConstructor([typeof(ModBaseObject)]);
        if (constructor is null) throw new InvalidOperationException($"Class {typeof(MM).Name} has no appropriate constructor");
        return constructor;
    }

    public MM New()
    {
        ConstructorInfo? constructor = typeof(MM).GetConstructor([typeof(ModBaseObject)]);
        if (constructor is null) throw new InvalidOperationException($"Class {typeof(MM).Name} has no appropriate constructor");

        MM? newOne = constructor.Invoke([Host]) as MM;  //ChildInstantiation.Invoke();
        if (newOne is null) throw new InvalidOperationException($"Cannot instantiate {typeof(MM).Name}");
        
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

