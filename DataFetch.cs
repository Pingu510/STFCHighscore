using STFCTools.PlayerHighScore.DTO.STFC.PRO;
using STFCTools.PlayerHighScore.Models;

namespace STFCTools.PlayerHighScore;

internal static class DataFetch
{
	internal static async Task<string> GetPageContent(string pageUrl)
	{
		using var client = new HttpClient();
		return await client.GetStringAsync(pageUrl);
	}

	internal static IEnumerable<Player> GetPlayersFromJson(string playerData)
    {
        return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Player>>(playerData) ?? [];
    }

    internal static PlayerDataCollection PlayerDataFromDTO(IEnumerable<Player> players)
	{
        PlayerDataCollection c = new()
        {
            CreationDate = players.First().LastModified
        };

        foreach (var player in players)
		{
			c.Players.Add(new PlayerData()
			{				
				Name = player.Owner ?? string.Empty,				
				Power = player.MaxPower.ToString() ?? string.Empty,
                TournamentTasks = player.Tasks ?? string.Empty,
                TornamentScore = player.TournamentScore?.ToString() ?? string.Empty
            });
		}

		return c;
	}

	internal static PlayerPowerDump? PlayerPowerDumpFromJson(string playerData)
	{
        return System.Text.Json.JsonSerializer.Deserialize<PlayerPowerDump>(playerData);
	}	
}
