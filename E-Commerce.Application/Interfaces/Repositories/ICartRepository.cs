using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;

namespace E_Commerce.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for cart operations.
/// Handles both authenticated user carts and guest carts.
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Gets a cart by user ID.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The user's cart if found, null otherwise.</returns>
    Task<Cart?> GetByUserIdAsync(string userId);

    /// <summary>
    /// Gets a cart by guest ID.
    /// </summary>
    /// <param name="guestId">The guest identifier.</param>
    /// <returns>The guest cart if found, null otherwise.</returns>
    Task<Cart?> GetByGuestIdAsync(string guestId);

    /// <summary>
    /// Adds or updates a cart in the database.
    /// </summary>
    /// <param name="cart">The cart to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveAsync(Cart cart);

    /// <summary>
    /// Deletes a cart by user ID.
    /// </summary>
    /// <param name="userId">The user's ID whose cart to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteByUserIdAsync(string userId);

    /// <summary>
    /// Deletes a cart by guest ID.
    /// </summary>
    /// <param name="guestId">The guest ID whose cart to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteByGuestIdAsync(string guestId);

    /// <summary>
    /// Merges a guest cart into a user cart when user logs in.
    /// </summary>
    /// <param name="guestId">The guest cart ID.</param>
    /// <param name="userId">The user ID to merge into.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task MergeGuestCartToUserCartAsync(string guestId, string userId);

    /// <summary>
    /// Clears all items from a cart.
    /// </summary>
    /// <param name="userId">The user ID whose cart to clear (optional).</param>
    /// <param name="guestId">The guest ID whose cart to clear (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearCartAsync(string? userId, string? guestId);
}