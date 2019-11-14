using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace OAuthService.Core.Helpers
{
    public static class ClaimKeyHelper
    {
        public const string FIRST_NAME = "first_name";
        public const string USER_ID = ClaimTypes.NameIdentifier;
        public const string EMAIL = ClaimTypes.Email;
        public const string NAME = ClaimTypes.Name;
        public const string ROLE = ClaimTypes.Role;
        public const string SYSTEM = "is_system";
        public const string TENANT_ID = "tenant_id";
        public const string TENANT_KEY = "tenant_key";

        public static string GetValue(this IPrincipal user, string key)
        {
            if (user == null)
            {
                return string.Empty;
            }

            var claim = ((ClaimsIdentity)user.Identity).Claims.FirstOrDefault(x => x.Type.Equals(key));

            if (claim == null)
            {
                return string.Empty;
            }

            return claim.Value;
        }

        public static bool WithRole(this IPrincipal user, string role)
        {
            var claim = ((ClaimsIdentity)user?.Identity)?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimKeyHelper.ROLE));

            return claim != null && claim.Value.Equals(role);
        }
    }
}
