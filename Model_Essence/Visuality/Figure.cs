using Model.Abstracts;

namespace Model.Visuality;

public interface DiaFigure : TermMatter
{
    [MatterProperty]
    string? Note { get; }
}


public interface DiaShape : DiaFigure
{
    [MatterProperty]
    string? Color { get; }
}

