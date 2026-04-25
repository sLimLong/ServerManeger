using System.IO;
using System.Text.Json;

namespace LHGSM.Services;

public class SettingsService
{
    private const string SettingsFile = "settings.json";

    public string ServerFolder { get; set; } = "";
    public bool AutoRestart { get; set; } = false;

    public static SettingsService Load()
    {
        if (!File.Exists(SettingsFile))
            return new SettingsService();

        var json = File.ReadAllText(SettingsFile);
        return JsonSerializer.Deserialize<SettingsService>(json) ?? new SettingsService();
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsFile, json);
    }
}
