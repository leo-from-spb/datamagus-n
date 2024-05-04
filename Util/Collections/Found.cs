namespace Util.Collections;

/// <summary>
/// Result of the search in a collections.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Found<T>
{
    /// <summary>
    /// The item that is found.
    /// </summary>
    public readonly T Item;

    /// <summary>
    /// Whether the search was successful.
    /// </summary>
    public readonly bool Ok;

    /// <summary>
    /// Prepares this structure.
    /// </summary>
    /// <param name="ok">whether the search was success.</param>
    /// <param name="item">the found item.</param>
    public Found(bool ok, T item)
    {
        Item = item;
        Ok   = ok;
    }


    public static implicit operator T (Found<T> f) => f.Item;


    public override string ToString() => Ok ? $"{Item}" : "<nothing>";
}
