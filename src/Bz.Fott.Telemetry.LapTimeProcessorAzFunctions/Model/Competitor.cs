using Newtonsoft.Json;
using System;

namespace Bz.Fott.Telemetry.IntegrationAzFunctions.Model;

internal class Competitor
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("number")]
    public string Number { get; set; }
    
    [JsonProperty("competitionId")]
    public Guid CompetitionId { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("birthDate")]
    public DateTime BirthDate { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
}
