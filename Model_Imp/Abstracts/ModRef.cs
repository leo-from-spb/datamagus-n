using System.Collections.Generic;
using Util.Extensions;

namespace Model.Abstracts;


public abstract class ModRef<M> : Ref<M>
    where M : Matter
{
    public abstract bool Exists { get; }
}



public sealed class ModMonoRef<M> : ModRef<M>, MonoRef<M>
    where M : Matter
{
    public uint Id { get; set; }


    public ModMonoRef() { }

    public ModMonoRef(uint id)
    {
        Id = id;
    }

    public ModMonoRef(MonoRef<Matter> origin)
    {
        Id = origin.Id;
    }

    public override bool Exists => Id != 0u;

    public override string ToString() => Id != 0 ? Id.ToString() : "nil";
}



public sealed class ModPolyRef<M> : ModRef<M>, PolyRef<M>
    where M : Matter
{
    private List<uint> MyIds = new List<uint>();


    public ModPolyRef() { }

    public ModPolyRef(PolyRef<M> originRef)
    {
        MyIds.AddRange(originRef.Ids);
    }


    public override bool Exists => MyIds.IsNotEmpty();

    public IReadOnlyList<uint> Ids => MyIds;
    
    public override string ToString() => MyIds.JoinToString(i=>i.ToString(), ",", empty: "none");
}