using Shared.Application.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByMobileAsync(string mobile);
}
