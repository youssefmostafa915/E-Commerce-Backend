using MediatR;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.DTOs.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Handler for clearing all items from a cart.
/// </summary>
public class ClearCartHandler : IRequestHandler<ClearCartCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes the clear cart handler.
    /// </summary>
    /// <param name="cartRepository">Repository for cart operations.</param>
    public ClearCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the clear cart command.
    /// Removes all items from the cart.
    /// </summary>
    /// <param name="command">The clear cart command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cart response with empty cart.</returns>
    public async Task<CartResponse> Handle(ClearCartCommand command, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(command.UserId) && string.IsNullOrWhiteSpace(command.GuestId))
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Either UserId or GuestId must be provided."
            };
        }

        // Clear cart
        await _cartRepository.ClearCartAsync(command.UserId, command.GuestId);

        return new CartResponse
        {
            IsSuccess = true,
            Message = "Cart cleared successfully.",
            UserId = command.UserId,
            GuestId = command.GuestId,
            Items = new List<CartItemDto>()
        };
    }
}