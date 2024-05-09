namespace Model.Abstracts;

/// <summary>
/// Matter — an abstract model element.
/// Every model element implements this interface.
/// </summary>
public interface Matter : Node
{
    /// <summary>
    /// Identifier of this element.
    /// This identifier is unique within the model.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// The version of this element.
    /// </summary>
    uint Version { get; }
}



/// <summary>
/// Matter that has families.
/// </summary>
public interface MediumMatter : Matter
{
    
}


/// <summary>
/// Terminal matter (that has no inner elements).
/// </summary>
public interface TermMatter : Matter
{
    
}


/// <summary>
/// Matter that has a name.
/// </summary>
public interface NamedMatter : Matter
{
    public string? Name { get; }
}


public interface NamedMediumMatter : MediumMatter, NamedMatter { }

public interface NamedTermMatter : TermMatter, NamedMatter { }
