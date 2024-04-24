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
        //int numOfEvents = 3;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        // TODO: Replace the <CONNECTION_STRING> and <HUB_NAME> placeholder values
        EventHubProducerClient producerClient = new EventHubProducerClient(
            "<Put EventHub connection string>",
            "evh-fott-qa-westeu");

        // Create a batch of events 
        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
        var @eventStartLine = CreateEventForStartLine();
        if (!eventBatch.TryAdd(new EventData(SerializeEvent(@eventStartLine))))
        {
            // if it is too large for the batch
            throw new Exception($"Event is too large for the batch and cannot be sent.");
        }

        var @eventFinishLine = CreateEventForFinishLine();
        if (!eventBatch.TryAdd(new EventData(SerializeEvent(@eventFinishLine))))
        {
            // if it is too large for the batch
            throw new Exception($"Event is too large for the batch and cannot be sent.");
        }


        //for (int i = 1; i <= numOfEvents; i++)
        //{
        //    var @event = CreateEvent();

        //    if (!eventBatch.TryAdd(new EventData(SerializeEvent(@event)/*Encoding.UTF8.GetBytes($"Event {i}")*/)))
        //    {
        //        // if it is too large for the batch
        //        throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
        //    }
        //}


        try
        {
            // Use the producer client to send the batch of events to the event hub
            await producerClient.SendAsync(eventBatch);
            Console.WriteLine($"A batch of {eventBatch.Count} events has been published.");
        }
        finally
        {
            await producerClient.DisposeAsync();
        }

        Console.WriteLine("Press <ENTER> to close");
        Console.ReadLine();
    }

    //private static LapTimeEvent CreateEvent()
    //{
    //    return new LapTimeEvent(
    //        Guid.NewGuid(),
    //        new Guid("6dadf095-ad0e-425a-8fb3-082e58c14d2b"),
    //        new Guid("1ec61d87-9a77-4208-a8f9-0c6b58968d29"),
    //        DateTime.UtcNow);
    //}

    private static LapTimeEvent CreateEventForStartLine()
    {
        return new LapTimeEvent(
            Guid.NewGuid(),
            new Guid("6dadf095-ad0e-425a-8fb3-082e58c14d2b"),
            new Guid("1ec61d87-9a77-4208-a8f9-0c6b58968d29"),
            DateTime.UtcNow.AddHours(-2).AddMinutes(-3));
    }

    private static LapTimeEvent CreateEventForFinishLine()
    {
        return new LapTimeEvent(
            Guid.NewGuid(),
            new Guid("6dadf095-ad0e-425a-8fb3-082e58c14d2b"),
            new Guid("1ec61d87-9a77-4208-a8f9-0c6b58968d29"),
            DateTime.UtcNow);
    }

    private static string SerializeEvent(LapTimeEvent @event)
    {
        return JsonSerializer.Serialize(@event);
    }
}