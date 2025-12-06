namespace Shared.Domain;

public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public DateTime CreatedTime { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime LastUpdatedTime { get; set; }
    public string? LastUpdatedBy { get; set; }
}
