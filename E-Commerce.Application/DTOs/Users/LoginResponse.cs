namespace E_Commerce.Application.DTOs.Users;

/// <summary>
/// Response DTO for successful login.
/// Contains authentication tokens and user information.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Indicates if login was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message describing the result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// JWT access token for API authentication.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Refresh token for obtaining new access tokens.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Token expiration time in UTC.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// User ID of the authenticated user.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// User's full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// User's role (Customer, Admin, etc.).
    /// </summary>
    public string? Role { get; set; }
}