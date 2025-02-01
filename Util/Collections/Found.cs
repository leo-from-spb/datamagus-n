namespace Util.Collections;

/// <summary>
/// Result of the search in a collection.
/// </summary>
/// <typeparam name="T">type of the element.</typeparam>
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

    public static T operator | (Found<T> f, T ersatz) => f.Ok ? f.Item : ersatz;

    public static Found<T> NotFound
    {
        get
        {
            #pragma warning disable CS8604
            return new Found<T>(false, default);
            #pragma warning restore CS8604
        }
    }

    public override string ToString() => Ok ? $"{Item}" : "<nothing>";
}

