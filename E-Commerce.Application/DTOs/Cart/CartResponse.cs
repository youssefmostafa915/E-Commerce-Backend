using E_Commerce.Domain.ValueObjects;

namespace E_Commerce.Application.DTOs.Cart;

/// <summary>
/// Response DTO containing cart information.
/// </summary>
public class CartResponse
{
    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message describing the result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The cart ID.
    /// </summary>
    public string? CartId { get; set; }

    /// <summary>
    /// The user ID (if authenticated).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The guest ID (if anonymous).
    /// </summary>
    public string? GuestId { get; set; }

    /// <summary>
    /// List of items in the cart.
    /// </summary>
    public List<CartItemDto> Items { get; set; } = new();

    /// <summary>
    /// Total number of items in the cart.
    /// </summary>
    public int TotalItems => Items.Sum(i => i.Quantity);

    /// <summary>
    /// Total price of all items in the cart.
    /// </summary>
    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
}

/// <summary>
/// DTO representing a cart item.
/// </summary>
public class CartItemDto
{
    /// <summary>
    /// The product ID.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// The product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The quantity in the cart.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The product image URL.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// The total price for this item (unit price * quantity).
    /// </summary>
    public decimal TotalPrice => UnitPrice * Quantity;
}