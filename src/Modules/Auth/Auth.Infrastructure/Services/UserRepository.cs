using Auth.Application.Interfaces;
using Auth.Infrastructure.Repositories;
using Serilog;
using Shared.Common.Services;

namespace Auth.Infrastructure.Services;

public sealed class UserRepository : BaseService, IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(ILogger logger, AppDbContext db)
        : base(logger.ForContext<UserRepository>(), null)
    {
            _db = db;
        }

        public async Task AddAsync(User user) => await _db.Users.AddAsync(user).ConfigureAwait(false);
        public async Task<User?> GetByEmailAsync(string email)
            => await _db.Users
            .Include(u => u.Country)
            .Include(u => u.State)
            .Include(u => u.City)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()).ConfigureAwait(false);
        public async Task<User?> GetByMobileAsync(string mobile)
            => await _db.Users
            .Include(u => u.Country)
            .Include(u => u.State)
            .Include(u => u.City)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Mobile == mobile).ConfigureAwait(false);
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users
                .Include(u => u.UserRoles)
                .Include(u => u.Country)
                .Include(u => u.State)
                .Include(u => u.City)
                .FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
            => await _db.Users
            .Include(u => u.Country)
            .Include(u => u.State)
            .Include(u => u.City)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync().ConfigureAwait(false);
        public async Task<IEnumerable<User>> GetPendingApprovalsAsync()
            => await _db.Users
            .Include(u => u.Country)
            .Include(u => u.State)
            .Include(u => u.City)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsApproved).ToListAsync().ConfigureAwait(false);
        public async Task SaveChangesAsync() => await _db.SaveChangesAsync().ConfigureAwait(false);
    }
