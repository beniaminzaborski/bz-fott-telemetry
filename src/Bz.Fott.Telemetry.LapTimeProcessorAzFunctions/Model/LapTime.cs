using Newtonsoft.Json;
using System;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Model;

internal class LapTime
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("checkPointId")]
    public Guid CheckPointId { get; set; }

    [JsonProperty("competitorId")]
    public Guid CompetitorId { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}
