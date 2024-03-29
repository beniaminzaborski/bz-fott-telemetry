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
    public const string QueueName = "registration-completed-events-to-telemetry-service";
    readonly IMessageReceiver _receiver;
    readonly ILogger<RegisterCompetitorIntegrationFunction> _logger;

    public RegisterCompetitorIntegrationFunction(
        IMessageReceiver receiver,
        ILogger<RegisterCompetitorIntegrationFunction> logger)
    {
        _receiver = receiver;
        _logger = logger;
    }

    [FunctionName("RegisterCompetitorFunction")]
    public Task Run([ServiceBusTrigger(QueueName, Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Run RegisterCompetitorFunction");
        return _receiver.HandleConsumer<CompetitorRegisteredIntegrationEventConsumer>(QueueName, message, cancellationToken);
    }
}
