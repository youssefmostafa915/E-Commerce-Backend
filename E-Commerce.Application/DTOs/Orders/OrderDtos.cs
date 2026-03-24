using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.DTOs.Orders;

/// <summary>
/// Request DTO for creating an order from cart.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Payment method to use.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Shipping address.
    /// </summary>
    public Address ShippingAddress { get; set; } = default!;

    /// <summary>
    /// Optional order notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Guest ID for anonymous orders.
    /// </summary>
    public string? GuestId { get; set; }
}

/// <summary>
/// Response DTO for order operations.
/// </summary>
public class OrderResponse
{
    /// <summary>
    /// Whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Response message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Order ID.
    /// </summary>
    public string? OrderId { get; set; }

    /// <summary>
    /// Order number.
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// Order details.
    /// </summary>
    public OrderDto? Order { get; set; }
}

/// <summary>
/// Order data transfer object.
/// </summary>
public class OrderDto
{
    /// <summary>
    /// Order ID.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Order number.
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// User ID.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Order status.
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Order items.
    /// </summary>
    public List<OrderItemDto> Items { get; set; } = new();

    /// <summary>
    /// Total amount.
    /// </summary>
    public Money TotalAmount { get; set; } = default!;

    /// <summary>
    /// Payment method.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Shipping address.
    /// </summary>
    public Address ShippingAddress { get; set; } = default!;

    /// <summary>
    /// Order notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Order date.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Payment transaction ID.
    /// </summary>
    public string? PaymentTransactionId { get; set; }
}

/// <summary>
/// Order item data transfer object.
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Product ID.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Unit price.
    /// </summary>
    public Money UnitPrice { get; set; } = default!;

    /// <summary>
    /// Quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Total price.
    /// </summary>
    public Money TotalPrice { get; set; } = default!;

    /// <summary>
    /// Product image URL.
    /// </summary>
    public string? ImageUrl { get; set; }
}

/// <summary>
/// Request DTO for updating order status.
/// </summary>
public class UpdateOrderStatusRequest
{
    /// <summary>
    /// New order status.
    /// </summary>
    public OrderStatus Status { get; set; }
}