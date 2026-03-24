using MediatR;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using CartEntity = E_Commerce.Domain.Entities.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Handler for updating cart item quantities.
/// </summary>
public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes the update cart item handler.
    /// </summary>
    /// <param name="cartRepository">Repository for cart operations.</param>
    public UpdateCartItemHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the update cart item command.
    /// Updates quantity or removes item if quantity is 0.
    /// </summary>
    /// <param name="command">The update cart item command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cart response with updated cart information.</returns>
    public async Task<CartResponse> Handle(UpdateCartItemCommand command, CancellationToken cancellationToken)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(command.ProductId))
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "ProductId is required."
            };
        }

        if (command.Quantity < 0)
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Quantity cannot be negative."
            };
        }

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

        if (cart == null)
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Cart not found.",
                UserId = command.UserId,
                GuestId = command.GuestId
            };
        }

        // Find the item
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == command.ProductId);
        if (existingItem == null)
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Item not found in cart."
            };
        }

        if (command.Quantity == 0)
        {
            // Remove item
            cart.Items.Remove(existingItem);
        }
        else
        {
            // Update quantity
            var updatedItem = existingItem with { Quantity = command.Quantity };
            cart.Items.Remove(existingItem);
            cart.Items.Add(updatedItem);
        }

        // Save cart
        await _cartRepository.SaveAsync(cart);

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
            Message = command.Quantity == 0 ? "Item removed from cart." : "Cart item updated successfully.",
            CartId = cart.Id,
            UserId = cart.UserId,
            GuestId = cart.GuestId,
            Items = cartItems
        };
    }
}