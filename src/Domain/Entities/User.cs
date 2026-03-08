using System;
using Domain.Common.Ids;

namespace Domain.Entities
{
    public class User
    {
        public EntityId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RegisterCode { get; set; }
        public bool IsActive { get; set; }
        public EntityId RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
