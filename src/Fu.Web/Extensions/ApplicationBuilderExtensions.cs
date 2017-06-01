using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Net.Http.Headers;
using Fu.Infrastructure;
using Fu.Module.Core.Extensions;
using Microsoft.AspNetCore.Authentication.ActiveDirectory;
using System.Linq;
using System.Reflection;
using Fu.Infrastructure.Web;
using Microsoft.AspNetCore.Routing;

namespace Fu.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomizedIdentity(this IApplicationBuilder app)
        {
            // Google Authentication
            //
            //app.UseIdentity()
            //    .UseGoogleAuthentication(new GoogleOptions
            //    {
            //        ClientId = "583825788849-8g42lum4trd5g3319go0iqt6pn30gqlq.apps.googleusercontent.com",
            //        ClientSecret = "X8xIiuNEUjEYfiEfiNrWOfI4"
            //    });

            // Facebook Authentication
            //app.UseIdentity()
            //    .UseFacebookAuthentication(new FacebookOptions
            //    {
            //        AppId = "1716532045292977",
            //        AppSecret = "dfece01ae919b7b8af23f962a1f87f95"
            //    });

            // Active Directory
            app.UseIdentity()
                .UseNtlm(new ActiveDirectoryOptions
                {
                    AutomaticAuthenticate = false,
                    AutomaticChallenge = false,
                    AuthenticationScheme = ActiveDirectoryOptions.DefaultAuthenticationScheme,
                    SignInAsAuthenticationScheme = ActiveDirectoryOptions.DefaultAuthenticationScheme
                });

            return app;
        }

        public static IApplicationBuilder UseCustomizedMvc(this IApplicationBuilder app)
        {
        
            app.UseMvc(routes =>
            {
                routes.Routes.Add(new UrlSlugRoute(routes.DefaultHandler));
                RoutesManager.RegisterRoutes(routes);

                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }

        public static IApplicationBuilder UseCustomizedStaticFiles(this IApplicationBuilder app, IList<ModuleInfo> modules)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(60)
                    };
                }
            });

            return app;
        }

        public static IApplicationBuilder UseCustomizedRequestLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("vi-VN"),
                new CultureInfo("fr-FR"),
                new CultureInfo("pt-BR"),
                new CultureInfo("uk-UA"),
                new CultureInfo("id-ID")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("vi-VN", "vi-VN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }
    }
}
