using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Application.UseCases.Cart;

namespace E_Commerce.Api.Controllers;

/// <summary>
/// Controller for cart operations.
/// Handles adding items, viewing cart, and cart management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for cart operations
public class CartController : ControllerBase
{
    private readonly AddToCartHandler _addToCartHandler;
    private readonly GetCartHandler _getCartHandler;
    private readonly UpdateCartItemHandler _updateCartItemHandler;
    private readonly ClearCartHandler _clearCartHandler;

    /// <summary>
    /// Initializes the cart controller.
    /// </summary>
    /// <param name="addToCartHandler">Handler for adding items to cart.</param>
    /// <param name="getCartHandler">Handler for getting cart contents.</param>
    /// <param name="updateCartItemHandler">Handler for updating cart items.</param>
    /// <param name="clearCartHandler">Handler for clearing cart.</param>
    public CartController(
        AddToCartHandler addToCartHandler,
        GetCartHandler getCartHandler,
        UpdateCartItemHandler updateCartItemHandler,
        ClearCartHandler clearCartHandler)
    {
        _addToCartHandler = addToCartHandler;
        _getCartHandler = getCartHandler;
        _updateCartItemHandler = updateCartItemHandler;
        _clearCartHandler = clearCartHandler;
    }

    /// <summary>
    /// Adds an item to the cart.
    /// Supports both authenticated users and guest carts.
    /// </summary>
    /// <param name="request">The add to cart request.</param>
    /// <returns>Cart response with updated cart information.</returns>
    [HttpPost("add")]
    [AllowAnonymous] // Allow anonymous users to add to cart
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        // For authenticated users, get UserId from claims
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                request.UserId = userId;
            }
        }

        // If no UserId provided and user is not authenticated, require GuestId
        if (string.IsNullOrWhiteSpace(request.UserId) && string.IsNullOrWhiteSpace(request.GuestId))
        {
            return BadRequest(new CartResponse
            {
                IsSuccess = false,
                Message = "Either authentication or GuestId is required."
            });
        }

        var command = new AddToCartCommand(request);
        var result = await _addToCartHandler.Handle(command, default);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets the current user's cart or guest cart.
    /// </summary>
    /// <param name="guestId">Optional guest ID for anonymous users.</param>
    /// <returns>Cart response with cart contents.</returns>
    [HttpGet]
    [AllowAnonymous] // Allow anonymous users to view their cart
    public async Task<IActionResult> GetCart([FromQuery] string? guestId = null)
    {
        string? userId = null;

        // For authenticated users, get UserId from claims
        if (User.Identity?.IsAuthenticated == true)
        {
            userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        // If no UserId and no GuestId provided, require GuestId
        if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(guestId))
        {
            return BadRequest(new CartResponse
            {
                IsSuccess = false,
                Message = "Either authentication or GuestId is required."
            });
        }

        var command = new GetCartCommand { UserId = userId, GuestId = guestId };
        var result = await _getCartHandler.Handle(command, default);

        return Ok(result);
    }

    /// <summary>
    /// Updates the quantity of an item in the cart.
    /// Set quantity to 0 to remove the item.
    /// </summary>
    /// <param name="productId">The product ID to update.</param>
    /// <param name="request">The update request with new quantity.</param>
    /// <returns>Cart response with updated cart information.</returns>
    [HttpPut("items/{productId}")]
    [AllowAnonymous] // Allow anonymous users to update their cart
    public async Task<IActionResult> UpdateCartItem(string productId, [FromBody] UpdateCartItemRequest request)
    {
        string? userId = null;

        // For authenticated users, get UserId from claims
        if (User.Identity?.IsAuthenticated == true)
        {
            userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        // If no UserId and no GuestId provided, require GuestId
        if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(request.GuestId))
        {
            return BadRequest(new CartResponse
            {
                IsSuccess = false,
                Message = "Either authentication or GuestId is required."
            });
        }

        var command = new UpdateCartItemCommand
        {
            ProductId = productId,
            Quantity = request.Quantity,
            UserId = userId,
            GuestId = request.GuestId
        };

        var result = await _updateCartItemHandler.Handle(command, default);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Clears all items from the cart.
    /// </summary>
    /// <param name="guestId">Optional guest ID for anonymous users.</param>
    /// <returns>Cart response with empty cart.</returns>
    [HttpDelete]
    [AllowAnonymous] // Allow anonymous users to clear their cart
    public async Task<IActionResult> ClearCart([FromQuery] string? guestId = null)
    {
        string? userId = null;

        // For authenticated users, get UserId from claims
        if (User.Identity?.IsAuthenticated == true)
        {
            userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        // If no UserId and no GuestId provided, require GuestId
        if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(guestId))
        {
            return BadRequest(new CartResponse
            {
                IsSuccess = false,
                Message = "Either authentication or GuestId is required."
            });
        }

        var command = new ClearCartCommand { UserId = userId, GuestId = guestId };
        var result = await _clearCartHandler.Handle(command, default);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}

/// <summary>
/// Request DTO for updating cart item quantity.
/// </summary>
public class UpdateCartItemRequest
{
    /// <summary>
    /// The new quantity for the item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Guest ID for anonymous users.
    /// </summary>
    public string? GuestId { get; set; }
}