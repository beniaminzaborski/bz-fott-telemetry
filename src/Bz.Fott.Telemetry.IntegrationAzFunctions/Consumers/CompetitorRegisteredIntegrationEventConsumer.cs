using Bz.Fott.Registration;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Model;
using MassTransit;
using Microsoft.Azure.Cosmos;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;

public class CompetitorRegisteredIntegrationEventConsumer : IConsumer<CompetitorRegisteredIntegrationEvent>
{
    private readonly Container _container;

    public CompetitorRegisteredIntegrationEventConsumer(CosmosClient cosmosClient)
    {
        var database = cosmosClient.GetDatabase("fott_telemetry");
        _container = database.GetContainer("Competitors");
    }

    public async Task Consume(ConsumeContext<CompetitorRegisteredIntegrationEvent> context)
    {
        var message = context.Message;

        LogContext.Debug?.Log("Competitor registered {FirstName} {LastName} on competition {CompetitionId}",
            message.FirstName,
            message.LastName,
            message.CompetitionId);

        var competitor = new Competitor
        {
            Id = message.CompetitorId,
            Number = message.Number,
            CompetitionId = message.CompetitionId,
            FirstName = message.FirstName,
            LastName = message.LastName,
            BirthDate = message.BirthDate,
            City = message.City,
            PhoneNumber = message.PhoneNumber
        };

        await _container.CreateItemAsync(competitor);
    }
}
