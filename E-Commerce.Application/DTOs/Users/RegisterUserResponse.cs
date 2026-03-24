using E_Commerce.Domain.Entities; // FIX: Added .Domain

namespace E_Commerce.Application.DTOs.Users;
public class RegisterUserResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? UserId { get; set; }
}