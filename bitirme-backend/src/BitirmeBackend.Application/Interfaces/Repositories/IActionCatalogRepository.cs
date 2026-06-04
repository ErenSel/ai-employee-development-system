using BitirmeBackend.Domain.Entities;

namespace BitirmeBackend.Application.Interfaces.Repositories;

public interface IActionCatalogRepository
{
    /// <summary>Lookup by the string ActionId primary key (e.g. "DEPT_COMP1_03").</summary>
    Task<ActionCatalog?> GetByIdAsync(string actionId);

    /// <summary>Alias for <see cref="GetByIdAsync(string)"/> kept for call-site clarity.</summary>
    Task<ActionCatalog?> GetByCodeAsync(string code);

    Task<IEnumerable<ActionCatalog>> GetByCodesAsync(IEnumerable<string> codes);
    Task<IEnumerable<ActionCatalog>> GetAllActiveAsync();
}
