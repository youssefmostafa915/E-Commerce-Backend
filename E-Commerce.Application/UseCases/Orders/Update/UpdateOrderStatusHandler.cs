using MediatR;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.UseCases.Orders.Update;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            return new OrderResponse { IsSuccess = false, Message = "Order not found." };
        }

        try
        {
            order.UpdateStatus(request.Status);
            await _orderRepository.UpdateAsync(order);
            return new OrderResponse { IsSuccess = true, Message = "Order status updated successfully." };
        }
        catch (Exception ex)
        {
            return new OrderResponse { IsSuccess = false, Message = $"Error updating status: {ex.Message}" };
        }
    }
}
