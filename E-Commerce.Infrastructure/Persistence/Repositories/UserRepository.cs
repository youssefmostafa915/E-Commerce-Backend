using E_Commerce.Application.Interfaces.Repositories; 
using E_Commerce.Domain.Entities;                  
using MongoDB.Driver;

namespace E_Commerce.Infrastructure.Persistence.Repositories;
public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    public UserRepository(MongoDbContext context)
    {
        _users = context.GetCollection<User>("Users");
    }
    public async Task<User?> GetByEmailAsync(string email) => 
        await _users.Find(u => u.Email.Value == email).FirstOrDefaultAsync();
    public async Task AddAsync(User user) => 
        await _users.InsertOneAsync(user);

    public async Task UpdateAsync(User user) =>
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
    

}