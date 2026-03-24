using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Entities;

/// <summary>
/// Represents an order in the e-commerce system.
/// Contains order details, items, payment, and shipping information.
/// </summary>
public class Order : BaseEntity
{
    /// <summary>
    /// Unique order number for display purposes.
    /// </summary>
    public string OrderNumber { get; private set; } = string.Empty;

    /// <summary>
    /// The user who placed the order.
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// The order status.
    /// </summary>
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    /// <summary>
    /// Items in the order.
    /// </summary>
    public List<OrderItem> Items { get; private set; } = new();

    /// <summary>
    /// Total amount of the order.
    /// </summary>
    public Money TotalAmount { get; private set; } = default!;

    /// <summary>
    /// Payment method used.
    /// </summary>
    public PaymentMethod PaymentMethod { get; private set; }

    /// <summary>
    /// Shipping address.
    /// </summary>
    public Address ShippingAddress { get; private set; } = default!;

    /// <summary>
    /// Order notes.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// When the order was placed.
    /// </summary>
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Payment transaction ID.
    /// </summary>
    public string? PaymentTransactionId { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private Order() { }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    public Order(string userId, List<OrderItem> items, Money totalAmount,
                 PaymentMethod paymentMethod, Address shippingAddress, string? notes = null)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        Items = items ?? throw new ArgumentNullException(nameof(items));
        TotalAmount = totalAmount;
        PaymentMethod = paymentMethod;
        ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        Notes = notes;

        // Generate order number
        OrderNumber = GenerateOrderNumber();

        // Validate order
        ValidateOrder();
    }

    /// <summary>
    /// Generates a unique order number.
    /// </summary>
    private string GenerateOrderNumber()
    {
        // Format: ORD-YYYYMMDD-XXXXX
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Id.ToString().Substring(0, 5).ToUpper()}";
    }

    /// <summary>
    /// Validates the order data.
    /// </summary>
    private void ValidateOrder()
    {
        if (Items.Count == 0)
            throw new InvalidOperationException("Order must contain at least one item.");

        if (TotalAmount.Amount <= 0)
            throw new InvalidOperationException("Order total must be greater than zero.");

        if (string.IsNullOrWhiteSpace(UserId))
            throw new InvalidOperationException("User ID is required.");
    }

    /// <summary>
    /// Updates the order status.
    /// </summary>
    public void UpdateStatus(OrderStatus newStatus)
    {
        // Validate status transition
        if (!IsValidStatusTransition(Status, newStatus))
            throw new InvalidOperationException($"Invalid status transition from {Status} to {newStatus}.");

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates if a status transition is allowed.
    /// </summary>
    private bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
    {
        return (current, next) switch
        {
            (OrderStatus.Pending, OrderStatus.Paid) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            (OrderStatus.Paid, OrderStatus.Processing) => true,
            (OrderStatus.Paid, OrderStatus.Cancelled) => true,
            (OrderStatus.Processing, OrderStatus.Shipped) => true,
            (OrderStatus.Processing, OrderStatus.Cancelled) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            _ => false
        };
    }

    /// <summary>
    /// Sets the payment transaction ID.
    /// </summary>
    public void SetPaymentTransactionId(string transactionId)
    {
        PaymentTransactionId = transactionId ?? throw new ArgumentNullException(nameof(transactionId));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount from items.
    /// </summary>
    public void RecalculateTotal()
    {
        var total = Items.Sum(item => item.TotalPrice.Amount);
        TotalAmount = new Money(total, TotalAmount.Currency);
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Represents an item in an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// The product ID.
    /// </summary>
    public string ProductId { get; private set; } = string.Empty;

    /// <summary>
    /// The product name at time of order.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// The unit price.
    /// </summary>
    public Money UnitPrice { get; private set; } = default!;

    /// <summary>
    /// The quantity ordered.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// The total price for this item.
    /// </summary>
    public Money TotalPrice { get; private set; } = default!;

    /// <summary>
    /// Product image URL.
    /// </summary>
    public string? ImageUrl { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private OrderItem() { }

    /// <summary>
    /// Creates a new order item.
    /// </summary>
    public OrderItem(string productId, string productName, Money unitPrice,
                     int quantity, string? imageUrl = null)
    {
        ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        UnitPrice = unitPrice;
        Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        ImageUrl = imageUrl;

        TotalPrice = new Money(unitPrice.Amount * quantity, unitPrice.Currency);
    }
}