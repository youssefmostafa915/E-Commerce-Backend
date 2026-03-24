using MediatR;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Domain.Entities;
using CartEntity = E_Commerce.Domain.Entities.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Handler for getting a user's cart.
/// </summary>
public class GetCartHandler : IRequestHandler<GetCartCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes the get cart handler.
    /// </summary>
    /// <param name="cartRepository">Repository for cart operations.</param>
    public GetCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the get cart command.
    /// Retrieves cart for authenticated user or guest.
    /// </summary>
    /// <param name="command">The get cart command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cart response with cart information.</returns>
    public async Task<CartResponse> Handle(GetCartCommand command, CancellationToken cancellationToken)
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

        // Get cart
        CartEntity? cart;
        if (!string.IsNullOrWhiteSpace(command.UserId))
        {
            cart = await _cartRepository.GetByUserIdAsync(command.UserId);
        }
        else
        {
            cart = await _cartRepository.GetByGuestIdAsync(command.GuestId!);
        }

        if (cart == null || cart.Items.Count == 0)
        {
            return new CartResponse
            {
                IsSuccess = true,
                Message = "Cart is empty.",
                UserId = command.UserId,
                GuestId = command.GuestId,
                Items = new List<CartItemDto>()
            };
        }

        // Convert to DTOs
        var cartItems = cart.Items.Select(i => new CartItemDto
        {
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity,
            ImageUrl = i.ImageUrl
        }).ToList();

        return new CartResponse
        {
            IsSuccess = true,
            Message = "Cart retrieved successfully.",
            CartId = cart.Id,
            UserId = cart.UserId,
            GuestId = cart.GuestId,
            Items = cartItems
        };
    }
}