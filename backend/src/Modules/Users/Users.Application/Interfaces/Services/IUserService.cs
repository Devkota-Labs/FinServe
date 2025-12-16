using Shared.Application.Interfaces;
using Shared.Application.Results;
using Users.Application.Dtos.User;

namespace Users.Application.Interfaces.Services;

public interface IUserService : IService<UserDto, CreateUserDto, UpdateUserDto>
{
    Task<Result<UserProfileDto?>> GetProfile(int id, CancellationToken cancellationToken = default); 
}
