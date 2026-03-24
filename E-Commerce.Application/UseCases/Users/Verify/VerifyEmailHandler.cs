// Logic: Since there is no subfolder, the namespace ends at .Users
namespace E_Commerce.Application.UseCases.Users; 

using MediatR;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.DTOs.Users;
using E_Commerce.Domain.Entities;

public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, VerifyUserResponse>
{
    private readonly IUserRepository _userRepository;

    public VerifyEmailHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<VerifyUserResponse> Handle(VerifyEmailCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null) 
            return new VerifyUserResponse { IsSuccess = false, Message = "User not found." };

        try
        {
            user.ConfirmEmail(request.Code);
            await _userRepository.UpdateAsync(user);
            
            return new VerifyUserResponse { IsSuccess = true, Message = "Email verified successfully!" };
        }
        catch (InvalidOperationException ex)
        {
            return new VerifyUserResponse { IsSuccess = false, Message = ex.Message };
        }
    }
}