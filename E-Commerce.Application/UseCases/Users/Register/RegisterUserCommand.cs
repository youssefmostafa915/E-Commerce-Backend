using MediatR;
using E_Commerce.Application.DTOs.Users;

namespace E_Commerce.Application.UseCases.Users.Register;

public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
public RegisterUserRequest Request { get; set; } = default!;
    
    public RegisterUserCommand(RegisterUserRequest request)
    {
        Request = request;
    }

    public RegisterUserCommand() { } 
}