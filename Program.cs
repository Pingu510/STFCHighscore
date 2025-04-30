using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Text;
using STFCTools.PlayerHighScore.DTO.STFC.PRO;
using STFCTools.PlayerHighScore.Models;

namespace STFCTools.PlayerHighScore;

internal class Program
{
    static readonly string baseLocation = "C:\\tmp\\";
    static readonly string playerOrderLocation = "C:\\tmp\\import_playerorder.txt";
    static readonly string outputLocation = "C:\\tmp\\export_player_scores.txt";

    static void Main(string[] args)
    {          
        string alliance = @"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";
        string result = StringHelper.DecodeString(alliance);


        string allianceScoreSource = @"https://stfc.wtf/api/allianceDetails?id=2495300849";
        //string allianceScoreSource = "https://stfc.wtf/api/players?type=player_data_power&region=EU&server="
        //                + serverId + "&sortBy=rank&sortOrder=asc&tag="
        //    + allianceName + "&rankMatch=true";

        DownloadDataAndSaveToFile((allianceScoreSource), baseLocation + nameof(allianceScoreSource) + ".json");
        GetDataAndDecodeAllianceDetails(baseLocation + nameof(allianceScoreSource) + ".json");
        var playerData = GetPlayersFromJson(baseLocation + "members.json");
        var playerCollection = DataFetch.PlayerDataFromDTO(playerData);


        // check input variations
        var playerOrder = FileHelper.ReadFileToList(playerOrderLocation);
        if (UpdateNamesQuery(playerCollection, playerOrder))
        {
            playerOrder = FileHelper.ReadFileToList(playerOrderLocation);
        }

        // filter and output
        var players = FilterPlayerData(playerCollection, playerOrder);
        PrintPlayerData(outputLocation, players);
    }

    private void Settings()
    {
        Console.WriteLine("Starting player score update {0}");
        ConsoleKeyInfo input;
        //do
        //{
        //    Console.WriteLine("Configure [1] or use default[0] parameters?");
        //    input = Console.ReadKey();
        //}
        //while (Char.Equals(input.KeyChar.ToString(), '0') || input.KeyChar.Equals(Char.GetNumericValue('1')));

        //if (input.KeyChar == '1')
        //{
        //    //ask for conkfiguration
        //}
    }

    /// <summary>
    /// Get data from url and save to file
    /// </summary>
    /// <param name="url"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static bool DownloadDataAndSaveToFile(string url, string filePath)
    {
        try
        {
            var htmlData = DataFetch.GetPageContent(url);
            htmlData.Wait();

            FileHelper.SaveStringToFile(filePath, htmlData.Result.JsonPrettify());
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    /// <summary>
    /// Load Data and decode alliance details, save alliance and members to file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static bool GetDataAndDecodeAllianceDetails(string filePath)
    {
        try
        {
            var model = LoadJSONDataAndDeserialize<AllianceDetails>(filePath);
            if (model != null)
            {
                // decode alliance and members
                string alliance = StringHelper.DecodeString(model.alliance);
                string members = StringHelper.DecodeString(model.members);

                FileHelper.SaveStringToFile(baseLocation + nameof(alliance) + ".json", alliance.JsonPrettify());
                FileHelper.SaveStringToFile(baseLocation + nameof(members) + ".json", members.JsonPrettify());
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    /// <summary>
    /// Read player information from file
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private static IEnumerable<Player> GetPlayersFromJson(string filepath)
    {
        try
        {
            var players = LoadJSONDataAndDeserialize<IEnumerable<Player>>(filepath);
            return players ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }


    /// <summary>
    /// Load json data from file and deserialize
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static T? LoadJSONDataAndDeserialize<T>(string filePath)
    {
        try
        {
            var input = FileHelper.ReadFileToString(filePath);
            var model = System.Text.Json.JsonSerializer.Deserialize<T>(input);

            return model;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
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

        FileHelper.SaveListToFile(path, l);
    }
}