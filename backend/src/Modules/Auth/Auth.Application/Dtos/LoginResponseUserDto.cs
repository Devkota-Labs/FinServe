using Shared.Application.Dtos;

namespace Auth.Application.Dtos;

public sealed record LoginResponseUserDto(int Id, string FullName, string Email, bool EmailVerified, bool MobileVerified, Uri? ProfileImageUrl, ICollection<MenuTreeDto>? Menus);
