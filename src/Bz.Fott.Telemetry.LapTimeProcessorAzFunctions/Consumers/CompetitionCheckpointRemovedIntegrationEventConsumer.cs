using Bz.Fott.Administration.Application.Competitions;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Model;
using MassTransit;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;

public class CompetitionCheckpointRemovedIntegrationEventConsumer : IConsumer<CompetitionCheckpointRemovedIntegrationEvent>
{
    private readonly Container _container;

    public CompetitionCheckpointRemovedIntegrationEventConsumer(CosmosClient cosmosClient)
    {
        var database = cosmosClient.GetDatabase("fott_telemetry");
        _container = database.GetContainer("Checkpoints");
    }

    public async Task Consume(ConsumeContext<CompetitionCheckpointRemovedIntegrationEvent> context)
    {
        var message = context.Message;

        LogContext.Debug?.Log("Checkpoint removed: {CheckpointId}", message.CheckpointId);

        await _container.DeleteItemAsync<CheckPoint>(message.CheckpointId.ToString(), new PartitionKey(message.CheckpointId.ToString()));
    }
}
