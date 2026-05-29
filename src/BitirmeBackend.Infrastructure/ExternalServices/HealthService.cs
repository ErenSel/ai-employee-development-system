using BitirmeBackend.Application.Interfaces.Services;

namespace BitirmeBackend.Infrastructure.ExternalServices;

public class HealthService : IHealthService
{
    public Task<object> GetHealthStatusAsync()
    {
        var result = new
        {
            status = "degraded",
            database = "mock",
            mlService = "unknown"
        };
        return Task.FromResult<object>(result);
    }
}
