namespace STFCTools.PlayerHighScore.Models;

/// <summary>
/// Name, Power
/// </summary>
internal class PlayerData
{
    public string PlayerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Power { get; set; } = string.Empty;
    public string CompletedTasks { get; set; } = string.Empty;
    public string StartedTasks { get; set; } = string.Empty;
    public string TornamentScore { get; set; } = string.Empty;
}
