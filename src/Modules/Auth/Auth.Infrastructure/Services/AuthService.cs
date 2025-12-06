using Auth.Application.Interfaces;
using Auth.Infrastructure;
using Auth.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Modules.Auth.Application.Dtos;
using Modules.Auth.Domain.Entities;
using Serilog;
using Shared.Common.Services;
using Shared.Security;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static FinServe.Api.Common.ApiRoutes;

namespace Auth.Infrastructure.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IUserRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;
    private readonly AuthDbContext _db;

    public AuthService(ILogger logger, IUserRepository repo, IPasswordHasher hasher, IJwtTokenGenerator jwt, AuthDbContext db)
    : base(logger.ForContext<AuthService>(), null)
    {
        _repo = repo;
        _hasher = hasher;
        _jwt = jwt;
        _db = db;
    }

    public async Task<UserDto?> RegisterAsync(string fullName, string email, string password)
    {
        var existing = await _repo.GetByEmailAsync(email);
        if (existing != null) return null;

        var hash = _hasher.Hash(password);
        var user = new User(fullName, email, hash);
        await _repo.AddAsync(user);

        // ensure default Customer role exists
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
        if (role == null)
        {
            role = new Role("Customer");
            _db.Roles.Add(role);
            await _db.SaveChangesAsync();
        }

        _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
        await _db.SaveChangesAsync();

        return new UserDto(user.Id, user.FullName, user.Email, new[] { "Customer" });
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null) return null;
        if (!_hasher.Verify(user.PasswordHash, password)) return null;
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        return _jwt.GenerateToken(user.Id, user.Email, roles);
    }
}