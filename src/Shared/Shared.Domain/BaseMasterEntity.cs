using System.ComponentModel.DataAnnotations;

namespace Shared.Domain;

public abstract class BaseMasterEntity : BaseAuditableEntity, IMasterEntity
{
    [MaxLength(150)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public required string Description { get; set; }
    public required bool IsActive { get; set; } = true;
}
