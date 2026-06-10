using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace KampusGuvenlikAI.WinForms;

public class Form4 : Form
{
    private readonly TextBox txtApiKey = new();
    private readonly TextBox txtApiUrl = new();
    private readonly TextBox txtModel = new();
    private readonly TextBox txtDatabasePath = new();
    private readonly CheckBox chkFallback = new();

    public Form4()
    {
        InitializeComponent();
        LoadSettingsToForm();
    }

    public static string SettingsFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KampusGuvenlikAI",
        "settings.json");

    public static AppSettings LoadAppSettings()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
            {
                var defaults = new AppSettings();
                SaveAppSettings(defaults);
                return defaults;
            }

            var json = File.ReadAllText(SettingsFilePath);
            return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void SaveAppSettings(AppSettings settings)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(SettingsFilePath, json);
    }

    private void InitializeComponent()
    {
        Text = "Sistem Ayarları";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(780, 460);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.FromArgb(245, 247, 250);

        var title = new Label
        {
            Text = "API ve Veri Tabanı Ayarları",
            Location = new Point(24, 22),
            Size = new Size(420, 34),
            Font = new Font("Segoe UI", 16F, FontStyle.Bold)
        };

        AddField("API Key", txtApiKey, 24, 84, password: true);
        AddField("API URL", txtApiUrl, 24, 144);
        AddField("Model", txtModel, 24, 204);
        AddField("SQLite DB Path", txtDatabasePath, 24, 264);

        chkFallback.Text = "API hatasında yerel kural tabanlı analiz kullan";
        chkFallback.Location = new Point(160, 324);
        chkFallback.Size = new Size(420, 28);

        var btnSave = new Button
        {
            Text = "Kaydet",
            Location = new Point(160, 370),
            Size = new Size(130, 38)
        };
        btnSave.Click += BtnSave_Click;

        var btnTestDb = new Button
        {
            Text = "DB Test",
            Location = new Point(310, 370),
            Size = new Size(130, 38)
        };
        btnTestDb.Click += BtnTestDb_Click;

        Controls.AddRange([title, chkFallback, btnSave, btnTestDb]);
    }

    private void LoadSettingsToForm()
    {
        var settings = LoadAppSettings();
        txtApiKey.Text = settings.ApiKey;
        txtApiUrl.Text = settings.ApiUrl;
        txtModel.Text = settings.Model;
        txtDatabasePath.Text = settings.DatabasePath;
        chkFallback.Checked = settings.UseFallback;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtApiUrl.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtDatabasePath.Text))
            {
                MessageBox.Show("API URL, model ve veri tabanı yolu boş olamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var settings = new AppSettings
            {
                ApiKey = txtApiKey.Text.Trim(),
                ApiUrl = txtApiUrl.Text.Trim(),
                Model = txtModel.Text.Trim(),
                DatabasePath = txtDatabasePath.Text.Trim(),
                UseFallback = chkFallback.Checked
            };

            SaveAppSettings(settings);
            MessageBox.Show("Ayarlar başarıyla kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch
        {
            MessageBox.Show("Ayarlar kaydedilirken hata oluştu!", "Ayar Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnTestDb_Click(object? sender, EventArgs e)
    {
        try
        {
            Form2.InitializeDatabase(txtDatabasePath.Text.Trim());
            using var connection = new SqliteConnection($"Data Source={txtDatabasePath.Text.Trim()}");
            connection.Open();
            MessageBox.Show("Veri tabanı bağlantısı başarılı.", "DB Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch
        {
            MessageBox.Show("Veri tabanı bağlantı testi başarısız oldu!", "DB Test Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddField(string labelText, TextBox textBox, int x, int y, bool password = false)
    {
        var label = new Label
        {
            Text = labelText,
            Location = new Point(x, y),
            Size = new Size(130, 24)
        };

        textBox.Location = new Point(x + 136, y - 2);
        textBox.Size = new Size(560, 28);
        textBox.UseSystemPasswordChar = password;

        Controls.Add(label);
        Controls.Add(textBox);
    }
}

public class AppSettings
{
    public string ApiKey { get; set; } = "";
    public string ApiUrl { get; set; } = "https://api.openai.com/v1/chat/completions";
    public string Model { get; set; } = "gpt-4o-mini";
    public string DatabasePath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KampusGuvenlikAI",
        "campus_security.db");
    public bool UseFallback { get; set; } = true;
}
