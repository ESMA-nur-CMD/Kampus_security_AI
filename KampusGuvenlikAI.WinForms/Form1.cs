using System;
using System.Windows.Forms;
using KampusGuvenlikAI.Core.Services;

namespace KampusGuvenlikAI.WinForms
{
    public partial class Form1 : Form
    {
        private WeatherService _weatherService;
        private DatabaseService _databaseService;
        private SettingsManager _settingsManager;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _settingsManager = new SettingsManager();
                _databaseService = new DatabaseService(_settingsManager.CurrentSettings.SqliteDbPath);
                _weatherService = new WeatherService(_settingsManager.CurrentSettings.OpenWeatherMapApiKey);

                // Hava durumunu yükle
                await LoadWeatherAsync();

                // İstatistikleri güncelle
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Başlatma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task LoadWeatherAsync()
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync();
                
                this.lblWeatherTemp.Text = $"{weather.Temperature}°C";
                this.lblWeatherDesc.Text = weather.Description;
                
                // İkon yükleme (basit metin gösterimi)
                if (weather.Icon.Contains("cloud"))
                    this.picWeatherIcon.Text = "☁️";
                else if (weather.Icon.Contains("rain"))
                    this.picWeatherIcon.Text = "🌧️";
                else if (weather.Icon.Contains("clear") || weather.Icon.Contains("sunny"))
                    this.picWeatherIcon.Text = "☀️";
                else
                    this.picWeatherIcon.Text = "🌡️";
            }
            catch (Exception ex)
            {
                this.lblWeatherTemp.Text = "Yükleme Hatası";
                this.lblWeatherDesc.Text = ex.Message;
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                int weeklyReports = _databaseService.GetReportsThisWeek();
                int criticalReports = _databaseService.GetCriticalReports();
                DateTime lastReportTime = _databaseService.GetLastReportTime();

                this.lblStatsThisWeek.Text = $"Bu hafta: {weeklyReports} olay";
                this.lblStatsCritical.Text = $"Kritik: {criticalReports}";

                if (lastReportTime == DateTime.MinValue)
                {
                    this.lblStatsLastReport.Text = "Son olay: Henüz yok";
                }
                else
                {
                    TimeSpan timeDiff = DateTime.Now - lastReportTime;
                    string timeAgo = timeDiff.TotalHours < 1 
                        ? $"{(int)timeDiff.TotalMinutes} dakika önce"
                        : $"{(int)timeDiff.TotalHours} saat önce";
                    
                    this.lblStatsLastReport.Text = $"Son olay: {timeAgo}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İstatistik yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNewReport_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(_databaseService, _settingsManager);
            form2.ShowDialog();
            UpdateStatistics();
        }

        private void BtnListReports_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(_databaseService);
            form3.ShowDialog();
            UpdateStatistics();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(_settingsManager, _weatherService, _databaseService);
            form4.ShowDialog();
        }
    }
}
