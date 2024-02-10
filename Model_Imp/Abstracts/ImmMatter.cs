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


public abstract class ImmMediumMatter : ImmMatter, MediumMatter
{
    protected ImmMediumMatter(uint id) : base(id) { }
    
}


public abstract class ImmTermMatter : ImmMatter, TermMatter
{
    protected ImmTermMatter(uint id) : base(id) { }
    
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


public abstract class ImmNamedMediumMatter : ImmMediumMatter, NamedMediumMatter
{
    protected ImmNamedMediumMatter(uint id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; }
}


public abstract class ImmNamedTermMatter : ImmTermMatter, NamedTermMatter
{
    protected ImmNamedTermMatter(uint id, string name) : base(id)
    {
        Name = name;
    }
    
    public string Name { get; }
}
