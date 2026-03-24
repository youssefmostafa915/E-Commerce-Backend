using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using MongoDB.Driver;

namespace E_Commerce.Infrastructure.Persistence.Repositories;

/// <summary>
/// MongoDB implementation of order repository.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    /// <summary>
    /// Initializes the order repository.
    /// </summary>
    public OrderRepository(MongoDbContext context)
    {
        _orders = context.Orders;
    }

    /// <summary>
    /// Gets an order by ID.
    /// </summary>
    public async Task<Order?> GetByIdAsync(string id)
    {
        return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets orders by user ID with pagination.
    /// </summary>
    public async Task<List<Order>> GetByUserIdAsync(string userId, int page = 1, int pageSize = 10)
    {
        return await _orders
            .Find(o => o.UserId == userId)
            .SortByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Gets an order by order number.
    /// </summary>
    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return await _orders.Find(o => o.OrderNumber == orderNumber).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    public async Task<Order> AddAsync(Order order)
    {
        await _orders.InsertOneAsync(order);
        return order;
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    public async Task UpdateAsync(Order order)
    {
        await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
    }

    /// <summary>
    /// Gets orders by status with pagination.
    /// </summary>
    public async Task<List<Order>> GetByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10)
    {
        return await _orders
            .Find(o => o.Status == status)
            .SortByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Gets total count of orders for a user.
    /// </summary>
    public async Task<int> GetTotalCountByUserIdAsync(string userId)
    {
        return (int)await _orders.CountDocumentsAsync(o => o.UserId == userId);
    }

    /// <summary>
    /// Gets total count of orders by status.
    /// </summary>
    public async Task<int> GetTotalCountByStatusAsync(OrderStatus status)
    {
        return (int)await _orders.CountDocumentsAsync(o => o.Status == status);
    }
}