using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Services;

/// <summary>
/// Service interface for JWT token operations.
/// Handles token generation and validation for user authentication.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT access token for the authenticated user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>The JWT token as a string.</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a refresh token for token renewal.
    /// </summary>
    /// <returns>A new refresh token string.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates if the provided token is valid and extracts the user ID.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>The user ID if valid, null otherwise.</returns>
    string? ValidateTokenAndGetUserId(string token);
}