//using Microsoft.EntityFrameworkCore;
//using Shared.Domain.Enums;
//using Shared.Security;
//using System.Data;
//using Users.Domain.Entities;
//using Users.Infrastructure.Db;

//namespace Users.Infrastructure.Seed;

//internal sealed class AdminSeeder(UserDbContext _context, IPasswordHasher _passwordHasher) : IAdminSeeder
//{
//    public async Task SeedAsync(CancellationToken cancellationToken = default)
//    {
//        Console.WriteLine($"SeedAsync on {Thread.CurrentThread.Name}");

//        //await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(true);

//        var seedTime = new DateTime(2024, 1, 1);

//        Console.WriteLine($"SeedAsync 1 on {Thread.CurrentThread.Name}");

//        // ---------- ROLE ----------
//        var adminRole = await _context.Roles
//            .AsNoTracking()
//            .FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken).ConfigureAwait(false);

//        int adminRoleId;

//        if (adminRole == null)
//        {
//            var newRole = new Role { Id = 1, Name = "Admin", Description = "Platform administrator", IsActive = true };

//            _context.Roles.Add(newRole);
//            //await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

//            adminRoleId = newRole.Id;
//        }
//        else
//        {
//            adminRoleId = adminRole.Id;
//        }

//        // ---------- USER ----------
//        var adminUser = await _context.Users
//            .AsNoTracking()
//            .FirstOrDefaultAsync(u => u.Email == "admin@finserve.com", cancellationToken).ConfigureAwait(false);

//        int adminUserId;

//        if (adminUser == null)
//        {
//            var newUser = new User
//            {
//                UserName = "Admin",
//                Email = "admin@finserve.com",
//                Mobile = "9999999999",
//                Gender = Gender.PerferNotToSay,
//                DateOfBirth = new DateOnly(seedTime.Year, seedTime.Month, seedTime.Day),
//                FirstName = "System",
//                LastName = "Administrator",
//                CountryId = 1,
//                StateId = 1,
//                CityId = 1,
//                Address = "123 Admin St, Metropolis",
//                PinCode = "400001",
//                IsActive = true,
//                IsApproved = true,
//                EmailVerified = true,
//                MobileVerified = true,
//                // Hash Password
//                PasswordHash = _passwordHasher.Hash("Admin@FinServe123!"),// "AENIe4W4SbJh10PQDlXCrz7vyJmLQulPRIuFhXqE+p41Pf0DRGhLa+CDx6EkNjHfhg==",
//                PasswordLastChanged = seedTime,
//                CreatedTime = seedTime,
//                CreatedBy = "Admin"
//            };

//            _context.Users.Add(newUser);
//            //await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

//            adminUserId = newUser.Id;
//        }
//        else
//        {
//            adminUserId = adminUser.Id;
//        }

//        // ---------- USER ROLE ----------
//        var exists = await _context.UserRoles
//            .AsNoTracking()
//            .AnyAsync(ur => ur.UserId == adminUserId && ur.RoleId == adminRoleId, cancellationToken).ConfigureAwait(false);

//        if (!exists)
//        {
//            _context.UserRoles.Add(new UserRole
//            {
//                UserId = adminUserId,
//                RoleId = adminRoleId,
//            });

//            //await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
//        }

//        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

//        //await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
//    }
//}
