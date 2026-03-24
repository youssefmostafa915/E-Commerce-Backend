namespace E_Commerce.Domain.Enums;

public enum OrderStatus
{
    Draft = 0,      // Checkout started but not finished
    Pending = 1,    // Order placed, waiting for payment/verification
    Paid = 2,       // Payment confirmed
    Processing = 3, // Being packed in the warehouse
    Shipped = 4,    // Handed over to the courier
    Delivered = 5,  // Received by the customer
    Cancelled = 6,  // Order aborted by user or system
    Refunded = 7    // Money returned to customer
}