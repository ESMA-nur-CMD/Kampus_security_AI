using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace KampusGuvenlikAI.WinForms
{
    public partial class Form1 : Form
    {
        // Karaman Hava Durumu API Key
        private const string ApiKey = "55fb49aa7863335ad57d050b86097d07";

        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            // Olayları (Event) kod tarafından güvenle ve temizce bağlıyoruz
            this.Shown += Form1_Shown;

            if (btnNewReport != null) btnNewReport.Click += BtnNewReport_Click;
            if (btnReports != null) btnReports.Click += BtnReports_Click;
            if (btnSettings != null) btnSettings.Click += BtnSettings_Click;
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            // Tasarım ekranından elinle yüklediğin arka plan resmini korur ve butonları süsler
            StyleMenuButtons();

            // Hava durumunu çekme asenkron görevini tetikliyoruz
            await LoadWeatherDataAsync();
        }

        private async Task LoadWeatherDataAsync()
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q=Karaman,TR&appid={ApiKey}&units=metric&lang=tr";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string jsonResponse = await client.GetStringAsync(url);
                    JObject data = JObject.Parse(jsonResponse);

                    double temp = Math.Round((double)data["main"]["temp"], 1);
                    string description = data["weather"][0]["description"].ToString();
                    string mainCondition = data["weather"][0]["main"].ToString().ToLower();

                    string weatherEmoji = "☀️";
                    if (mainCondition.Contains("cloud")) weatherEmoji = "☁️";
                    else if (mainCondition.Contains("rain") || mainCondition.Contains("drizzle")) weatherEmoji = "🌧️";
                    else if (mainCondition.Contains("snow")) weatherEmoji = "❄️";
                    else if (mainCondition.Contains("thunder")) weatherEmoji = "⛈️";

                    description = char.ToUpper(description[0]) + description.Substring(1);

                    lblWeather.Text = $"{weatherEmoji} Karaman: {temp}°C - {description}";
                }
            }
            catch (Exception)
            {
                lblWeather.Text = "⚠️ Hava durumu güncellenemedi";
            }
        }

        private void StyleMenuButtons()
        {
            ApplyButtonStyle(btnNewReport);
            ApplyButtonStyle(btnReports);

            if (btnSettings != null)
            {
                ApplyButtonStyle(btnSettings);
                btnSettings.Size = new Size(45, 45);
                btnSettings.Text = "⚙";
                btnSettings.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
                btnSettings.BackColor = Color.FromArgb(34, 59, 92);

                btnSettings.Location = new Point(this.ClientSize.Width - btnSettings.Width - 20, 20);
                btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnSettings.BringToFront();
            }
        }

        private static void ApplyButtonStyle(Button button)
        {
            if (button == null) return;

            button.BackColor = Color.FromArgb(34, 59, 92);
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button.FlatAppearance.BorderColor = Color.FromArgb(100, 255, 255, 255);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 43, 72);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(48, 82, 125);
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.White;
            button.UseVisualStyleBackColor = false;
        }

        private void BtnNewReport_Click(object sender, EventArgs e)
        {
            using (Form2 form = new Form2())
            {
                form.ShowDialog(this);
            }
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            using (Form3 form = new Form3())
            {
                form.ShowDialog(this);
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            using (Form4 form = new Form4())
            {
                form.ShowDialog(this);
            }
        }
    }
}