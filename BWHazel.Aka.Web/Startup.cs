using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using BWHazel.Aka.Data;
using BWHazel.Aka.Web.Services;

namespace BWHazel.Aka.Web
{
    /// <summary>
    /// Configures the application on startup.
    /// </summary>
    public class Startup
    {
        private const string AkaDataStoreConnectionStringKey = "AkaDataStore";
        private const string AzureAdSectionKey = "AzureAD";
        private const string CosmosDbDatabaseKey = "CosmosDb:Database";
        private const string GroupsKey = "Groups";

        /// <summary>
        /// Initialises a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures and adds services to the application container.
        /// </summary>
        /// <param name="services">The application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IdentityService>();
            services.AddSingleton<ShortUrlService>();
            services.AddScoped<DataService>();

            services.AddMicrosoftIdentityWebAppAuthentication(
                this.Configuration,
                AzureAdSectionKey);

            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

            services.AddAuthorization(options =>
            {
                this.AddAuthorisationPolicies(options);
            });

            services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.AccessDeniedPath = new("/Account/AccessDenied");
            });

            services.AddDbContext<AkaDbContext>(options =>
            {
                options.UseCosmos(
                    this.Configuration.GetConnectionString(AkaDataStoreConnectionStringKey),
                    this.Configuration[CosmosDbDatabaseKey]);
            });
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "AkaLink",
                    pattern: "{linkId}",
                    defaults: new { controller = "Links", action = "Open" });
            });
        }

        /// <summary>
        /// Add authorisation policies for all application groups.
        /// </summary>
        /// <param name="authorisationOptions">The authorisation options.</param>
        private void AddAuthorisationPolicies(AuthorizationOptions authorisationOptions)
        {
            IEnumerable<KeyValuePair<string, string>> groups =
                this.Configuration
                    .GetSection(GroupsKey)
                    .GetChildren()
                    .ToDictionary(s => s.Key, s => s.Value);

            foreach (KeyValuePair<string, string> group in groups)
            {
                authorisationOptions.AddPolicy(
                    group.Key,
                    new AuthorizationPolicyBuilder().RequireClaim(
                        "groups",
                        group.Value
                    ).Build()
                );
            }
        }
    }
}
