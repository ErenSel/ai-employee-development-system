namespace BitirmeBackend.Contracts.Requests;

/// <summary>
/// Confirmation payload for the destructive "reset transactional data" maintenance operation.
/// </summary>
public class ResetTransactionalDataRequest
{
    /// <summary>Must equal the literal "RESET" for the operation to proceed (safety guard).</summary>
    public string Confirmation { get; set; } = string.Empty;
}
