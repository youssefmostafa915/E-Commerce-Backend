using MediatR;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Interfaces.Repositories;

namespace E_Commerce.Application.UseCases.Orders.Get;

public class GetUserOrdersHandler : IRequestHandler<GetUserOrdersQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetUserOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(request.UserId, request.Page, request.PageSize);
        return orders.Select(MapToOrderDto).ToList();
    }

    private OrderDto MapToOrderDto(E_Commerce.Domain.Entities.Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Status = order.Status,
            Items = order.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
                ImageUrl = item.ImageUrl
            }).ToList(),
            TotalAmount = order.TotalAmount,
            PaymentMethod = order.PaymentMethod,
            ShippingAddress = order.ShippingAddress,
            Notes = order.Notes,
            OrderDate = order.OrderDate,
            PaymentTransactionId = order.PaymentTransactionId
        };
    }
}

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Status = order.Status,
            Items = order.Items.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
                ImageUrl = item.ImageUrl
            }).ToList(),
            TotalAmount = order.TotalAmount,
            PaymentMethod = order.PaymentMethod,
            ShippingAddress = order.ShippingAddress,
            Notes = order.Notes,
            OrderDate = order.OrderDate,
            PaymentTransactionId = order.PaymentTransactionId
        };
    }
}
