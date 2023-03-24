using Newtonsoft.Json;
using System;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Model;

internal class CheckPoint
{
    [JsonProperty("id")]
    public Guid CheckpointId { get; set; }

    [JsonProperty("competitionId")]
    public Guid CompetitionId { get; set; }

    [JsonProperty("trackPointDistance")]
    public decimal TrackPointDistance { get; set; }

    [JsonProperty("trackPointUnit")]
    public string TrackPointUnit { get; set; }
}
