namespace BitirmeBackend.Application.Interfaces;

/// <summary>
/// Generates a personalized, human-readable evaluation text for an action plan
/// using an external LLM (DeepSeek). Implementations must be resilient: if the
/// remote call fails or times out they return an empty string so PDF generation
/// can still proceed without the summary section.
/// </summary>
public interface ILlmReportService
{
    Task<string> GenerateActionPlanSummaryAsync(
        string employeeName,
        string department,
        string jobRole,
        List<(string CompetencyName, double Score)> competencyScores,
        List<string> actionItemTitles);
}
