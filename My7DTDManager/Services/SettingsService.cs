using System.Text.Json;
using My7DTDManager.Models;

namespace My7DTDManager.Services;

public class SettingsService
{
    private const string FileName = "settings.json";

    public AppSettings Load()
    {
        if (!File.Exists(FileName))
            return new AppSettings();

        var json = File.ReadAllText(FileName);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }

    public void Save(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(FileName, json);
    }
}
