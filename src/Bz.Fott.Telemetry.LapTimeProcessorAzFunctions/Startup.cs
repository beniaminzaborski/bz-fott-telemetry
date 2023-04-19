using Bz.Fott.Administration.Application.Competitions;
using Bz.Fott.Registration;
using Bz.Fott.Telemetry.IntegrationAzFunctions;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;
using Bz.Fott.Telemetry.LapTimeProcessorAzFunctions;
using MassTransit;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Bz.Fott.Telemetry.IntegrationAzFunctions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddScoped<AddCheckpointIntegrationFunction>()
            .AddScoped<RemoveCheckpointIntegrationFunction>()
            .AddScoped<RegisterCompetitorIntegrationFunction>()
            .AddScoped<LapTimeReaderFunction>()
            .AddScoped<CompetitorTimeProcessorFunction>()
            .AddMassTransitForAzureFunctions(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                cfg.AddRequestClient<CompetitionCheckpointAddedIntegrationEvent>(new Uri($"queue:{AddCheckpointIntegrationFunction.QueueName}"));
                cfg.AddRequestClient<CompetitionCheckpointRemovedIntegrationEvent>(new Uri($"queue:{RemoveCheckpointIntegrationFunction.QueueName}"));
                cfg.AddRequestClient<CompetitorRegisteredIntegrationEvent>(new Uri($"queue:{RegisterCompetitorIntegrationFunction.QueueName}"));
            },
            "ServiceBusConnectionString")
            .AddSingleton(sp => 
            {
                var cosmosDbConnectionString = sp.GetRequiredService<IConfiguration>().GetConnectionStringOrSetting("CosmosConnectionString");
                CosmosClientBuilder cosmosClientBuilder = new CosmosClientBuilder(cosmosDbConnectionString);

                return cosmosClientBuilder.WithConnectionModeDirect()
                    .WithApplicationRegion("North Europe")
                    .WithBulkExecution(true)
                    .Build();
            });
    }
}
