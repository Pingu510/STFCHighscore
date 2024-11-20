using System.Data;

namespace STFCTools.PlayerHighScore;

internal class Program
{
	static readonly string playerOrderLocation = "C:\\Users\\Porta\\Desktop\\Star Trek Fleet Command\\import_googledocs_playerorder.txt";
	static readonly string outputLocation = "C:\\Users\\Porta\\Desktop\\Star Trek Fleet Command\\export_player_scores.txt";
	static readonly string allianceName = "ALLIANCETAG";
	static readonly string serverId = "SERVERNUMBER";

	static void Main(string[] args)
	{
		string allianceScoreSource = "https://stfc.wtf/api/players?type=player_data_power&region=EU&server="
			+ serverId + "&sortBy=rank&sortOrder=asc&tag="
			+ allianceName + "&rankMatch=true";

		// get external data
		var htmlData = DataFetch.GetPageContent(allianceScoreSource);
		htmlData.Wait();

		// read external data
		var playerString = DataFetch.PlayerPowerDumpFromJson(htmlData.Result)?.PlayersEncoded;
		var playerData = DataFetch.PlayerDataFromJson(DataFetch.DecodeString(playerString));

		// check input variations
		var playerOrder = ReadPlayerOrder(playerOrderLocation);
		if(UpdateNamesQuery(playerData, playerOrder))
		{
			playerOrder = ReadPlayerOrder(playerOrderLocation);
		}

		// filter and output
		var players = FilterPlayerData(playerData, playerOrder);
		PrintPlayerData(outputLocation, players);
	}

	static bool UpdateNamesQuery(PlayerDataCollection dataCollection, List<string> oldOrder)
	{
		var listdiff = dataCollection.Players.Where(n => !oldOrder.Any(o => n.Name == o));
		if (listdiff.Any())
		{
			Console.WriteLine("Found new names:");
			foreach (var player in listdiff)
			{
				Console.WriteLine(player.Name);
			}
			Console.WriteLine("Press any button when you have updated the input list at: " + playerOrderLocation);
			Console.ReadKey();
			return true;
		}
		return false;
	}

	static PlayerDataCollection FilterPlayerData(PlayerDataCollection dataCollection, List<string> oldOrder)
	{
		var currentMembers = dataCollection.Players;
		var p = new List<PlayerData>();
		var skipped = new List<PlayerData>();

		foreach (var member in oldOrder)
		{
			var foundPlayer = currentMembers.Find(x => x.Name == member);
			if (foundPlayer != null)
			{
				p.Add(foundPlayer);
				currentMembers.Remove(foundPlayer);
			}
			// add blank row and add to bottom of the list
			else
			{
				p.Add(new PlayerData());
				skipped.Add(new PlayerData() { Name = member });
			}
		}

		p.Add(new PlayerData()); p.Add(new PlayerData()); p.Add(new PlayerData()); p.Add(new PlayerData()); p.Add(new PlayerData());
		p.AddRange(skipped);
		p.AddRange(currentMembers);
		dataCollection.Players = p;
		return dataCollection;
	}

	static void PrintPlayerData(string path, PlayerDataCollection dataCollection)
	{
		var l = new List<string>() { dataCollection.CreationDate.ToLongDateString() };

		l.AddRange(dataCollection.Players.Select(x =>
		{
			return $"{x.Name}\t{x.Power}";
		}).ToList());

		File.WriteAllLines(path, l);
	}

	static List<string> ReadPlayerOrder(string filelocation)
	{
		return File.ReadAllLines(filelocation).ToList() ?? new();
	}
}