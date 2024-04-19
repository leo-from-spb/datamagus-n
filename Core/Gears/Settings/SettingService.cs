using Core.Services;

namespace Core.Gears.Settings;


[Service]
public interface SettingService
{

    public SystemSettings    SystemSettings    { get; }
    public WorkspaceSettings WorkspaceSettings { get; }

}
