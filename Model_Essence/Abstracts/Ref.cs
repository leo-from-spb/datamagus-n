using System.Collections.Generic;

namespace Model.Abstracts;

/// <summary>
/// Base interface to a reference to one or several matters in the same model.
/// </summary>
/// <typeparam name="M">the type of target matters.</typeparam>
/// <seealso cref="MonoRef{E}"/>
/// <seealso cref="PolyRef{E}"/>
public interface Ref<out M>
    where M: Matter
{
    /// <summary>
    /// Whether the reference exists.
    /// </summary>
    bool Exists { get; }
}


/// <summary>
/// Reference to a single another matter in the same model.
/// </summary>
/// <typeparam name="M">the type of the target matter.</typeparam>
public interface MonoRef<out M> : Ref<M>
    where M : Matter
{
    /// <summary>
    /// Id of the target element.
    /// Zero means no reference.
    /// </summary>
    uint Id { get; }
}


/// <summary>
/// Reference to several other matters in the same model.
/// </summary>
/// <typeparam name="M">the type of target matters.</typeparam>
public interface PolyRef<out M> : Ref<M>
    where M : Matter
{
    /// <summary>
    /// Ids of target elements.
    /// </summary>
    IReadOnlyList<M> Ids { get; }
}



