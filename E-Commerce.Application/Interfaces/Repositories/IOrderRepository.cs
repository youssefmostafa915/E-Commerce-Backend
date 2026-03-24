using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for order operations.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gets an order by ID.
    /// </summary>
    Task<Order?> GetByIdAsync(string id);

    /// <summary>
    /// Gets orders by user ID.
    /// </summary>
    Task<List<Order>> GetByUserIdAsync(string userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Gets an order by order number.
    /// </summary>
    Task<Order?> GetByOrderNumberAsync(string orderNumber);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    Task<Order> AddAsync(Order order);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    Task UpdateAsync(Order order);

    /// <summary>
    /// Gets orders by status.
    /// </summary>
    Task<List<Order>> GetByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10);

    /// <summary>
    /// Gets total count of orders for a user.
    /// </summary>
    Task<int> GetTotalCountByUserIdAsync(string userId);

    /// <summary>
    /// Gets total count of orders by status.
    /// </summary>
    Task<int> GetTotalCountByStatusAsync(OrderStatus status);
}