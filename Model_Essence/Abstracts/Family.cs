namespace Model.Essence.Abstracts;

/// <summary>
/// Family of similar matters.
/// </summary>
/// <typeparam name="M"></typeparam>
public interface Family<out M> : Node, IEnumerable<M>
    where M: Matter
{
    /// <summary>
    /// Number of inner matters.
    /// </summary>
    int Count { get; }
}



public interface NamingFamily<out M> : Family<M>
    where M: NamedMatter
{
    /// <summary>
    /// Gets the inner matter by its name.
    /// </summary>
    /// <param name="name">name of the matter to get, always case sensitively</param>
    public M? this[string name] { get; }
}
