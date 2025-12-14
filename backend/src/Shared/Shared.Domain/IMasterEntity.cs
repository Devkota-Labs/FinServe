using System.ComponentModel.DataAnnotations;

namespace Shared.Domain;

public interface IMasterEntity : IBaseEntity, IAuditableEntity
{
    [MaxLength(150)]
    string Name { get; set; }
    [MaxLength(500)]
    string Description { get; set; }
    bool IsActive { get; set; }
}
