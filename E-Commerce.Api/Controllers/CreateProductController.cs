using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Domain.Shared;
using E_Commerce.Domain.Enums;
using E_Commerce.Application.UseCases.Products;
using E_Commerce.Application.UseCases.Products.Create;

namespace E_Commerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// Require authentication for all product operations
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _handler;

    // The Constructor injects our 'Brain' (The Handler)
    public ProductsController(CreateProductHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> HandleOperation([FromBody] CreateProductCommand command)
    {
        // Logic: Pass the request to the Application Layer
        // The default CancellationToken is passed to satisfy the method signature
        var result = await _handler.HandleAsync(command, HttpContext.RequestAborted);
        
        // Logic: Map the Domain Response to an HTTP Response
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        // Return the specific status code (404, 400, 409, etc.) defined in the Domain
        return StatusCode((int)result.StatusCode, result);
    }
}