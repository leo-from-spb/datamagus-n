using Model.Abstracts;
using Model.Concept;

namespace Model.Visuality;


/// <summary>
/// Entity avatar, a box that represents an entity.
/// </summary>
public interface DiaEntity : DiaBoxAvatar
{
    /// <summary>
    /// The entity which is the subject of this avatar.
    /// </summary>
    MonoRef<ConEntity> Entity { get; }
    MonoRef<Matter> DiaAvatar.Subject => Entity;
    
}
