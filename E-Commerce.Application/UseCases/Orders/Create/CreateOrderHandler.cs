using MediatR;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Exceptions;

namespace E_Commerce.Application.UseCases.Orders.Create;

/// <summary>
/// Handler for creating orders from cart.
/// </summary>
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentService _paymentService;

    public CreateOrderHandler(
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IPaymentService paymentService)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentService = paymentService;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Get the cart
            var cart = await _cartRepository.GetCartAsync(command.UserId, command.GuestId);
            if (cart == null || cart.Items.Count == 0)
            {
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = "Cart is empty or not found."
                };
            }

            // 2. Validate stock availability
            foreach (var cartItem in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product == null)
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = $"Product {cartItem.ProductName} not found."
                    };
                }

                if (product.StockQuantity < cartItem.Quantity)
                {
                    return new OrderResponse
                    {
                        IsSuccess = false,
                        Message = $"Insufficient stock for {product.Name}. Available: {product.StockQuantity}, Requested: {cartItem.Quantity}"
                    };
                }
            }

            // 3. Convert cart items to order items
            var orderItems = cart.Items.Select(cartItem => new OrderItem(
                cartItem.ProductId,
                cartItem.ProductName,
                new Money(cartItem.UnitPrice, "USD"),
                cartItem.Quantity,
                cartItem.ImageUrl
            )).ToList();

            // 4. Calculate total
            var totalAmount = new Money(
                orderItems.Sum(item => item.TotalPrice.Amount),
                orderItems.First().UnitPrice.Currency
            );

            // 5. Create the order
            var order = new Order(
                command.UserId!,
                orderItems,
                totalAmount,
                command.Request.PaymentMethod,
                command.Request.ShippingAddress,
                command.Request.Notes
            );

            // 6. Process payment (mock implementation)
            var paymentResult = await _paymentService.ProcessPaymentAsync(
                order.Id,
                totalAmount,
                command.Request.PaymentMethod
            );

            if (!paymentResult.IsSuccess)
            {
                return new OrderResponse
                {
                    IsSuccess = false,
                    Message = $"Payment failed: {paymentResult.Message}"
                };
            }

            // 7. Set payment transaction ID
            order.SetPaymentTransactionId(paymentResult.TransactionId!);

            // 8. Save the order
            await _orderRepository.AddAsync(order);

            // 9. Update product inventory
            foreach (var orderItem in orderItems)
            {
                var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
                if (product != null)
                {
                    product.ReduceStock(orderItem.Quantity);
                    await _productRepository.UpdateAsync(product);
                }
            }

            // 10. Clear the cart
            await _cartRepository.ClearCartAsync(command.UserId, command.GuestId);

            // 11. Return success response
            return new OrderResponse
            {
                IsSuccess = true,
                Message = "Order created successfully.",
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Order = MapToOrderDto(order)
            };
        }
        catch (Exception ex)
        {
            return new OrderResponse
            {
                IsSuccess = false,
                Message = $"Failed to create order: {ex.Message}"
            };
        }
    }

    private OrderDto MapToOrderDto(Order order)
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