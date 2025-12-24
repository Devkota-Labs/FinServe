using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Users.Domain.Entities;

public sealed class UserAddress : BaseAuditableEntity
{
    public int UserId { get; set; }
    public required AddressType AddressType { get; set; }
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public required int CountryId { get; set; }
    public required int StateId { get; set; }
    public required int CityId { get; set; }
    public required string PinCode { get; set; }
    public bool IsPrimary { get; set; }
    public void SetPrimary() => IsPrimary = true;
    public void UnsetPrimary() => IsPrimary = false;
}