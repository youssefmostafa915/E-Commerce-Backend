using Microsoft.AspNetCore.Mvc;
using E_Commerce.Application.DTOs.Users; 
using E_Commerce.Application.UseCases.Users.Register;
using E_Commerce.Application.UseCases.Users.Login;
using E_Commerce.Application.UseCases.Users; // For VerifyEmailHandler and Command

namespace E_Commerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly RegisterUserHandler _registerHandler;
    private readonly LoginHandler _loginHandler;
    private readonly VerifyEmailHandler _verifyHandler;

    // Injecting all three handlers directly into the constructor
    public UserController(
        RegisterUserHandler registerHandler, 
        LoginHandler loginHandler,
        VerifyEmailHandler verifyHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
        _verifyHandler = verifyHandler;
      
    }

    // 1. Register Endpoint
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(request);
        var result = await _registerHandler.Handle(command, default);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    // 2. Login Endpoint
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request);
        var result = await _loginHandler.Handle(command, default);

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    // 3. Email Verification Endpoint
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        // Wrap DTO into the internal command
        var command = new VerifyEmailCommand(request.Email, request.Code);
        
        var result = await _verifyHandler.Handle(command, default);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

   
}