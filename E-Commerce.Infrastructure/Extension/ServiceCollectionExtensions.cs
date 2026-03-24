using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Infrastructure.Persistence.Repositories;
using E_Commerce.Infrastructure.Services;

namespace E_Commerce.Infrastructure.Extension;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Register token service for JWT authentication
        services.AddScoped<ITokenService, TokenService>();

        // Register email service for sending verification emails
        services.AddScoped<IEmailService, EmailService>();

        // Register payment service (mock implementation)
        services.AddScoped<IPaymentService, MockPaymentService>();

        return services;
    }
}