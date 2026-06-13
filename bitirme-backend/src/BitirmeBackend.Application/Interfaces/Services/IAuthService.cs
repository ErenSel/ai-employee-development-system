using BitirmeBackend.Contracts.Requests;
using BitirmeBackend.Contracts.Responses;

namespace BitirmeBackend.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);

    /// <summary>
    /// Revokes the given refresh token. Resolves the owning user from the token itself, so it
    /// works without a valid access token and is idempotent — an unknown/already-revoked/expired
    /// token still completes successfully (no exception).
    /// </summary>
    Task LogoutAsync(string refreshToken);
    Task<CurrentUserResponse> GetCurrentUserAsync(int userId);
}
