using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Infrastructure.Services;

/// <summary>
/// Mock payment service for demonstration purposes.
/// In production, this would integrate with Stripe, PayPal, etc.
/// </summary>
public class MockPaymentService : IPaymentService
{
    /// <summary>
    /// Processes a payment (mock implementation).
    /// </summary>
    public async Task<PaymentResult> ProcessPaymentAsync(string orderId, Money amount, PaymentMethod paymentMethod)
    {
        // Simulate payment processing delay
        await Task.Delay(1000);

        // Mock payment logic - always succeeds for demo
        // In real implementation, this would call payment provider APIs
        var transactionId = $"TXN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        return new PaymentResult
        {
            IsSuccess = true,
            TransactionId = transactionId,
            Message = $"Payment of {amount.Amount} {amount.Currency} processed successfully via {paymentMethod}."
        };
    }
}