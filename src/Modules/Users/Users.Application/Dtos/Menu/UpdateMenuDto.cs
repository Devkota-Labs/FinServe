namespace Users.Application.Dtos.Menu;

public sealed record UpdateMenuDto(string? Name, int? ParentMenuId, string? Route, string? Icon, int? Order);