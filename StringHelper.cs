using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace STFCTools.PlayerHighScore
{
    internal static class StringHelper
    {
        private static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };

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

        internal static string JsonPrettify(this string json)
        {
            using var jDoc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(jDoc, CachedJsonSerializerOptions);
        }
    }
}
