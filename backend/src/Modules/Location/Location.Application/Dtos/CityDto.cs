namespace Location.Application.Dtos;

public sealed record CityDto(int Id, string Name, int StateId, string? StateName, int CountryId, string? CountryName);
