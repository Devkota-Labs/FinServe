using Auth.Application.Dtos.Menus;

namespace Auth.Application.Dtos.Auth;

public sealed record LoginResponseUserDto(int Id, string FullName, string Email, bool EmailVerified, bool MobileVerified, string? ProfileImageUrl, IList<string> Roles, List<MenuTreeDto>? Menus);
