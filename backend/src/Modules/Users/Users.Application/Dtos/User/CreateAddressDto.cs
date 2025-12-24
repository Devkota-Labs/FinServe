using Shared.Domain.Enums;

namespace Users.Application.Dtos.User;

public sealed record CreateAddressDto(AddressType AddressType, int CountryId, int StateId, int CityId, string AddressLine1, string? AddressLine2, string PinCode, bool IsPrimary);
