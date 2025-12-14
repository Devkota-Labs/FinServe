namespace Users.Application.Dtos.Menu;

public sealed record CreateMenuDto(string Name, int? ParentMenuId, string? Route, string? Icon, int Order);
