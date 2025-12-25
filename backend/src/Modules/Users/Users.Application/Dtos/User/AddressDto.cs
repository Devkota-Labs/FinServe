using Shared.Domain.Enums;

namespace Users.Application.Dtos.User;

public sealed record AddressDto(int Id, AddressType AddressType, string AddressLine1, string? AddressLine2, int CountryId, string Country, int StateId, string State, int CityId, string City, string PinCode, bool IsPrimary);