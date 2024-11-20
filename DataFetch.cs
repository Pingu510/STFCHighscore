using STFCTools.PlayerHighScore.DTO.STFC.WTF;
using System.IO.Compression;
using System.Text;

namespace STFCTools.PlayerHighScore;

internal static class DataFetch
{
	internal static async Task<string> GetPageContent(string pageUrl)
	{
		using var client = new HttpClient();
		return await client.GetStringAsync(pageUrl);
	}

	internal static PlayerDataCollection PlayerDataFromJson(string playerData)
	{
		var c = new PlayerDataCollection();

		var players = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Player>>(playerData);
		if (players == null) return c;

		c.CreationDate = players.First().Lastmodified;
		foreach (var player in players)
		{
			c.Players.Add(new PlayerData()
			{
				Rank = player.Rank.ToString(),
				Name = player.Owner ?? string.Empty,
				Level = player.Level.ToString(),
				Power = player.MaxPower.ToString(),
				Alliance = player.Tag
			});
		}

		return c;
	}

	internal static PlayerPowerDump? PlayerPowerDumpFromJson(string playerData)
	{
		return System.Text.Json.JsonSerializer.Deserialize<PlayerPowerDump>(playerData);
	}

	internal static string DecodeString(string encodedStr)
	{
		// Step 1: Base64 decode
		byte[] compressedData = Convert.FromBase64String(encodedStr);

		// Step 2: Zlib decompression
		byte[] decompressedData = DecompressZlib(compressedData);

		// Step 3: Convert decompressed data to a UTF-8 string
		return Encoding.UTF8.GetString(decompressedData);
	}

	static byte[] DecompressZlib(byte[] compressedData)
	{
		using MemoryStream input = new(compressedData);

		// Skip the first two bytes (zlib header)
		input.ReadByte(); // CMF (Compression Method and Flags)
		input.ReadByte(); // FLG (Flags)

		using DeflateStream deflateStream = new(input, CompressionMode.Decompress);
		using MemoryStream output = new();
		deflateStream.CopyTo(output);
		return output.ToArray();
	}
}
