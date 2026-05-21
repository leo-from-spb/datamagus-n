namespace Core.Services;

/// <summary>
/// Service.
/// This interface contains several common methods that are called by the ServiceMill.
/// </summary>
public interface Service
{
    /// <summary>
    /// Name of the service.
    /// </summary>
    public string ServiceName => this.GetType().Name;

    /// <summary>
    /// This method is called before shutting down, in the normal order.
    /// In this method the service should stop all it's activities (file and network operations, threads, etc.).
    /// </summary>
    public void Finalizing() { }

    /// <summary>
    /// This method is called on shut down, in the reverse order.
    /// </summary>
    public void Shutdown() { }
}
