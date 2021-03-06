namespace Linn.Purchasing.Service.Host
{
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;

    using Carter;

    using Linn.Common.Authentication.Host.Extensions;
    using Linn.Common.Configuration;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Host.Negotiators;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddCors();
            services.AddSingleton<IViewLoader, ViewLoader>();
            services.AddCredentialsExtensions();
            services.AddSqsExtensions();
            services.AddLog();

            services.AddBuilders();
            services.AddFacades();
            services.AddServices();
            services.AddPersistence();
            services.AddHandlers();
            services.AddMessagingServices();

            services.AddCarter();
            services.AddLinnAuthentication(
                options =>
                    {
                        options.Authority = ConfigurationManager.Configuration["AUTHORITY_URI"];
                        options.CallbackPath = new PathString("/purchasing/signin-oidc");
                        options.CookiePath = "/purchasing";
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles(new StaticFileOptions
                                       {
                                           RequestPath = "/purchasing/build",
                                           FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "client", "build"))
                                       });
            }
            else
            {
                app.UseStaticFiles(new StaticFileOptions
                                       {
                                           RequestPath = "/purchasing/build",
                                           FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "app", "client", "build"))
                                       });
            }

            app.UseAuthentication();

            app.UseBearerTokenAuthentication();

            app.UseRouting();
            app.UseEndpoints(cfg => cfg.MapCarter());
        }
    }
}
