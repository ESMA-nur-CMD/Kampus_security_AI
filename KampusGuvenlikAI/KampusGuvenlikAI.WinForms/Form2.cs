using System.Net.Http.Headers;
using System.Text;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KampusGuvenlikAI.WinForms;

public class Form2 : Form
{
    private readonly RichTextBox txtReport = new();
    private readonly ComboBox cmbHumanLabel = new();
    private readonly TextBox txtAiCategory = new();
    private readonly TextBox txtUrgency = new();
    private readonly TextBox txtStep1 = new();
    private readonly TextBox txtStep2 = new();
    private readonly TextBox txtStep3 = new();
    private readonly Button btnAnalyze = new();
    private readonly Button btnSave = new();

    private string lastCategory = "";
    private string lastUrgency = "";
    private string lastStep1 = "";
    private string lastStep2 = "";
    private string lastStep3 = "";

    private static readonly string[] Categories =
    [
        "Güvenlik",
        "Sağlık",
        "Disiplin",
        "Kayıp Eşya",
        "Yangın",
        "Teknik Arıza",
        "Diğer"
    ];

    public Form2()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Yeni Rapor Giriş ve AI Analiz";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(980, 650);
        MinimumSize = new Size(940, 620);
        BackColor = Color.FromArgb(245, 247, 250);

        var title = CreateLabel("Yeni Olay Raporu", 24, 20, 360, 18, true);
        var reportLabel = CreateLabel("Olay Metni", 24, 78, 220);
        txtReport.Location = new Point(24, 106);
        txtReport.Size = new Size(555, 390);
        txtReport.Font = new Font("Segoe UI", 10F);

        var humanLabel = CreateLabel("HumanLabel (Manuel Kategori)", 610, 78, 300);
        cmbHumanLabel.Location = new Point(610, 106);
        cmbHumanLabel.Size = new Size(300, 28);
        cmbHumanLabel.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbHumanLabel.Items.AddRange(Categories);
        cmbHumanLabel.SelectedIndex = 0;

        AddReadonlyField("AI Kategorisi", txtAiCategory, 610, 158);
        AddReadonlyField("Aciliyet Seviyesi", txtUrgency, 610, 224);
        AddReadonlyField("İlk Adım 1", txtStep1, 610, 290);
        AddReadonlyField("İlk Adım 2", txtStep2, 610, 356);
        AddReadonlyField("İlk Adım 3", txtStep3, 610, 422);

        btnAnalyze.Text = "Analiz Et";
        btnAnalyze.Location = new Point(24, 525);
        btnAnalyze.Size = new Size(170, 42);
        btnAnalyze.Click += BtnAnalyze_Click;

        btnSave.Text = "Kaydet";
        btnSave.Location = new Point(212, 525);
        btnSave.Size = new Size(170, 42);
        btnSave.Click += BtnSave_Click;

