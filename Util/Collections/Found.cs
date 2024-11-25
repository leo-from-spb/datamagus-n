namespace Util.Collections;

/// <summary>
/// Result of the search in a collections.
/// </summary>
/// <typeparam name="V">type of the value.</typeparam>
public readonly struct Found<V>
{
    /// <summary>
    /// The item that is found.
    /// </summary>
    public readonly V Item;

    /// <summary>
    /// Whether the search was successful.
    /// </summary>
    public readonly bool Ok;

    /// <summary>
    /// Prepares this structure.
    /// </summary>
    /// <param name="item">the found item.</param>
    /// <param name="ok">whether the search was success.</param>
    public Found(V item, bool ok)
    {
        Item = item;
        Ok   = ok;
    }


    public static implicit operator V (Found<V> f) => f.Item;


    public override string ToString() => Ok ? $"{Item}" : "<nothing>";
}
