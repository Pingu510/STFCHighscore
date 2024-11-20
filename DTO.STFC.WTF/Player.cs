using System.Text.Json.Serialization;

namespace STFCTools.PlayerHighScore.DTO.STFC.WTF;

internal class Player
{
    [JsonPropertyName("max_power")]
    public int MaxPower { get; set; }

    [JsonPropertyName("power")]
    public int Power { get; set; }

    [JsonPropertyName("playerid")]
    public long Playerid { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("suffix")]
    public object? Suffix { get; set; }

    [JsonPropertyName("server")]
    public int Server { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("lastmodified")]
    public DateTime Lastmodified { get; set; }

    [JsonPropertyName("rank")]
    public int Rank { get; set; }
}
