using Model.Abstracts;

namespace Model.Concept;

public interface ConDomain : NamedTermMatter
{
    [MatterProperty]
    string? ContentType { get; }
}
