﻿using Bz.Fott.Administration.Application.Competitions;
using Bz.Fott.Registration;
using Bz.Fott.Telemetry.IntegrationAzFunctions;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;
using MassTransit;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
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
            .AddMassTransitForAzureFunctions(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                cfg.AddRequestClient<CompetitionCheckpointAddedIntegrationEvent>(new Uri("queue:checkpoints-events-to-telemetry-service"));
                cfg.AddRequestClient<CompetitionCheckpointRemovedIntegrationEvent>(new Uri("queue:checkpoints-events-to-telemetry-service"));
                cfg.AddRequestClient<CompetitorRegisteredIntegrationEvent>(new Uri("queue:registration-completed-events-to-telemetry-service"));
            },
            "AzureWebJobsServiceBus");
    }
}
