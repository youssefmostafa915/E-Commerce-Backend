using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.UseCases.Orders.Create;
using E_Commerce.Application.UseCases.Orders.Get;
using E_Commerce.Application.UseCases.Orders.Update;
using E_Commerce.Domain.Shared;

namespace E_Commerce.Api.Controllers;

/// <summary>
/// Controller for order operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for order operations
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates an order from the current user's cart.
    /// </summary>
    /// <param name="request">Order creation request.</param>
    /// <returns>Order creation result.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new OrderResponse
            {
                IsSuccess = false,
                Message = "User authentication required."
            });
        }

        var command = new CreateOrderCommand
        {
            Request = request,
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets orders for the current user.
    /// </summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Items per page (default: 10).</param>
    /// <returns>List of user orders.</returns>
    [HttpGet]
    public async Task<IActionResult> GetUserOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { IsSuccess = false, Message = "User authentication required." });
        }

        var query = new GetUserOrdersQuery
        {
            UserId = userId,
            Page = page,
            PageSize = pageSize
        };

        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(string id)
    {
        var query = new GetOrderByIdQuery { OrderId = id };
        var order = await _mediator.Send(query);

        if (order == null)
        {
            return NotFound(new { IsSuccess = false, Message = "Order not found." });
        }

        return Ok(order);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")] // Admin only
    public async Task<IActionResult> UpdateOrderStatus(string id, [FromBody] UpdateOrderStatusRequest request)
    {
        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            Status = request.Status
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
