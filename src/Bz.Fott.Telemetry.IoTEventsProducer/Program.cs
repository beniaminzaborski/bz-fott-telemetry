using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Bz.Fott.Telemetry;
using System.Text;
using System.Text.Json;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Press <ENTER> to start producing events");
        Console.ReadLine();

        // number of events to be sent to the event hub
        int numOfEvents = 3;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        // TODO: Replace the <CONNECTION_STRING> and <HUB_NAME> placeholder values
        EventHubProducerClient producerClient = new EventHubProducerClient(
            "<Put EventHub connection string>",
            "evh-fott-qa-westeu");

        // Create a batch of events 
        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

        for (int i = 1; i <= numOfEvents; i++)
        {
            var @event = CreateEvent();

            if (!eventBatch.TryAdd(new EventData(SerializeEvent(@event)/*Encoding.UTF8.GetBytes($"Event {i}")*/)))
            {
                // if it is too large for the batch
                throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
            }
        }

        try
        {
            // Use the producer client to send the batch of events to the event hub
            await producerClient.SendAsync(eventBatch);
            Console.WriteLine($"A batch of {numOfEvents} events has been published.");
        }
        finally
        {
            await producerClient.DisposeAsync();
        }

        Console.WriteLine("Press <ENTER> to close");
        Console.ReadLine();
    }

    private static LapTimeEvent CreateEvent()
    {
        return new LapTimeEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow);
    }

    private static string SerializeEvent(LapTimeEvent @event)
    {
        return JsonSerializer.Serialize(@event);
    }
}