using Bz.Fott.Administration.Application.Competitions;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Model;
using MassTransit;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;

public class CompetitionCheckpointAddedIntegrationEventConsumer : IConsumer<CompetitionCheckpointAddedIntegrationEvent>
{
    private readonly Container _container;

    public CompetitionCheckpointAddedIntegrationEventConsumer(CosmosClient cosmosClient)
    {
        var database = cosmosClient.GetDatabase("fott_telemetry");
        _container = database.GetContainer("Checkpoints");
    }

    public async Task Consume(ConsumeContext<CompetitionCheckpointAddedIntegrationEvent> context)
    {
        var message = context.Message;

        LogContext.Debug?.Log("Checkpoint added: {CheckpointId}", message.CheckpointId);

        var checkPoint = new CheckPoint
        {
            CheckpointId = message.CheckpointId,
            CompetitionId = message.CompetitionId,
            TrackPointDistance = message.TrackPointDistance,
            TrackPointUnit = message.TrackPointUnit
        };

        await _container.CreateItemAsync(checkPoint);
    }
}
