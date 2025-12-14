namespace Users.Application.Dtos.Role;

public sealed record RoleDto(int Id, string Name, string Description, ICollection<string>? Menus);
