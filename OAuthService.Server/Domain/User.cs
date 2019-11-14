using System;

namespace OAuthService.Server.Domain
{
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool IsSystem { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual DateTime? ModifiedDate { get; set; }
    }
}
