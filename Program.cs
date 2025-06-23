using System.ComponentModel;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using STFCTools.PlayerHighScore.DTO.STFC.PRO;
using STFCTools.PlayerHighScore.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace STFCTools.PlayerHighScore;

internal class Program
{

    static void Main(string[] args)
    {
        // Define the base location and file paths for player order and output files
        string baseLocation = "C:\\tmp\\";
        string playerOrderLocation_Power = baseLocation + "import_playerorder_power.txt";
        string playerOrderLocation_Tournament = baseLocation + "import_playerorder_tournament.txt";
        string outputLocation_Power = baseLocation + "export_player_scores_power.txt";
        string outputLocation_Tournament = baseLocation + "export_player_scores_tournament.txt";

        // Define the source URLs for alliance tournament and details
        string allianceTournamentSource = @"https://stfc.wtf/api/tournamentDetails?";
        string allianceDetailsSource = @"https://stfc.wtf/api/allianceDetails?";
        var allianceId = "2495300849";

        if (args.Length != 0)
        {
            if (args.Length == 1)
            {
                // Check if the argument is a valid number
                if (!Int32.TryParse(args[0], out _))
                {
                    Console.WriteLine("Invalid allianceId. Please provide a valid number.");
                    return;
                }
                allianceId = args[0];
            }
            else
            {
                Console.WriteLine("Usage: STFCTools.PlayerHighScore [allianceId]");
                return;
            }
        }
        else
        {
            Console.WriteLine("No allianceId provided, using default: " + allianceId);
        }

        var p = new Program();
        PlayerDataCollection players;
        switch (p.Settings())
        {
            case 1:
                players = UseScrape("Power",
                    allianceDetailsSource + "id=" + allianceId,
                    baseLocation,
                    playerOrderLocation_Power);

                PrintPlayerPowerData(outputLocation_Power, players);
                break;
            case 2:
                players = UseScrape("Tournament",
                    allianceTournamentSource + "id=" + allianceId,
                    baseLocation,
                    playerOrderLocation_Tournament);

                PrintPlayerTournamentData(outputLocation_Tournament, players);
                break;
            default:
                Console.WriteLine("Invalid option selected.");
                break;
        }

        Console.WriteLine("Program completed without errors.");
    }

    private int Settings()
    {
        Console.WriteLine("Initializing stfc.wtf scraper for alliances");
        Console.WriteLine("Enter '1' for power scores or '2' for tournament scores");
        while (true)
        {
            var input = Console.ReadKey();
            Console.WriteLine();

            if (!Char.IsNumber(input.KeyChar))
            { continue; }

            // Parse the input to an integer
            var userOption = Int32.Parse(input.KeyChar.ToString());

            if (userOption == 1 || userOption == 2)
            {
                return userOption;
            }
            Console.WriteLine("Invalid input, please enter '1' or '2'.");
        }
    }

    private static PlayerDataCollection UseScrape(string name, string sourceUrl, string baseLocation, string playerOrderLocation)
    {
        DownloadDataAndSaveToFile((sourceUrl), baseLocation + name + ".json");
        GetDataAndDecodeAllianceDetails(baseLocation + name + ".json", baseLocation);
        var playerData = GetPlayersFromJson(baseLocation + "members.json");
        var playerCollection = DataFetch.PlayerDataFromDTO(playerData);
        List<string> playerOrder = [];

        // check input variations
        try
        {
            playerOrder = FileHelper.ReadFileToList(playerOrderLocation);
            if (UpdateNamesQuery(playerCollection, playerOrder, playerOrderLocation))
            {
                playerOrder = FileHelper.ReadFileToList(playerOrderLocation);
            }
        }
        catch (FileNotFoundException)
        {
        }

        // filter and output
        return FilterPlayerData(playerCollection, playerOrder);
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
    private static bool GetDataAndDecodeAllianceDetails(string filePath, string baseLocation)
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




    static bool UpdateNamesQuery(PlayerDataCollection dataCollection, List<string> oldOrder, string Location)
    {
        var listdiff = dataCollection.Players.Where(n => !oldOrder.Any(o => n.Name == o));
        if (listdiff.Any())
        {
            Console.WriteLine("Found new names:");
            foreach (var player in listdiff)
            {
                Console.WriteLine(player.Name);
            }
            Console.WriteLine("Press any button when you have updated the input list at: " + Location);
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

    static void PrintPlayerPowerData(string path, PlayerDataCollection dataCollection)
    {
        var l = new List<string>() { dataCollection.CreationDate.ToLongDateString() };

        l.AddRange(dataCollection.Players.Select(x =>
        {
            return $"{x.PlayerId}\t{x.Name}\t{x.Power}";
        }).ToList());

        FileHelper.SaveListToFile(path, l);
    }


    static void PrintPlayerTournamentData(string path, PlayerDataCollection dataCollection)
    {
        var l = new List<string>() { dataCollection.CreationDate.ToLongDateString() };
        l.AddRange(dataCollection.Players.Select(x =>
        {
            return $"{x.PlayerId}\t{x.Name}\t{x.CompletedTasks}\t{x.StartedTasks}\t{x.TornamentScore}";
        }).ToList());
        FileHelper.SaveListToFile(path, l);
    }
}