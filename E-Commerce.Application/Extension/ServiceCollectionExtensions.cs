using Microsoft.Extensions.DependencyInjection;
using E_Commerce.Application.UseCases.Products;
using E_Commerce.Application.UseCases.Users.Register;
using E_Commerce.Application.UseCases.Users.Login;
using E_Commerce.Application.UseCases.Cart;
using E_Commerce.Application.UseCases.Users;
using E_Commerce.Application.UseCases.Orders.Create;

namespace E_Commerce.Application.Extension;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<RegisterUserHandler>();

        // Register login handler for authentication
        services.AddScoped<LoginHandler>();

        // Register cart handler for cart operations
        services.AddScoped<AddToCartHandler>();
        services.AddScoped<GetCartHandler>();
        services.AddScoped<UpdateCartItemHandler>();
        services.AddScoped<ClearCartHandler>();

        // Register order handlers
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<GetUserOrdersHandler>();
        services.AddScoped<GetOrderByIdHandler>();
        services.AddScoped<UpdateOrderStatusHandler>();

        // This will now resolve because of the updated 'using' above
        services.AddScoped<VerifyEmailHandler>();

        return services;
    }
}