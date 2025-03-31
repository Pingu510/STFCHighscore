namespace STFCTools.PlayerHighScore;

internal static class FileHelper
{
    internal static void SaveStringToFile(string path, string data)
    {
        File.WriteAllText(path, data);
    }

    internal static void SaveListToFile(string path, IEnumerable<string> data)
    {
        File.WriteAllLines(path, data);
    }

    internal static string ReadFileToString(string path)
    {
        return File.ReadAllText(path);
    }

    internal static List<string> ReadFileToList(string path)
    {
        return File.ReadAllLines(path).ToList() ?? [];
    }
}
