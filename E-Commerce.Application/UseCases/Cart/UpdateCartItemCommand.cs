using MediatR;
using E_Commerce.Application.DTOs.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Command for updating the quantity of an item in the cart.
/// </summary>
public class UpdateCartItemCommand : IRequest<CartResponse>
{
    /// <summary>
    /// The product ID to update.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// The new quantity (0 to remove the item).
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// User ID (for authenticated users).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Guest ID (for anonymous users).
    /// </summary>
    public string? GuestId { get; set; }
}