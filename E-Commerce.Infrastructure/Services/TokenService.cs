using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Infrastructure.Services;

/// <summary>
/// Implementation of JWT token service.
/// Uses Microsoft.IdentityModel.Tokens for JWT handling.
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes the token service with configuration settings.
    /// </summary>
    /// <param name="configuration">Application configuration containing JWT settings.</param>
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT access token containing user claims.
    /// Includes user ID, email, and role in the token payload.
    /// </summary>
    /// <param name="user">The user entity to generate token for.</param>
    /// <returns>JWT token string.</returns>
    public string GenerateAccessToken(User user)
    {
        // Retrieve JWT settings from configuration
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "60");

        // Create signing key from secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create claims for the token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Subject (user ID)
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value), // User email
            new Claim(ClaimTypes.Role, user.Role.ToString()), // User role
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
        };

        // Create the JWT token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        // Return the serialized token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates a cryptographically secure refresh token.
    /// Used for obtaining new access tokens without re-authentication.
    /// </summary>
    /// <returns>A random refresh token string.</returns>
    public string GenerateRefreshToken()
    {
        // Generate 32 bytes of random data
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Convert to base64 string for storage/transmission
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Validates a JWT token and extracts the user ID from claims.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>User ID if token is valid, null otherwise.</returns>
    public string? ValidateTokenAndGetUserId(string token)
    {
        try
        {
            // Get JWT settings
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            // Create token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
            };

            // Validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

            // Extract user ID from token claims
            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);
            return userIdClaim?.Value;
        }
        catch
        {
            // Token validation failed
            return null;
        }
    }
}