using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modules.Auth.Domain.Entities;

namespace Auth.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
