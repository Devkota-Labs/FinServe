namespace Auth.Application.Dtos.Countries;

public sealed record UpdateCountryDto(string? Name, string? IsoCode, string? MobileCode);
