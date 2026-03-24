using MediatR;
using BCrypt.Net;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums; // Required for UserRole
using E_Commerce.Application.DTOs.Users;
using System.Text.RegularExpressions;

namespace E_Commerce.Application.UseCases.Users.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public RegisterUserHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var request = command.Request;

        // 1. Strong Password Logic
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMinimum8Chars = new Regex(@".{8,}");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]+");

        bool isStrong = hasMinimum8Chars.IsMatch(request.Password) && 
                        hasUpperChar.IsMatch(request.Password) && 
                        hasNumber.IsMatch(request.Password) && 
                        hasSymbols.IsMatch(request.Password);

        if (!isStrong)
        {
            return new RegisterUserResponse { IsSuccess = false, Message = "Password too weak!" };
        }

        // 2. Uniqueness Check
        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing != null) return new RegisterUserResponse { IsSuccess = false, Message = "Email taken." };

        // 3. Hashing
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 4. Mapping (Fixes CS0029 error by using UserRole.Customer)
        var user = new User
        {
            FullName = request.FullName,
            Email = new Email(request.Email), 
            PasswordHash = passwordHash,
            Role = UserRole.Customer, 
            Phone = new PhoneNumber("+20", request.PhoneNumber) 
        };

        // 5. Verification Logic (Fixes CS1061 error)
        var code = new Random().Next(100000, 999999).ToString();
        user.GenerateVerification(code); 

        // 6. Send Verification Email
        var emailSent = await _emailService.SendVerificationEmailAsync(request.Email, code);
        if (!emailSent)
        {
            // If email fails, we could either fail registration or continue
            // For now, we'll continue but could log that email failed
            // In production, consider queuing emails for retry
        }

        // 7. Persistence
        await _userRepository.AddAsync(user);

        return new RegisterUserResponse
        {
            IsSuccess = true,
            Message = "Registration successful! Please check your email for the code.",
            UserId = user.Id
        };
    }
}