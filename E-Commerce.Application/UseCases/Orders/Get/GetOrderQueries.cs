using MediatR;
using E_Commerce.Application.DTOs.Orders;

namespace E_Commerce.Application.UseCases.Orders.Get;

public class GetUserOrdersQuery : IRequest<List<OrderDto>>
{
    public string UserId { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public string OrderId { get; set; } = string.Empty;
}
