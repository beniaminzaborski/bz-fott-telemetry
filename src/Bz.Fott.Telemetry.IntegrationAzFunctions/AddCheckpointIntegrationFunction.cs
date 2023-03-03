using Azure.Messaging.ServiceBus;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Consumers;
using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions;

public class AddCheckpointIntegrationFunction
{
    const string checkpointsQueueName = "checkpoints";
    readonly IMessageReceiver _receiver;

    public AddCheckpointIntegrationFunction(IMessageReceiver receiver)
    {
        _receiver = receiver;
    }

    [FunctionName("AddCheckpointFunction")]
    public Task Run([ServiceBusTrigger(checkpointsQueueName)] ServiceBusReceivedMessage message, CancellationToken cancellationToken)
    {
        return _receiver.HandleConsumer<CompetitionCheckpointAddedIntegrationEventConsumer>(checkpointsQueueName, message, cancellationToken);
    }
}
