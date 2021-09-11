using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Azure.Identity;

namespace BWHazel.Aka.Web
{
    /// <summary>
    /// Main program logic.
    /// </summary>
    public static class Program
    {
        private const string AzureAdTenantIdKey = "AzureAD:TenantId";
        private const string AzureAdClientIdKey = "AzureAD:ClientId";
        private const string AzureAdClientSecretKet = "AzureAD:ClientSecret";
        private const string SecretsKeyVaultKey = "Secrets:KeyVault";

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        /// <summary>
        /// Creates the host builder.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    IConfigurationRoot rootConfig = config
                        .AddEnvironmentVariables()
                        .Build();

                    config.AddAzureKeyVault(
                        new Uri($"https://{rootConfig[SecretsKeyVaultKey]}.vault.azure.net"),
                        new ClientSecretCredential(
                            rootConfig[AzureAdTenantIdKey],
                            rootConfig[AzureAdClientIdKey],
                            rootConfig[AzureAdClientSecretKet])
                        );
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
