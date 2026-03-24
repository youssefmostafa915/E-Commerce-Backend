namespace E_Commerce.Application.DTOs.Cart;

/// <summary>
/// Request DTO for adding or updating an item in the cart.
/// </summary>
public class AddToCartRequest
{
    /// <summary>
    /// The product ID to add to cart.
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
    /// The quantity to add.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The product image URL.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Optional user ID (for authenticated users).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Optional guest ID (for anonymous users).
    /// </summary>
    public string? GuestId { get; set; }
}