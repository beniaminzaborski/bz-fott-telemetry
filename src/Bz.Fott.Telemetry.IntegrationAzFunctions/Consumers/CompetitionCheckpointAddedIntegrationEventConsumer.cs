using Bz.Fott.Administration.Application.Competitions;
using MassTransit;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;

public class CompetitionCheckpointAddedIntegrationEventConsumer : IConsumer<CompetitionCheckpointAddedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CompetitionCheckpointAddedIntegrationEvent> context)
    {
        LogContext.Debug?.Log("Checkpoint added: {CheckpointId}", context.Message.CheckpointId);

    }
}
