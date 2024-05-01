using System.Collections.Generic;

namespace Model.Abstracts;

/// <summary>
/// Family of similar matters.
/// </summary>
/// <typeparam name="M"></typeparam>
public interface Family<out M> : Node, IEnumerable<M>
    where M: Matter
{
    /// <summary>
    /// This family has children.
    /// </summary>
    bool IsNotEmpty { get; }

    /// <summary>
    /// This family is empty.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Number of inner matters.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Gets a matter by its id.
    /// </summary>
    /// <param name="id">id of the matter to get.</param>
    /// <returns>the matter, or null if no.</returns>
    public M? ById(uint id);

    /// <summary>
    /// Gets all ids preserving the order.
    /// Every call instantiates a new array.
    /// </summary>
    /// <returns>array of all ids.</returns>
    public uint[] GetAllIds();

    /// <summary>
    /// Returns the children as an array.
    /// Every call instantiates a new array.
    /// </summary>
    /// <returns>an array with all children.</returns>
    public M[] ToArray();

    /// <summary>
    /// Returns a list view on the children,
    /// without copying.
    /// </summary>
    /// <returns>the children as a list.</returns>
    public IReadOnlyList<M> AsList();
}



public interface NamingFamily<out M> : Family<M>
    where M: NamedMatter
{
    /// <summary>
    /// Gets the inner matter by its name.
    /// </summary>
    /// <param name="name">name of the matter to get, always case sensitively</param>
    public M? this[string name] { get; }

    /// <summary>
    /// All non-null names.
    /// </summary>
    /// <returns>all names.</returns>
    public string[] GetAllNames();
}
