using Azure.Messaging.ServiceBus;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;
using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions;

public class RemoveCheckpointIntegrationFunction
{
    public const string QueueName = "remove-checkpoint-events-to-telemetry-service";
    readonly IMessageReceiver _receiver;
    readonly ILogger<RemoveCheckpointIntegrationFunction> _logger;

    public RemoveCheckpointIntegrationFunction(
        IMessageReceiver receiver,
        ILogger<RemoveCheckpointIntegrationFunction> logger)
    {
        _receiver = receiver;
        _logger = logger;
    }

    [FunctionName("RemoveCheckpointFunction")]
    public Task Run([ServiceBusTrigger(QueueName, Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Run RegisterCompetitorFunction");
        return _receiver.HandleConsumer<CompetitionCheckpointRemovedIntegrationEventConsumer>(QueueName, message, cancellationToken);
    }
}
