namespace E_Commerce.Application.DTOs.Users;

/// <summary>
/// Request DTO for user login.
/// Contains email and password for authentication.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password (plain text, will be hashed for comparison).
    /// </summary>
    public string Password { get; set; } = string.Empty;
}