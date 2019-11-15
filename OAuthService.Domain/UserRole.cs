using OAuthService.Domain.Base;

namespace OAuthService.Domain
{
    public class UserRole: EntityBase, IEntity, IAudit
    {
        public UserRole() { }

        public UserRole(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
