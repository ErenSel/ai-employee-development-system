namespace BitirmeBackend.Application.Interfaces.Services;

/// <summary>
/// ML modeli yetkinlikleri pozisyonel jenerik slot kodlarıyla döndürür
/// (Dept_Comp1..3, Role_Comp1..2). Bu resolver, çalışanın departman/rolüne göre
/// bu kodları gerçek yetkinlik adlarına çevirir. Eşleşme bulunamazsa kodu olduğu
/// gibi döndürür (güvenli fallback).
/// </summary>
public interface ICompetencyLabelResolver
{
    string Resolve(string? code, string? department, string? jobRole);
}
