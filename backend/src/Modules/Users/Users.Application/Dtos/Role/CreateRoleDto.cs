namespace Users.Application.Dtos.Role;

public sealed record CreateRoleDto(string Name, string Description, bool IsActive);
