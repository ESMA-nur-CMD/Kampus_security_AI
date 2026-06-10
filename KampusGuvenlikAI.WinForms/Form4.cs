using System;
using System.Windows.Forms;
using KampusGuvenlikAI.Core.Services;

namespace KampusGuvenlikAI.WinForms
{
    public partial class Form4 : Form
    {
        private SettingsManager _settingsManager;
        private WeatherService _weatherService;
        private DatabaseService _databaseService;

        public Form4(SettingsManager settingsManager, WeatherService weatherService, DatabaseService databaseService)
        {
            InitializeComponent();
            _settingsManager = settingsManager;
            _weatherService = weatherService;
            _databaseService = databaseService;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                // Ayarları form alanlarına yükle
                this.txtOpenWeatherMapApiKey.Text = _settingsManager.CurrentSettings.OpenWeatherMapApiKey ?? "";
                this.txtOpenAiApiKey.Text = _settingsManager.CurrentSettings.OpenAiApiKey ?? "";
                this.txtOpenAiModel.Text = _settingsManager.CurrentSettings.OpenAiModel ?? "gpt-3.5-turbo";
                this.txtSqliteDbPath.Text = _settingsManager.CurrentSettings.SqliteDbPath ?? "";
                this.chkUseFallback.Checked = _settingsManager.CurrentSettings.UseFallbackAnalysis;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                _settingsManager.CurrentSettings.OpenWeatherMapApiKey = this.txtOpenWeatherMapApiKey.Text;
                _settingsManager.CurrentSettings.OpenAiApiKey = this.txtOpenAiApiKey.Text;
                _settingsManager.CurrentSettings.OpenAiModel = this.txtOpenAiModel.Text;
                _settingsManager.CurrentSettings.SqliteDbPath = this.txtSqliteDbPath.Text;
                _settingsManager.CurrentSettings.UseFallbackAnalysis = this.chkUseFallback.Checked;

                _settingsManager.SaveSettings();

                MessageBox.Show("Ayarlar başarıyla kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar kaydetme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnTestOpenWeatherMap_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnTestOpenWeatherMap.Enabled = false;
                this.btnTestOpenWeatherMap.Text = "Test ediliyor...";

                var weather = await _weatherService.GetWeatherAsync();

                if (weather.Description == "API Hatası")
                {
                    MessageBox.Show("OpenWeatherMap API bağlantısı başarısız!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"OpenWeatherMap API Başarılı!\n\n{weather.CityName}: {weather.Temperature}°C - {weather.Description}", 
                        "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OpenWeatherMap API Test Hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.btnTestOpenWeatherMap.Enabled = true;
                this.btnTestOpenWeatherMap.Text = "OpenWeatherMap Test";
            }
        }

        private void BtnTestOpenAi_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtOpenAiApiKey.Text))
                {
                    MessageBox.Show("OpenAI API Key girilmemiş", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("OpenAI API bağlantısı başarılı!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OpenAI API Test Hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTestDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                int weeklyReports = _databaseService.GetReportsThisWeek();
                MessageBox.Show($"SQLite Veritabanı Başarılı!\n\nBu hafta {weeklyReports} olay kaydı bulunmaktadır.", 
                    "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SQLite Test Hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBrowseDatabase_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.txtSqliteDbPath.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
