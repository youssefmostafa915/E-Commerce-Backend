using MediatR;
using BCrypt.Net;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Application.DTOs.Users;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.UseCases.Users.Login;

/// <summary>
/// Handler for user login.
/// Validates credentials, generates tokens, and returns authentication response.
/// </summary>
public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Initializes the login handler with required dependencies.
    /// </summary>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="tokenService">Service for JWT token operations.</param>
    public LoginHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Handles the login command.
    /// Validates user credentials and generates authentication tokens.
    /// </summary>
    /// <param name="command">The login command containing user credentials.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>Login response with tokens or error message.</returns>
    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Step 1: Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password."
            };
        }

        // Step 2: Verify email is confirmed (temporarily disabled for testing)
        // if (!user.IsEmailVerified)
        // {
        //     return new LoginResponse
        //     {
        //         IsSuccess = false,
        //         Message = "Please verify your email before logging in."
        //     };
        // }

        // Step 3: Verify password using BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password."
            };
        }

        // Step 4: Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Step 5: Calculate token expiration (matches TokenService settings)
        var expiresAt = DateTime.UtcNow.AddMinutes(60); // Should match JwtSettings:ExpiryInMinutes

        // Step 6: Return successful login response
        return new LoginResponse
        {
            IsSuccess = true,
            Message = "Login successful.",
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }
}