        Controls.AddRange([title, reportLabel, txtReport, humanLabel, cmbHumanLabel, btnAnalyze, btnSave]);
    }

    private async void BtnAnalyze_Click(object? sender, EventArgs e)
    {
        var settings = Form4.LoadAppSettings();
        var cleanedText = CleanText(txtReport.Text);

        if (string.IsNullOrWhiteSpace(cleanedText))
        {
            MessageBox.Show("Lütfen olay metnini giriniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnAnalyze.Enabled = false;
            btnAnalyze.Text = "Analiz ediliyor...";

            if (string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                throw new InvalidOperationException("API anahtarı boş.");
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);

            var requestBody = new
            {
                model = settings.Model,
                response_format = new { type = "json_object" },
                messages = new object[]
                {
                    new
                    {
                        role = "system",
                        content = "Kampüs güvenlik olay raporlarını analiz et. Sadece JSON döndür: category, urgency, steps. category: Güvenlik, Sağlık, Disiplin, Kayıp Eşya, Yangın, Teknik Arıza veya Diğer. urgency: Düşük, Orta veya Yüksek. steps üç maddelik string dizisi olsun."
                    },
                    new
                    {
                        role = "user",
                        content = cleanedText
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(settings.ApiUrl, content);
            var responseText = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            FillAnalysisFromApi(responseText);
        }
        catch
        {
            MessageBox.Show("API Hatası oluştu!", "API Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (settings.UseFallback)
            {
                FillAnalysisWithLocalRules(cleanedText);
                MessageBox.Show("Yerel kural tabanlı yedek analiz kullanıldı.", "Yedek Analiz", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        finally
        {
            btnAnalyze.Enabled = true;
            btnAnalyze.Text = "Analiz Et";
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            var reportText = CleanText(txtReport.Text);
            if (string.IsNullOrWhiteSpace(reportText) || string.IsNullOrWhiteSpace(lastCategory))
            {
                MessageBox.Show("Kaydetmeden önce olay metnini girip analiz yapınız.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var settings = Form4.LoadAppSettings();
            InitializeDatabase(settings.DatabasePath);

            using var connection = new SqliteConnection($"Data Source={settings.DatabasePath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                """
                INSERT INTO IncidentReports
                (CreatedAt, ReportText, HumanLabel, AiCategory, Urgency, Step1, Step2, Step3)
                VALUES
                ($CreatedAt, $ReportText, $HumanLabel, $AiCategory, $Urgency, $Step1, $Step2, $Step3);
                """;
            command.Parameters.AddWithValue("$CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("$ReportText", reportText);
            command.Parameters.AddWithValue("$HumanLabel", Convert.ToString(cmbHumanLabel.SelectedItem) ?? "Diğer");
            command.Parameters.AddWithValue("$AiCategory", lastCategory);
            command.Parameters.AddWithValue("$Urgency", lastUrgency);
            command.Parameters.AddWithValue("$Step1", lastStep1);
            command.Parameters.AddWithValue("$Step2", lastStep2);
            command.Parameters.AddWithValue("$Step3", lastStep3);
            command.ExecuteNonQuery();

            MessageBox.Show("Olay raporu başarıyla kaydedildi.", "Kayıt Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }
        catch
        {
            MessageBox.Show("Veri tabanı kayıt hatası oluştu!", "Veri Tabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void FillAnalysisFromApi(string responseText)
    {
        var root = JObject.Parse(responseText);
        var content = root["choices"]?[0]?["message"]?["content"]?.ToString() ?? "{}";
        var result = JObject.Parse(content);
        var steps = result["steps"] as JArray ?? [];

        lastCategory = result["category"]?.ToString() ?? "Diğer";
        lastUrgency = result["urgency"]?.ToString() ?? "Orta";
        lastStep1 = steps.ElementAtOrDefault(0)?.ToString() ?? "Olay yerini güvenli hale getir.";
        lastStep2 = steps.ElementAtOrDefault(1)?.ToString() ?? "İlgili birime haber ver.";
        lastStep3 = steps.ElementAtOrDefault(2)?.ToString() ?? "Raporu sisteme kaydet.";

        ShowAnalysis();
    }

    private void FillAnalysisWithLocalRules(string text)
    {
        lastCategory = "Diğer";
        lastUrgency = "Orta";

        if (ContainsAny(text, "KAVGA", "HIRSIZLIK", "SALDIRI", "TEHDİT", "SİLAH"))
        {
            lastCategory = "Güvenlik";
            lastUrgency = "Yüksek";
        }
        else if (ContainsAny(text, "YARALANMA", "YARALI", "BAYILMA", "AMBULANS", "KANAMA"))
        {
            lastCategory = "Sağlık";
            lastUrgency = "Yüksek";
        }
        else if (ContainsAny(text, "DİSİPLİN", "KURAL", "GÜRÜLTÜ", "KOPYA"))
        {
            lastCategory = "Disiplin";
            lastUrgency = "Orta";
        }
        else if (ContainsAny(text, "KAYIP", "CÜZDAN", "TELEFON", "ÇANTA", "KİMLİK"))
        {
            lastCategory = "Kayıp Eşya";
            lastUrgency = "Düşük";
        }

        lastStep1 = "Olay yerini güvenli hale getir.";
        lastStep2 = "İlgili kampüs birimine haber ver.";
        lastStep3 = "Saat, konum ve tanık bilgilerini rapora ekle.";
        ShowAnalysis();
    }

    private void ShowAnalysis()
    {
        txtAiCategory.Text = lastCategory;
        txtUrgency.Text = lastUrgency;
        txtStep1.Text = lastStep1;
        txtStep2.Text = lastStep2;
        txtStep3.Text = lastStep3;
    }

    private void ClearForm()
    {
        txtReport.Clear();
        txtAiCategory.Clear();
        txtUrgency.Clear();
        txtStep1.Clear();
        txtStep2.Clear();
        txtStep3.Clear();
        lastCategory = "";
        lastUrgency = "";
        lastStep1 = "";
        lastStep2 = "";
        lastStep3 = "";
    }

    public static void InitializeDatabase(string databasePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);
        using var connection = new SqliteConnection($"Data Source={databasePath}");
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS IncidentReports (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CreatedAt TEXT NOT NULL,
                ReportText TEXT NOT NULL,
                HumanLabel TEXT NOT NULL,
                AiCategory TEXT NOT NULL,
                Urgency TEXT NOT NULL,
                Step1 TEXT NOT NULL,
                Step2 TEXT NOT NULL,
                Step3 TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    private static string CleanText(string text)
    {
        return (text ?? "")
            .Trim()
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\t", " ")
            .ToUpperInvariant();
    }

    private static bool ContainsAny(string text, params string[] words)
    {
        return words.Any(word => text.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    private void AddReadonlyField(string labelText, TextBox textBox, int x, int y)
    {
        Controls.Add(CreateLabel(labelText, x, y, 300));
        textBox.Location = new Point(x, y + 26);
        textBox.Size = new Size(300, 28);
        textBox.ReadOnly = true;
        Controls.Add(textBox);
    }

    private static Label CreateLabel(string text, int x, int y, int width, int fontSize = 10, bool bold = false)
    {
        return new Label
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(width, 26),
            Font = new Font("Segoe UI", fontSize, bold ? FontStyle.Bold : FontStyle.Regular)
        };
    }
}
