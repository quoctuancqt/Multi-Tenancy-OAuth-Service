using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OAuthService.Core.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthService.Core.Extensions
{
    public static class LogExtention
    {
        private const string CONTENT_TYPE = "application/json";

        public static IApplicationBuilder AddLog(this IApplicationBuilder app,
            IConfiguration configuration)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {

                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

                    var exception = context.Features.Get<IExceptionHandlerFeature>();

                    if (exception.Error is BadRequestException)
                    {

                        await BadRequest(context, exception);

                        return;
                    }

                    if (exception.Error is ForbiddenException)
                    {
                        context.Response.StatusCode = 403;

                        string errorMsg = string.IsNullOrEmpty(exception.Error.Message) ? "Forbidden" : exception.Error.Message;

                        await context.Response.WriteAsync(errorMsg).ConfigureAwait(false);

                        return;
                    }

                    //TODO add log

                    context.Response.StatusCode = 500;

                    context.Response.ContentType = "application/json";

                    var error = JsonConvert.SerializeObject(new
                    {
                        error = exception.Error.Message,
                        stackTrace = exception.Error.StackTrace,
                    });

                    await context.Response.WriteAsync(error).ConfigureAwait(false);

                    return;

                });
            });

            return app;
        }

        private static async Task BadRequest(HttpContext context, IExceptionHandlerFeature exception)
        {
            context.Response.StatusCode = 400;

            context.Response.ContentType = CONTENT_TYPE;

            var badRequestException = exception.Error as BadRequestException;

            if (badRequestException.Errors == null)
            {
                var error = JsonConvert.SerializeObject(new { error = exception.Error.Message });

                await context.Response.WriteAsync(error).ConfigureAwait(false);
            }
            else
            {
                var values = badRequestException.Errors.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key.FirstCharToLower(), d.Value));

                await context.Response.WriteAsync("{" + string.Join(",", values) + "}").ConfigureAwait(false);
            }
        }

        private static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return input.First().ToString().ToLower() + input.Substring(1);
        }
    }
}
