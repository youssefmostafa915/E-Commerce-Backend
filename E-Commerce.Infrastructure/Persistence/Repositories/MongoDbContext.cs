using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Infrastructure.Persistence.Repositories;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        // Logic: Pulls the connection string from your appsettings.json
        var connectionString = configuration.GetConnectionString("MongoDb");
        var databaseName = configuration["DatabaseName"];

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    // Collections
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

    public IMongoCollection<T> GetCollection<T>(string name)
        => _database.GetCollection<T>(name);
}