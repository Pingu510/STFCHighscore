namespace STFCTools.PlayerHighScore.Models;

internal class PlayerDataCollection
{
	public DateTime CreationDate { get; set; }
	public List<PlayerData> Players { get; set; }

	public PlayerDataCollection()
	{
		Players = [];
	}
}
