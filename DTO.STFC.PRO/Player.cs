using System.Text.Json.Serialization;

namespace STFCTools.PlayerHighScore.DTO.STFC.PRO;

internal class Player
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [JsonPropertyName("playerid")]
    public object PlayerId { get; set; }

    [JsonPropertyName("shaplayerid")]
    public string ShaplayerId { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("suffix")]
    public object Suffix { get; set; }

    [JsonPropertyName("comment")]
    public object Comment { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("arank")]
    public object AllianceRoleRank { get; set; }

    [JsonPropertyName("rankid")]
    public int? RankId { get; set; }

    [JsonPropertyName("rankdesc")]
    public string RankDesc { get; set; }

    [JsonPropertyName("avatar")]
    public object Avatar { get; set; }

    [JsonPropertyName("frame")]
    public object Frame { get; set; }

    [JsonPropertyName("allianceid")]
    public object AllianceId { get; set; }

    [JsonPropertyName("tag")]
    public string Tag { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("emblem")]
    public int? Emblem { get; set; }

    [JsonPropertyName("ajoined")]
    public DateTime? AllianceJoined { get; set; }

    [JsonPropertyName("server")]
    public int? Server { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("power")]
    public long? Power { get; set; }

    [JsonPropertyName("max_power")]
    public long? MaxPower { get; set; }

    [JsonPropertyName("pd")]
    public object PowerDestroyed { get; set; }

    [JsonPropertyName("arr")]
    public int? ArenaRating { get; set; }

    [JsonPropertyName("ass")]
    public object AssessmentRank { get; set; }

    [JsonPropertyName("rss")]
    public object RssRaided { get; set; }

    [JsonPropertyName("mcomplete")]
    public int? MissionsComplete { get; set; }

    [JsonPropertyName("ahelps")]
    public int? AllianceHelps { get; set; }

    [JsonPropertyName("rssmined")]
    public object RssMined { get; set; }

    [JsonPropertyName("hdestroyed")]
    public int? HostilesDestroyed { get; set; }

    [JsonPropertyName("pdamage")]
    public object PlayerDamage { get; set; }

    [JsonPropertyName("hdamage")]
    public object HostileDamage { get; set; }

    [JsonPropertyName("kdr")]
    public double? Kdr { get; set; }

    [JsonPropertyName("pdestroyed")]
    public int? PlayerShipsDestroyed { get; set; }

    [JsonPropertyName("created")]
    public DateTime PlayerCreated { get; set; }

    [JsonPropertyName("lastmodified")]
    public DateTime LastModified { get; set; }



    // Tournament related properties

    [JsonPropertyName("score")]
    public int? TournamentScore { get; set; }

    [JsonPropertyName("position")]
    public int? TournamentPosition { get; set; }

    [JsonPropertyName("dailyCompleted")]
    public int? DailyCompleted { get; set; }

    [JsonPropertyName("totalTasks")]
    public int? TotalTasks { get; set; }

    [JsonPropertyName("completedTasks")]
    public int? CompletedTasks { get; set; }

    [JsonPropertyName("assignedTask")]
    public int? AssignedTask { get; set; }

    [JsonPropertyName("activeTask")]
    public string ActiveTask { get; set; }

    [JsonPropertyName("activeTaskDesc")]
    public string ActiveTaskDesc { get; set; }

    [JsonPropertyName("tasks")]
    public string Tasks { get; set; }

    [JsonPropertyName("dailyTasks")]
    public string DailyTasks { get; set; }

}
