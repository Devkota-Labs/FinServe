namespace Users.Application.Dtos.Role;

public sealed record UpdateRoleDto(string? Name, string? Description, bool? IsActive);
