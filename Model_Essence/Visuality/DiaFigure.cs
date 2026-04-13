using Model.Abstracts;
using Util.Geometry;

namespace Model.Visuality;

/// <summary>
/// Figure on a diagram.
/// </summary>
public interface DiaFigure : TermMatter
{
    abstract GeoRect? Bounds { get; }
}

/// <summary>
/// A static figure one a diagram.
/// </summary>
public interface DiaShape : DiaFigure
{
    [MatterProperty]
    GeoRect? Place { get; }
    GeoRect? DiaFigure.Bounds => Place;

    [MatterProperty]
    int LayerOrder { get; }

    [MatterProperty]
    string? Color { get; }

    [MatterProperty]
    string? Note { get; }
}


/// <summary>
/// Avatar is a figure on a diagram that represents a conceptual or database element.
/// The represented element is named Subject.
/// </summary>
public interface DiaAvatar : DiaFigure
{
    /// <summary>
    /// Subject — the matter which this avatar represents.
    /// </summary>
    abstract MonoRef<Matter> Subject { get; }
}


/// <summary>
/// Avatar that is a rectangle with active information inside.
/// </summary>
public interface DiaBoxAvatar : DiaAvatar
{
    /// <summary>
    /// The rectangular place.
    /// </summary>
    [MatterProperty]
    GeoRect? Place { get; }
    GeoRect? DiaFigure.Bounds => Place;
}
