using Shared.Application.Interfaces.Services;
using Shared.Application.Results;
using Users.Application.Dtos.User;

namespace Users.Application.Interfaces.Services;

public interface IUserService : IService<UserDto, CreateUserDto, UpdateUserDto>
{
    Task<Result<UserProfileDto>> GetProfile(int id, CancellationToken cancellationToken = default);
    Task<Result<ICollection<AddressDto>>> GetAddressAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<AddressDto>> UpdateAddressAsync(int userId, int addressId, UpdateAddressDto dto, CancellationToken cancellationToken = default);
    Task<Result<AddressDto>> DeleteAddressAsync(int userId, int addressId, CancellationToken cancellationToken = default);
}
