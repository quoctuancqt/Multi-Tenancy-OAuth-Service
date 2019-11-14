using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAuthService.Core.Swagger
{
    public static class SwashbuckleExtension
    {
        public static IServiceCollection AddSwashbuckle(this IServiceCollection services,
            IConfiguration Configuration, Func<SwaggerGenOptions, SwaggerGenOptions> custom = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "APIs Services",
                        Description = "Web API",
                        Version = "v1"
                    }
                 );

                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Description = "Please enter JWT with Bearer into field",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Bearer Authorization",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Scheme = "Bearer"
                        },
                        Enumerable.Empty<string>().ToList()
                    }
                });

                if (custom != null)
                {
                    c = custom.Invoke(c);
                }

            });

            return services;
        }

        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app, IConfiguration Configuration)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document API V1");
            });

            return app;
        }
    }
}

