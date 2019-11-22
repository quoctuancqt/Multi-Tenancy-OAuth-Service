using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OAuthService.Core.Extensions;
using OAuthService.Core.Swagger;
using OAuthService.DistributedCache;
using OAuthService.Domain;
using OAuthService.JWT.Extensions;
using OAuthService.JWT.Proxy;
using OAuthService.TenantFactory;
using OAuthService.TenantFactory.Exntensions;
using Webcast.Core.Exntensions;

namespace OAuthService.WebApi
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers().AddNewtonsoftJson();

            services.AddJWTAddAuthentication(Configuration);

            services.AddHttpJWT<OAuthClient>(new Uri(Configuration.GetSection("JWTSettings").GetValue<string>("Issuer")));

            services.Configure<TenantConfig>(Configuration.GetSection("TenantConfig"));

            services.AddCors();

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddMongoContext();

            services.AddContext();

            services.AddOAuthProxy(Configuration);

            services.AddSwashbuckle(Configuration, (c) =>
            {
                c.AddSecurityDefinition("WebSecurity", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter web cast security",
                    Name = "WebSecurity",
                    Type = SecuritySchemeType.ApiKey
                });


                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Scheme = "WebSecurity"
                        },
                        Enumerable.Empty<string>().ToList()
                    }
                });

                return c;
            });

            services.AddSqlServerCache(Configuration);
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

            app.UseCors();

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
