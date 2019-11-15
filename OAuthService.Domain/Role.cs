using OAuthService.Domain.Base;

namespace OAuthService.Domain
{
    public class Role: IEntity
    {
        public Role() { }

        public Role(string id, string name, string normalizedName)
            :this(name, normalizedName)
        {
            Id = id;
        }

        public Role(string name, string normalizedName)
        {
            Name = name;
            NormalizedName = normalizedName;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string Description { get; set; }
    }
}
