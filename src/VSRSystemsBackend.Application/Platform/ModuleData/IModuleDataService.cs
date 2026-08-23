namespace VSRSystemsBackend.Application.Platform.ModuleData;

public interface IModuleDataService
{
    Task<string?> GetAsync(string module, string collection, CancellationToken cancellationToken = default);
    Task SaveAsync(string module, string collection, string json, CancellationToken cancellationToken = default);
}
