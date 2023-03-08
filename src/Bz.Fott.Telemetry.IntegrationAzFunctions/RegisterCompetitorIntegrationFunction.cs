using Azure.Messaging.ServiceBus;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;
using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions;

public class RegisterCompetitorIntegrationFunction
{
    const string queueName = "registration-completed-events-to-telemetry-service";
    readonly IMessageReceiver _receiver;
    readonly ILogger _logger;

    public RegisterCompetitorIntegrationFunction(
        IMessageReceiver receiver, ILogger logger)
    {
        _receiver = receiver;
        _logger = logger;
    }

    [FunctionName("RegisterCompetitorFunction")]
    public Task Run([ServiceBusTrigger(queueName, Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Run RegisterCompetitorFunction");
        return _receiver.HandleConsumer<CompetitionCheckpointRemovedIntegrationEventConsumer>(queueName, message, cancellationToken);
    }
}
