using MediatR;
using E_Commerce.Application.DTOs.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Command for clearing all items from a cart.
/// </summary>
public class ClearCartCommand : IRequest<CartResponse>
{
    /// <summary>
    /// User ID (for authenticated users).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Guest ID (for anonymous users).
    /// </summary>
    public string? GuestId { get; set; }
}