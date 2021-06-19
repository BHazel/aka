using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BWHazel.Aka.Data;

namespace BWHazel.Aka.Web
{
    /// <summary>
    /// Configures the application on startup.
    /// </summary>
    public class Startup
    {
        private const string AkaDataStoreConnectionStringKey = "AkaDataStore";
        private const string CosmosDbDatabaseKey = "CosmosDb:Database";

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
            services.AddControllersWithViews();
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
    }
}
