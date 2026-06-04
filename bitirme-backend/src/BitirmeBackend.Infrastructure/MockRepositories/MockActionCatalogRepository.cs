using BitirmeBackend.Application.Interfaces.Repositories;
using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Infrastructure.MockRepositories;

public class MockActionCatalogRepository : IActionCatalogRepository
{
    private static IEnumerable<ActionCatalog> Active =>
        MockDataStore.ActionCatalog.Where(c => !c.IsDeleted);

    public Task<ActionCatalog?> GetByIdAsync(string actionId) =>
        Task.FromResult(Active.FirstOrDefault(c =>
            c.ActionId.Equals(actionId, StringComparison.OrdinalIgnoreCase)));

    public Task<ActionCatalog?> GetByCodeAsync(string code) =>
        Task.FromResult(Active.FirstOrDefault(c =>
            c.ActionId.Equals(code, StringComparison.OrdinalIgnoreCase)));

    public Task<IEnumerable<ActionCatalog>> GetByCodesAsync(IEnumerable<string> codes)
    {
        var codeSet = codes.Select(c => c.ToUpper()).ToHashSet();
        var result = Active.Where(c => codeSet.Contains(c.ActionId.ToUpper()));
        return Task.FromResult(result);
    }

    public Task<IEnumerable<ActionCatalog>> GetAllActiveAsync() =>
        Task.FromResult(Active.Where(c => c.IsActive));
}
