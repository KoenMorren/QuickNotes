using System.IO;
using System.Text.Json;

namespace QuickNoteApp.Configuration;

public class ConfigHelper
{
    private const string path = "config.json";

    public Config LoadConfig()
    {
        if (!File.Exists(path))
            return new Config(); // fallback to defaults

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Config>(json) ?? new Config();
    }
}
