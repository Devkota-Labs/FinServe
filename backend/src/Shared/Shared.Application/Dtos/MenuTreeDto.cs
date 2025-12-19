namespace Shared.Application.Dtos;

public sealed record MenuTreeDto(int Id, string Name, string Route, string Icon, int Order)
{
    public ICollection<MenuTreeDto> Children { get; private set; } = [];
}
