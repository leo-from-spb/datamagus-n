using Model.Essence.Abstracts;

namespace Model.Imp.Abstracts;

public abstract class ImmMatter : Matter
{
    protected ImmMatter(uint id)
    {
        Id = id;
    }

    public uint Id { get; }
}



public abstract class ImmNamedMatter : ImmMatter, NamedMatter
{
    protected ImmNamedMatter(uint id, string name) 
        : base(id)
    {
        Name = name;
    }

    public string Name { get; }
}
