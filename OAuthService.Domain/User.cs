using OAuthService.Core.TokenSerializer;
using OAuthService.Domain.Base;
using OAuthService.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAuthService.Domain
{
    public class User : EntityBase, IEntity, ITokenSecurity
    {
        public User()
        {
            CreatedDate = DateTime.Now;
        }

        public User(string id = null, string userName = null)
        {
            Id = id;
            UserName = userName;
            Email = userName;
            IsSystem = true;
            LockoutEnabled = false;
        }
        public long RefNo { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string DisplayName { get; set; }
        public string NormalizedDisplayName { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; } = true;
        public UserStatusEnum Status { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsSystem { get; set; }
        public bool IsFirstTimeLogin { get; set; } = false;
        public bool IsValidUser { get; set; } = true;
        [NotMapped]
        public string Key => Id;
    }
}
