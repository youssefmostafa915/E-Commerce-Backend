using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Application.UseCases.Orders.Create;

/// <summary>
/// Command to create an order from cart.
/// </summary>
public class CreateOrderCommand : IRequest<OrderResponse>
{
    /// <summary>
    /// The request data.
    /// </summary>
    public CreateOrderRequest Request { get; set; } = default!;

    /// <summary>
    /// User ID (set from claims).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Guest ID for anonymous orders.
    /// </summary>
    public string? GuestId { get; set; }
}