using System;
using System.Collections.Generic;
using Shared.Kernel;

namespace Auth.Domain.Entities
{
    public sealed class Role : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required bool IsActive { get; set; } = true;
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
