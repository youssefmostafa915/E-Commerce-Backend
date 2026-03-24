using MediatR;
using E_Commerce.Application.DTOs.Users;

namespace E_Commerce.Application.UseCases.Users;

/// <summary>
/// Command for user login.
/// Wraps the login request and implements MediatR IRequest.
/// </summary>
public class LoginCommand : IRequest<LoginResponse>
{
    /// <summary>
    /// The login request containing email and password.
    /// </summary>
    public LoginRequest Request { get; set; } = default!;

    /// <summary>
    /// Initializes a new login command.
    /// </summary>
    /// <param name="request">The login request DTO.</param>
    public LoginCommand(LoginRequest request)
    {
        Request = request;
    }

    /// <summary>
    /// Parameterless constructor for serialization.
    /// </summary>
    public LoginCommand() { }
}