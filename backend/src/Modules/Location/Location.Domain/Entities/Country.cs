using Shared.Domain.Entities;

namespace Location.Domain.Entities;

public sealed class Country : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string IsoCode { get; set; }
    public required string MobileCode { get; set; }

    public ICollection<State>? States { get; }
}
