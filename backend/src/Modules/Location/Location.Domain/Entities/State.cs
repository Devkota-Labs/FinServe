using Shared.Domain.Entities;

namespace Location.Domain.Entities;

public sealed class State : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public ICollection<City>? Cities { get; }
}
