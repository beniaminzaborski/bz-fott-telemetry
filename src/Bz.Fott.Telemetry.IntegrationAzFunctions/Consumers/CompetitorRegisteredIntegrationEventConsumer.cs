using Bz.Fott.Registration;
using MassTransit;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;

public class CompetitorRegisteredIntegrationEventConsumer : IConsumer<CompetitorRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<CompetitorRegisteredIntegrationEvent> context)
    {
        LogContext.Debug?.Log("Competitor regisered {FirstName} {LastName} on competition {CompetitionId}",
            context.Message.FirstName,
            context.Message.LastName,
            context.Message.CompetitionId);

    }
}
