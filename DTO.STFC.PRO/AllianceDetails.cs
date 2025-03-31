using System.Text.Json.Serialization;

namespace STFCTools.PlayerHighScore.DTO.STFC.PRO;

internal class AllianceDetails
{
    [JsonPropertyName("alliance")]
    public string alliance { get; set; }

    [JsonPropertyName("members")]
    public string members { get; set; }

    [JsonPropertyName("diplomacy")]
    public string diplomacy { get; set; }

    [JsonPropertyName("diplomrev")]
    public string diplomrev { get; set; }

    [JsonPropertyName("lastcached")]
    public DateTime lastcached { get; set; }
}
