using System;
using System.IO;
using Newtonsoft.Json;

namespace KampusGuvenlikAI.Core.Services
{
    /// <summary>
    /// Ayarlar Yöneticisi - Kullanıcı ayarlarını kaydeder ve yükler
    /// </summary>
    public class SettingsManager
    {
        private readonly string _settingsPath;
        private readonly Logger _logger;

        public class AppSettings
        {
            [JsonProperty("openweathermap_api_key")]
            public string OpenWeatherMapApiKey { get; set; }

            [JsonProperty("openai_api_key")]
            public string OpenAiApiKey { get; set; }

            [JsonProperty("openai_model")]
            public string OpenAiModel { get; set; } = "gpt-3.5-turbo";

            [JsonProperty("sqlite_db_path")]
            public string SqliteDbPath { get; set; }

            [JsonProperty("use_fallback_analysis")]
            public bool UseFallbackAnalysis { get; set; } = true;
        }

        public AppSettings CurrentSettings { get; private set; }

        public SettingsManager()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KampusGuvenlikAI"
            );

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            _settingsPath = Path.Combine(appDataPath, "settings.json");
            _logger = new Logger();

            LoadSettings();
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    string json = File.ReadAllText(_settingsPath);
                    CurrentSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                }
                else
                {
                    CurrentSettings = new AppSettings
                    {
                        SqliteDbPath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "KampusGuvenlikAI",
                            "incidents.db"
                        )
                    };
                    SaveSettings();
                }

                _logger.LogInfo("Ayarlar yüklendi");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ayarlar yükleme hatası: {ex.Message}", ex);
                CurrentSettings = new AppSettings();
            }
        }

        public void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(CurrentSettings, Formatting.Indented);
                File.WriteAllText(_settingsPath, json);
                _logger.LogInfo("Ayarlar kaydedildi");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ayarlar kaydetme hatası: {ex.Message}", ex);
            }
        }
    }
}
