using Shared.Application.Interfaces.Entities;

namespace Shared.Domain.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
}
