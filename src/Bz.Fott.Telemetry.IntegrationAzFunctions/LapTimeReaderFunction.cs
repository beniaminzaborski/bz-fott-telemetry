using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions
{
    public class LapTimeReaderFunction
    {
        private readonly Container _container;

        public LapTimeReaderFunction(CosmosClient cosmosClient)
        {
            var database = cosmosClient.GetDatabase("fott_telemetry");
            _container = database.GetContainer("LapTimes");
        }

        [FunctionName("LapTimeReaderFunction")]
        public async Task Run([EventHubTrigger("evh-fott-qa-westeu", Connection = "EventHubConnectionString")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    log.LogInformation($"Event received from Hub: {eventData.EventBody}");
                    var @event = JsonSerializer.Deserialize<LapTime>(eventData.EventBody);
                    await _container.CreateItemAsync(@event);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
