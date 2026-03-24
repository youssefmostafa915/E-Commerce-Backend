using E_Commerce.Domain.Entities;

namespace E_Commerce.Application.Interfaces.Repositories;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);

    //make verification work
    Task UpdateAsync(User user);
}