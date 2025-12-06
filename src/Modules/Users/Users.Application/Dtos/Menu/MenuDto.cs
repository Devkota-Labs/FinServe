namespace Users.Application.Dtos.Menu;

public sealed record MenuDto(int MenuId, string Name, string? Route, string? Icon, int Order)
{
    public ICollection<MenuDto>? Children { get; }
}
