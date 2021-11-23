using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using BWHazel.Aka.Web;

const string AzureAdTenantIdKey = "AzureAD:TenantId";
const string AzureAdClientIdKey = "AzureAD:ClientId";
const string AzureAdClientSecretKet = "AzureAD:ClientSecret";
const string SecretsKeyVaultKey = "Secrets:KeyVault";

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
    })
    .Build()
    .Run();