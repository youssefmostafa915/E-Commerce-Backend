using E_Commerce.Domain.Entities;
using System.ComponentModel.DataAnnotations;    

namespace E_Commerce.Application.DTOs.Users;
public class RegisterUserRequest    
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
