using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using Fu.Infrastructure;
using Fu.Infrastructure.Web;
using Fu.Module.Core.Extensions;
using Fu.Module.Core.Models;
using Fu.Module.Localization;
using Fu.Web.Extensions;
using Serilog;
using Fu.Web.Services;

namespace Fu.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private static readonly IList<ModuleInfo> Modules = new List<ModuleInfo>();

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();

            var connectionStringConfig = builder.Build();

            builder.AddEntityFrameworkConfig(options => 
                    options.UseSqlServer(connectionStringConfig.GetConnectionString("DefaultConnection"))
            );

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
               .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "logs\\log-{Date}.log"))
               .CreateLogger();

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            GlobalConfiguration.WebRootPath = _hostingEnvironment.WebRootPath;
            GlobalConfiguration.ContentRootPath = _hostingEnvironment.ContentRootPath;
            services.LoadModules(Modules, _hostingEnvironment);

            services.AddCustomizedDataStore(Configuration);
            services.AddCustomizedIdentity();

            services.AddSingleton<IStringLocalizerFactory, EfStringLocalizerFactory>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddScoped<SignInManager<User>, CoreSignInManager<User>>();

            services.Configure<RazorViewEngineOptions>(
                options => {
                    options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
                    options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
                });

            services.AddCustomizedMvc(GlobalConfiguration.Modules);

            // Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddMvc();

            //// Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            return services.Build(Configuration, _hostingEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                loggerFactory.WithFilter(new FilterLoggerSettings
                {
                    { "Microsoft", LogLevel.Warning },
                    { "System", LogLevel.Warning },
                    { "Fu", LogLevel.Debug }
                })
                .AddConsole()
                .AddSerilog();

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                loggerFactory.WithFilter(new FilterLoggerSettings
                {
                    { "Microsoft", LogLevel.Warning },
                    { "System", LogLevel.Warning },
                    { "Fu", LogLevel.Error }
                })
                .AddSerilog();

                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Home/ErrorWithCode/{0}");

            app.UseCustomizedRequestLocalization();
            app.UseCustomizedStaticFiles(Modules);
            app.UseCustomizedIdentity();
            app.UseCustomizedMvc();
        }
    }
}
