using System.Collections.Generic;
using System.Linq;

namespace Model.Gears.Mapping;

/// <summary>
/// Read-only interface for the main map in the Model.
/// </summary>
public interface CascadingMap<E>
    where E: class
{
    /// <summary>
    /// Returns a one-moment snapshot of all held versions, in the natural order.
    /// </summary>
    /// <returns>an immutable list of all versions.</returns>
    public IReadOnlyList<uint> ListAllVersions();

    /// <summary>
    /// The requested version cascade.
    /// </summary>
    /// <param name="version"></param>
    /// <returns>the dictionary related to the given version, immutable;
    ///          or null if not found.</returns>
    public IReadOnlyDictionary<uint,E>? GetVersionDict(uint ver);

}
