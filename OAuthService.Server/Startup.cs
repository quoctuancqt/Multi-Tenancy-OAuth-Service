using System;
using System.Linq;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OAuthService.Core.Extensions;
using OAuthService.Core.ModelBinders;
using OAuthService.Core.Swagger;
using OAuthService.DistributedCache;
using OAuthService.JWT.Extensions;
using OAuthService.JWT.Proxy;
using OAuthService.JWT.Services;
using OAuthService.Server.Domain;
using OAuthService.Server.Extensions;
using OAuthService.Server.Services;

namespace OAuthService.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddScoped((service) =>
            {
                var builder = new DbContextOptionsBuilder<OAuthContext>();

                var connectionString = Configuration.GetConnectionString("OAuthConnectionString");

                builder.UseSqlServer(connectionString);

                return new OAuthContext(builder.Options);
            });

            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<IJWTClientService, ClientService>();

            services.AddSingleton<ITokenService, TokenService>();

            services.AddAccountManager<AccountService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddSwashbuckle(Configuration);

            services.Configure<TenantConfig>(Configuration.GetSection("TenantConfig"));

            services.AddJWTAddAuthentication(Configuration);

            services.AddHttpJWT<OAuthClient>(new Uri(Configuration.GetSection("JWTSettings").GetValue<string>("Issuer")));

            services.AddProxy(Configuration);

            services.AddSqlServerCache(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<SeedSchemes>();

            services.AddControllers(options => options.RegisterDateTimeProvider(services))
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.AddLog(Configuration);

            app.UseSwashbuckle(Configuration);

            app.UseJWTBearerToken(Configuration, verifyClient: true);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
