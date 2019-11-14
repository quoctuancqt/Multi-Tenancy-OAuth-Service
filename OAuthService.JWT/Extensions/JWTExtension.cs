using OAuthService.JWT.Helper;
using OAuthService.JWT.Middleware;
using OAuthService.JWT.Models;
using OAuthService.JWT.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace OAuthService.JWT.Extensions
{
    public static class JWTExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="path"></param>
        /// <param name="verifyClient"></param>
        /// <param name="expiration">1440 == 1day</param>
        /// <returns></returns>
        public static IApplicationBuilder UseJWTBearerToken(this IApplicationBuilder app,
            IConfiguration configuration,
            string path = "/token",
            bool verifyClient = false,
            int expiration = 1440)
        {
            string secretKey = string.IsNullOrEmpty(configuration.GetValue<string>("JWTSettings:SecurityKey"))
                ? SettingHelper.SecurityKey : configuration.GetValue<string>("JWTSettings:SecurityKey");

            app.UseMiddleware<TokenProviderMiddleware>(new TokenProviderOptions
            {
                Path = path,
                Audience = configuration.GetValue<string>("JWTSettings:Audience"),
                Issuer = configuration.GetValue<string>("JWTSettings:Issuer"),
                SecurityKey = secretKey,
                VerifyClient = verifyClient,
                Expiration = TimeSpan.FromMinutes(+expiration)
            });

            return app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="defaultScheme"></param>
        /// <returns></returns>
        public static IServiceCollection AddJWTAddAuthentication(this IServiceCollection services,
            IConfiguration configuration,
            string defaultScheme = JwtBearerDefaults.AuthenticationScheme)
        {
            string secretKey = string.IsNullOrEmpty(configuration.GetValue<string>("JWTSettings:SecurityKey"))
                ? SettingHelper.SecurityKey : configuration.GetValue<string>("JWTSettings:SecurityKey");

            services.AddAuthentication(defaultScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("JWTSettings:Issuer"),
                    ValidAudience = configuration.GetValue<string>("JWTSettings:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
                };
            });

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAccountManager"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAccountManager<TAccountManager>(this IServiceCollection services)
            where TAccountManager : class, IAccountManager
        {
            services.AddScoped<IAccountManager, TAccountManager>();

            return services;
        }
    }
}
