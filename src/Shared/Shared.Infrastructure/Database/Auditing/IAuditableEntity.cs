namespace Shared.Infrastructure.Database.Auditing;

public interface IAuditableEntity
{
    DateTime CreatedTime { get; set; }
    string? CreatedBy { get; set; }
    DateTime LastUpdatedTime { get; set; }
    string? LastUpdatedBy { get; set; }
}
