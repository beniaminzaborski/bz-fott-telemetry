using System.Collections.Generic;
using System.Threading.Tasks;
using Bz.Fott.Telemetry.IntegrationAzFunctions.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Bz.Fott.Telemetry.LapTimeProcessorAzFunctions;

public class CompetitorTimeProcessorFunction
{
    readonly ILogger<CompetitorTimeProcessorFunction> _logger;
    private readonly Container _checkpointsContainer;
    private readonly Container _competitorsContainer;
    private readonly Container _lapTimesContainer;

    public CompetitorTimeProcessorFunction(
        ILogger<CompetitorTimeProcessorFunction> logger,
        CosmosClient cosmosClient)
    {
        _logger = logger;
        var database = cosmosClient.GetDatabase("fott_telemetry");
        _checkpointsContainer = database.GetContainer("Checkpoints");
        _competitorsContainer = database.GetContainer("Competitors");
        _lapTimesContainer = database.GetContainer("LapTimes");
    }

    [FunctionName("CompetitorTimeProcessorFunction")]
    //public async Task Run([CosmosDBTrigger(
    //    databaseName: "fott_telemetry",
    //    collectionName: "LapTimes",
    //    ConnectionStringSetting = "CosmosConnectionString",
    //    LeaseCollectionName = "leases",
    //    CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<LapTime> input)
    public async Task Run([CosmosDBTrigger(
        databaseName: "fott_telemetry",
        containerName: "LapTimes",
        Connection = "CosmosConnectionString",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)]IReadOnlyList<LapTime> input)
    {
        if (input != null && input.Count > 0)
        {
            _logger.LogInformation("Documents modified " + input.Count);

            foreach (var lapTime in input)
            {
                await ProcessItemAsync(lapTime);
            }
        }
    }

    private async Task ProcessItemAsync(LapTime lapTime)
    {
        _logger.LogInformation($"Processing LapTime with Id: {lapTime.Id}");

        var isValid = await ValidateItemAsync(lapTime);
        if (!isValid)
        {
            _logger.LogInformation($"LapTime with Id: {lapTime.Id} is not valid!");
            return;
        }

        // TODO: Fetch all LapTime items for current document

        // TODO: Fetch all Checkpoints for current competition

        // TODO: Check if all LapTimes for all Checkpoints exists

        // TODO: Calculate overall competitor time
    }

    private async Task<bool> ValidateItemAsync(LapTime lapTime)
    {
        // Check if checkpoint & competitor exist!
        _logger.LogInformation($"Validating LapTime with Id: {lapTime.Id}");

        var checkPointStringId = lapTime.CheckPointId.ToString();
        var competitorStringId = lapTime.CompetitorId.ToString();

        var checkPointResponse = await _checkpointsContainer.ReadItemAsync<CheckPoint>(
            checkPointStringId,
            new Microsoft.Azure.Cosmos.PartitionKey(checkPointStringId));

        _logger.LogInformation($"Checkpoint response code: {checkPointResponse.StatusCode}");

        if (checkPointResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;        
        }

        var competitorResponse = await _checkpointsContainer.ReadItemAsync<Competitor>(
            competitorStringId,
            new Microsoft.Azure.Cosmos.PartitionKey(competitorStringId));

        _logger.LogInformation($"Competitor response code: {competitorResponse.StatusCode}");

        if (competitorResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        return true;
    }
}
