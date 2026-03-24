namespace E_Commerce.Application.DTOs.Users;

public class VerifyUserResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}