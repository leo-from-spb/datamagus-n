using Core.Services;

namespace Core.Gears.Settings;


/// <summary>
/// The service that holds all settings.
/// </summary>
[Service]
public interface SettingService
{

    public SystemSettings    SystemSettings    { get; }
    public WorkspaceSettings WorkspaceSettings { get; }

}
