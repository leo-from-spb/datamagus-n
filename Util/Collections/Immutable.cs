using System.Collections.Generic;

namespace Util.Collections;


/// <summary>
/// Immutable thing.
/// </summary>
public abstract class Immutable
{

}


/// <summary>
/// Base class for every immutable collection class.
/// </summary>
/// <typeparam name="T">type of the element.</typeparam>
public abstract class ImmutableCollection<T> : Immutable
{

    /// <summary>
    /// The equality comparer that should be used for check on equality.
    /// </summary>
    protected static readonly EqualityComparer<T> eq = EqualityComparer<T>.Default;

    /// <summary>
    /// The constructor is for internal usage only.
    /// </summary>
    internal ImmutableCollection() { }

    /// <summary>
    /// This property is for internal usage only.
    /// </summary>
    public ImmutableCollection<T> Imp => this;

}
