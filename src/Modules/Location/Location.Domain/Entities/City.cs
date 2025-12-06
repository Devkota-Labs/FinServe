using Shared.Domain;

namespace Location.Domain.Entities;
public sealed class City : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required int StateId { get; set; }
    public State State { get; set; } = null!;
}
