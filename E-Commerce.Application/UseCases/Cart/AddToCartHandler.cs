using MediatR;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.DTOs.Cart;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using CartEntity = E_Commerce.Domain.Entities.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Handler for adding items to cart.
/// Supports both authenticated users and guest carts.
/// </summary>
public class AddToCartHandler : IRequestHandler<AddToCartCommand, CartResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes the add to cart handler.
    /// </summary>
    /// <param name="cartRepository">Repository for cart operations.</param>
    /// <param name="productRepository">Repository for product data access.</param>
    public AddToCartHandler(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handles the add to cart command.
    /// Validates the product, gets or creates cart, adds item, and saves.
    /// </summary>
    /// <param name="command">The add to cart command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Cart response with updated cart information.</returns>
    public async Task<CartResponse> Handle(AddToCartCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Step 1: Validate input
        if (string.IsNullOrWhiteSpace(request.ProductId))
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Product ID is required."
            };
        }

        if (request.Quantity <= 0)
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Quantity must be greater than zero."
            };
        }

        // Must have either UserId or GuestId
        if (string.IsNullOrWhiteSpace(request.UserId) && string.IsNullOrWhiteSpace(request.GuestId))
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Either UserId or GuestId must be provided."
            };
        }

        // Step 2: Verify product exists and get current price
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            return new CartResponse
            {
                IsSuccess = false,
                Message = "Product not found."
            };
        }

        // Use current product price instead of request price
        var cartItem = new CartItem(
            ProductId: request.ProductId,
            ProductName: product.Name,
            UnitPrice: product.Price.Amount,
            Quantity: request.Quantity,
            ImageUrl: product.ImageUrls.FirstOrDefault() ?? ""
        );

        // Step 3: Get or create cart
        CartEntity cart;
        if (!string.IsNullOrWhiteSpace(request.UserId))
        {
            // Authenticated user cart
            cart = await _cartRepository.GetByUserIdAsync(request.UserId) ?? new CartEntity { UserId = request.UserId };
        }
        else
        {
            // Guest cart
            cart = await _cartRepository.GetByGuestIdAsync(request.GuestId!) ?? new CartEntity { GuestId = request.GuestId };
        }

        // Step 4: Add or update item in cart
        cart.AddOrUpdateItem(cartItem);

        // Step 5: Save cart
        await _cartRepository.SaveAsync(cart);

        // Step 6: Return success response
        return new CartResponse
        {
            IsSuccess = true,
            Message = "Item added to cart successfully.",
            CartId = cart.Id,
            UserId = cart.UserId,
            GuestId = cart.GuestId,
            Items = cart.Items.Select(i => new CartItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                ImageUrl = i.ImageUrl
            }).ToList()
        };
    }
}