using angelasFunctionApp2.DataAccess;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((config) =>
    {
        
        var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri") ?? throw new InvalidOperationException("Key Vault URI not found"));

        config.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        string sqlConnectionString = configuration["ConnectionString"]; 

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(sqlConnectionString);
        });

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

await host.RunAsync();