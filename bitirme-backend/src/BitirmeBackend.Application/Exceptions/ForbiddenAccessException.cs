namespace BitirmeBackend.Application.Exceptions;

/// <summary>
/// Represents an authenticated request that is not allowed by role, ownership, or team scope.
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
