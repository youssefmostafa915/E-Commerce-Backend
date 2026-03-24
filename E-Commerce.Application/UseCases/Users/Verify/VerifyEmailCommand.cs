using MediatR;
using E_Commerce.Application.DTOs.Users;

// Logic: Removed .Verify to match your actual folder structure
namespace E_Commerce.Application.UseCases.Users;

public record VerifyEmailCommand(string Email, string Code) : IRequest<VerifyUserResponse>;