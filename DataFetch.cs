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
				PlayerId = (player.PlayerId.ToString() ?? string.Empty),
				Name = player.Owner ?? string.Empty,				
				Power = player.MaxPower.ToString() ?? string.Empty,
				Level = player.Level.ToString() ?? string.Empty,
                StartedTasks = player.TotalTasks.ToString() ?? string.Empty,
				CompletedTasks = player.CompletedTasks.ToString() ?? string.Empty,
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
