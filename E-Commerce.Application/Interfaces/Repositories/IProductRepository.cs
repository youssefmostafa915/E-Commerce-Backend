using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id);
    Task<List<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(string id);
    Task<Product?> GetByProductNameAsync(string name); 
    }
