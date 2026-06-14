using System.Reflection;
using System.Text.Json;
using BitirmeBackend.Application.Interfaces.Services;

namespace BitirmeBackend.Infrastructure.Services;

/// <summary>
/// competency_mapping.json (embedded resource) tabanlı resolver. Veri statik
/// referanstır (sabit ML modeline bağlı) — bu yüzden DB yerine gömülü kaynak.
/// Sözlük bir kez yüklenir (lazy) ve thread-safe paylaşılır.
/// </summary>
public class CompetencyLabelResolver : ICompetencyLabelResolver
{
    private const string ResourceName =
        "BitirmeBackend.Infrastructure.Resources.competency_mapping.json";

    private static readonly Lazy<Mapping> _mapping = new(Load, isThreadSafe: true);

    public string Resolve(string? code, string? department, string? jobRole)
    {
        if (string.IsNullOrWhiteSpace(code))
            return string.Empty;

        var m = _mapping.Value;

        if (code.StartsWith("Dept_Comp", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(department)
            && m.Dept.TryGetValue(department, out var deptSlots)
            && deptSlots.TryGetValue(code, out var deptLabel))
        {
            return deptLabel;
        }

        if (code.StartsWith("Role_Comp", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(jobRole)
            && m.Role.TryGetValue(jobRole, out var roleSlots)
            && roleSlots.TryGetValue(code, out var roleLabel))
        {
            return roleLabel;
        }

        // Core_* ve eşleşmeyen her şey: kodu olduğu gibi döndür (güvenli fallback).
        return code;
    }

    private static Mapping Load()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Gömülü kaynak bulunamadı: {ResourceName}");

        var dto = JsonSerializer.Deserialize<MappingDto>(stream)
            ?? throw new InvalidOperationException("competency_mapping.json ayrıştırılamadı.");

        var cmp = StringComparer.OrdinalIgnoreCase;
        var dept = (dto.DeptCompLabels ?? new())
            .ToDictionary(kv => kv.Key, kv => new Dictionary<string, string>(kv.Value, cmp), cmp);
        var role = (dto.RoleCompLabels ?? new())
            .ToDictionary(kv => kv.Key, kv => new Dictionary<string, string>(kv.Value, cmp), cmp);

        return new Mapping(dept, role);
    }

    private sealed record Mapping(
        Dictionary<string, Dictionary<string, string>> Dept,
        Dictionary<string, Dictionary<string, string>> Role);

    private sealed class MappingDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("dept_comp_labels")]
        public Dictionary<string, Dictionary<string, string>>? DeptCompLabels { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("role_comp_labels")]
        public Dictionary<string, Dictionary<string, string>>? RoleCompLabels { get; set; }
    }
}
