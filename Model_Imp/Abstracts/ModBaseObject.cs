namespace Model.Abstracts;

/// <summary>
/// Base class for a modifiable matter.
/// </summary>
public abstract class ModBaseObject
{
    /// <summary>
    /// The master model accessor.
    /// </summary>
    public readonly ModelMaster Master;

    /// <summary>
    /// Parent modifiable object, or null if this is a root.
    /// </summary>
    public readonly ModBaseObject? Parent;

    /// <summary>
    /// Creates a root modifiable object.
    /// </summary>
    /// <param name="master">a master model accessor.</param>
    protected ModBaseObject(ModelMaster master)
    {
        Master = master;
        Parent = null;
    }

    /// <summary>
    /// Creates a regular modifiable object.
    /// </summary>
    /// <param name="parent">the parent.</param>
    protected ModBaseObject(ModBaseObject parent)
    {
        Master = parent.Master;
        Parent = parent;
    }
}
