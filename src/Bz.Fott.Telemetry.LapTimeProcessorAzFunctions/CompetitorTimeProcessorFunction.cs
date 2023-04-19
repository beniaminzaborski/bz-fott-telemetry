using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Bz.Fott.Telemetry.LapTimeProcessorAzFunctions;

public class CompetitorTimeProcessorFunction
{
    public CompetitorTimeProcessorFunction()
    {
        
    }

    [FunctionName("CompetitorTimeProcessorFunction")]
    public void Run([CosmosDBTrigger(
        databaseName: "fott_telemetry",
        collectionName: "LapTimes",
        ConnectionStringSetting = "",
        LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
        ILogger log)
    {
        if (input != null && input.Count > 0)
        {
            // TODO: Check if competition & competitor exist!

            log.LogInformation("Documents modified " + input.Count);
            log.LogInformation("First document Id " + input[0].Id);
        }
    }
}
