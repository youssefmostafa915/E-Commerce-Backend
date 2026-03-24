using E_Commerce.Application.Interfaces.Repositories; 
using E_Commerce.Domain.Entities;                  
using MongoDB.Driver;

namespace E_Commerce.Infrastructure.Persistence.Repositories;

    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(MongoDbContext context)
        {
            // Logic: Links the repository to the "Products" collection in MongoDB
            _collection = context.GetCollection<Product>("Products");
        }

        // Logic: Find by internal ID
        public async Task<Product?> GetByIdAsync(string id) => 
            await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

        // Logic: Get everything in the collection
        public async Task<List<Product>> GetAllAsync() => 
            await _collection.Find(_ => true).ToListAsync();

        // Logic: Save a new Product
        public async Task AddAsync(Product product) => 
            await _collection.InsertOneAsync(product);

        // Logic: Find the existing document by ID and swap it for the updated version
        public async Task UpdateAsync(Product product) => 
            await _collection.ReplaceOneAsync(p => p.Id == product.Id, product);

        // Logic: Permanent removal from the DB
        public async Task DeleteAsync(string id) => 
            await _collection.DeleteOneAsync(p => p.Id == id);

        // Logic: Used for the "Conflict" check in your CreateProductHandler logic
        public async Task<Product?> GetByProductNameAsync(string name) => 
            await _collection.Find(p => p.Name == name).FirstOrDefaultAsync();
    }
