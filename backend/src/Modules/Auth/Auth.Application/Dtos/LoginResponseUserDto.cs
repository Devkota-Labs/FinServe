using Shared.Application.Dtos;
using Users.Application.Dtos.Role;

namespace Auth.Application.Dtos;

public sealed record LoginResponseUserDto(int Id, string FullName, string Email, bool EmailVerified, bool MobileVerified, Uri? ProfileImageUrl, ICollection<string>? Roles, ICollection<MenuTreeDto>? Menus);
