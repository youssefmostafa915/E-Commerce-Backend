using MediatR;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.UseCases.Orders.Update;

public class UpdateOrderStatusCommand : IRequest<OrderResponse>
{
    public string OrderId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
}
