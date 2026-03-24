using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using MongoDB.Driver;

namespace E_Commerce.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation of cart repository.
/// Handles cart persistence for both authenticated users and guests.
/// </summary>
public class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _carts;

    /// <summary>
    /// Initializes the cart repository with MongoDB context.
    /// </summary>
    /// <param name="context">The MongoDB context for database access.</param>
    public CartRepository(MongoDbContext context)
    {
        _carts = context.GetCollection<Cart>("Carts");
    }

    /// <summary>
    /// Retrieves a cart by user ID.
    /// </summary>
    /// <param name="userId">The user's ID.</param>
    /// <returns>The cart if found, null otherwise.</returns>
    public async Task<Cart?> GetByUserIdAsync(string userId)
    {
        return await _carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves a cart by guest ID.
    /// </summary>
    /// <param name="guestId">The guest identifier.</param>
    /// <returns>The cart if found, null otherwise.</returns>
    public async Task<Cart?> GetByGuestIdAsync(string guestId)
    {
        return await _carts.Find(c => c.GuestId == guestId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Saves or updates a cart in the database.
    /// Uses upsert operation to insert if not exists or update if exists.
    /// </summary>
    /// <param name="cart">The cart to save.</param>
    public async Task SaveAsync(Cart cart)
    {
        var filter = Builders<Cart>.Filter.Or(
            Builders<Cart>.Filter.Where(c => c.UserId == cart.UserId && !string.IsNullOrEmpty(cart.UserId)),
            Builders<Cart>.Filter.Where(c => c.GuestId == cart.GuestId && !string.IsNullOrEmpty(cart.GuestId))
        );

        await _carts.ReplaceOneAsync(filter, cart, new ReplaceOptions { IsUpsert = true });
    }

    /// <summary>
    /// Deletes a cart by user ID.
    /// </summary>
    /// <param name="userId">The user's ID whose cart to delete.</param>
    public async Task DeleteByUserIdAsync(string userId)
    {
        await _carts.DeleteOneAsync(c => c.UserId == userId);
    }

    /// <summary>
    /// Deletes a cart by guest ID.
    /// </summary>
    /// <param name="guestId">The guest ID whose cart to delete.</param>
    public async Task DeleteByGuestIdAsync(string guestId)
    {
        await _carts.DeleteOneAsync(c => c.GuestId == guestId);
    }

    /// <summary>
    /// Merges guest cart items into user cart when user logs in.
    /// Combines items and removes the guest cart.
    /// </summary>
    /// <param name="guestId">The guest cart ID.</param>
    /// <param name="userId">The user ID to merge into.</param>
    public async Task MergeGuestCartToUserCartAsync(string guestId, string userId)
    {
        // Get both carts
        var guestCart = await GetByGuestIdAsync(guestId);
        var userCart = await GetByUserIdAsync(userId);

        if (guestCart == null || guestCart.Items.Count == 0)
        {
            return; // Nothing to merge
        }

        if (userCart == null)
        {
            // Create user cart from guest cart
            userCart = new Cart
            {
                UserId = userId
            };
            // Add all guest items to the new user cart
            foreach (var item in guestCart.Items)
            {
                userCart.AddOrUpdateItem(item);
            }
        }
        else
        {
            // Merge items into existing user cart
            foreach (var guestItem in guestCart.Items)
            {
                userCart.AddOrUpdateItem(guestItem);
            }
        }

        // Save the merged user cart
        await SaveAsync(userCart);

        // Remove the guest cart
        await DeleteByGuestIdAsync(guestId);
    }

    /// <summary>
    /// Clears all items from a cart by updating the existing cart document.
    /// </summary>
    /// <param name="userId">The user ID whose cart to clear.</param>
    /// <param name="guestId">The guest ID whose cart to clear.</param>
    public async Task ClearCartAsync(string? userId, string? guestId)
    {
        var filter = Builders<Cart>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
        }
        else if (!string.IsNullOrWhiteSpace(guestId))
        {
            filter = Builders<Cart>.Filter.Eq(c => c.GuestId, guestId);
        }
        else
        {
            return; // No valid identifier provided
        }

        // Update operation to clear the Items array
        var update = Builders<Cart>.Update
            .Set(c => c.Items, new List<CartItem>())
            .Set(c => c.UpdatedAt, DateTime.UtcNow);

        // Use upsert to create empty cart if it doesn't exist
        var options = new UpdateOptions { IsUpsert = true };

        await _carts.UpdateOneAsync(filter, update, options);
    }
}