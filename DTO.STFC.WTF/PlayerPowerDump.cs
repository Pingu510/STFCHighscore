using System.Text.Json.Serialization;

namespace STFCTools.PlayerHighScore.DTO.STFC.WTF;

internal class PlayerPowerDump
{
	[JsonPropertyName("count")]
	public int Count { get; set; }

	[JsonPropertyName("players")]
	public string? PlayersEncoded { get; set; }

	[JsonPropertyName("lastcached")]
	public DateTime Lastcached { get; set; }

	[JsonPropertyName("serverData")]
	public List<int> ServerData { get; set; } = new();

	[JsonPropertyName("regionData")]
	public List<string> RegionData { get; set; } = new();

	[JsonPropertyName("levelData")]
	public List<int> LevelData { get; set; } = new();

	[JsonPropertyName("playerIndex")]
	public int PlayerIndex { get; set; }

	[JsonPropertyName("playerPage")]
	public int PlayerPage { get; set; }
}
