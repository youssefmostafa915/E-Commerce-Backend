using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.Interfaces.Services;

/// <summary>
/// Payment service interface for processing payments.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment for an order.
    /// </summary>
    /// <param name="orderId">The order ID.</param>
    /// <param name="amount">The payment amount.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <returns>Payment result.</returns>
    Task<PaymentResult> ProcessPaymentAsync(string orderId, Money amount, PaymentMethod paymentMethod);
}

/// <summary>
/// Result of a payment operation.
/// </summary>
public class PaymentResult
{
    /// <summary>
    /// Whether the payment was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Payment transaction ID.
    /// </summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// Result message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}