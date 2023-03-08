using Bz.Fott.Administration.Application.Competitions;
using MassTransit;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;

public class CompetitionCheckpointRemovedIntegrationEventConsumer : IConsumer<CompetitionCheckpointRemovedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CompetitionCheckpointRemovedIntegrationEvent> context)
    {
        LogContext.Debug?.Log("Checkpoint removed: {CheckpointId}", context.Message.CheckpointId);
    }
}
