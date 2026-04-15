using System.Collections.Generic;
using Util.Extensions;

namespace Model.Abstracts;

public abstract class ImmRef<M> : Ref<M>
    where M : Matter
{
    public abstract bool Exists { get; }
}


public sealed class ImmMonoRef<M> : ImmRef<M>, MonoRef<M>
    where M : Matter
{
    public uint Id { get; }

    public override bool Exists => Id != 0u;

    public ImmMonoRef(uint id)
    {
        Id = id;
    }

    public override string ToString() => Id != 0 ? Id.ToString() : "nil";
}


public sealed class ImmPolyRef<M> : ImmRef<M>, PolyRef<M>
    where M : Matter
{
    public IReadOnlyList<M> Ids { get; }

    public override bool Exists => Ids.IsNotEmpty();

    public ImmPolyRef(IReadOnlyList<M> ids)
    {
        Ids = ids; // TODO ensure the list is read-only
    }
    
    public override string ToString() => Ids.JoinToString(i=>i.ToString(), ",", empty: "none");
}



public static class ImmRefExtensions
{

    public static ImmMonoRef<M> ToImmRef<M>(this MonoRef<M> mr)
        where M : class, Matter
        => mr as ImmMonoRef<M> ?? new ImmMonoRef<M>(mr.Id);

    public static ImmPolyRef<M> ToImmRef<M>(this PolyRef<M> mr)
        where M : class, Matter
        => mr as ImmPolyRef<M> ?? new ImmPolyRef<M>(mr.Ids);
    


}