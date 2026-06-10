using KampusGuvenlikAI.Core.Models;
using Newtonsoft.Json;

namespace KampusGuvenlikAI.Core.Services;

public class SettingsService
{
    public string SettingsFilePath { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KampusGuvenlikAI",
        "settings.json");

    public AppSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
            {
                var defaults = new AppSettings();
                Save(defaults);
                return defaults;
            }

            var json = File.ReadAllText(SettingsFilePath);
            return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            return new AppSettings();
        }
    }

    public void Save(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFilePath, json);
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            throw;
        }
    }
}
