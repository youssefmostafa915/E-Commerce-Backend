using MediatR;
using E_Commerce.Application.DTOs.Cart;

namespace E_Commerce.Application.UseCases.Cart;

/// <summary>
/// Command for adding an item to the cart.
/// </summary>
public class AddToCartCommand : IRequest<CartResponse>
{
    /// <summary>
    /// The request containing cart item details.
    /// </summary>
    public AddToCartRequest Request { get; set; } = default!;

    /// <summary>
    /// Initializes a new add to cart command.
    /// </summary>
    /// <param name="request">The add to cart request.</param>
    public AddToCartCommand(AddToCartRequest request)
    {
        Request = request;
    }

    /// <summary>
    /// Parameterless constructor for serialization.
    /// </summary>
    public AddToCartCommand() { }
}