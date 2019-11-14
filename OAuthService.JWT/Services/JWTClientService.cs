using OAuthService.JWT.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace OAuthService.JWT.Services
{
    public abstract class JWTClientService<TClient> : IJWTClientService
        where TClient : class, IClient
    {
        protected abstract DbSet<TClient> _clients { get; set; }

        public async Task<ClientBase> FindByClientIdAsync(string clientId)
        {
            var client = await _clients.FirstOrDefaultAsync(x => x.ClientId.Equals(clientId));

            return client as ClientBase;
        }

        public async Task<string> VerifyClientByClientIdAsync(string secretKey)
        {
            try
            {
                var credentials = Base64Decode(secretKey).Split(':');

                string clientId = credentials[0];

                string secret = credentials[1];

                var result = await _clients.FirstOrDefaultAsync(x => x.ClientId.Equals(clientId) && x.Secret.Equals(secret));

                return result.Id;
            }
            catch
            {
                throw new Exception($"secretKey invalid: {secretKey}");
            }

        }

        public async Task<string> GetSecretKeyByClientId(string clientId)
        {
            var client = await _clients.FirstOrDefaultAsync(x => x.ClientId.Equals(clientId));

            if (client == null) throw new Exception($"Not found clientId {clientId}");

            return Base64Encode($"{clientId}:{client.Secret}");
        }

        protected virtual string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        protected virtual string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
