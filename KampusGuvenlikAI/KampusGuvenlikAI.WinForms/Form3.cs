using System.Data;
using Microsoft.Data.Sqlite;

namespace KampusGuvenlikAI.WinForms;

public class Form3 : Form
{
    private readonly DataGridView grid = new();
    private readonly ComboBox cmbHumanLabel = new();
    private readonly Button btnRefresh = new();
    private readonly Button btnUpdate = new();
    private readonly Button btnDelete = new();

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

    public Form3()
    {
        InitializeComponent();
        LoadReports();
    }

    private void InitializeComponent()
    {
        Text = "Rapor Listeleme ve Karşılaştırma";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1120, 700);
        MinimumSize = new Size(1000, 620);
        BackColor = Color.FromArgb(245, 247, 250);

        var title = new Label
        {
            Text = "Geçmiş Raporlar ve HumanLabel vs AI Karşılaştırma",
            Location = new Point(20, 18),
            Size = new Size(720, 34),
            Font = new Font("Segoe UI", 16F, FontStyle.Bold)
        };

        grid.Location = new Point(20, 68);
        grid.Size = new Size(1060, 500);
        grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        grid.SelectionChanged += Grid_SelectionChanged;

        var humanLabel = new Label
        {
            Text = "Seçili kaydın HumanLabel değeri",
            Location = new Point(20, 590),
            Size = new Size(250, 24)
        };

        cmbHumanLabel.Location = new Point(275, 588);
        cmbHumanLabel.Size = new Size(190, 28);
        cmbHumanLabel.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbHumanLabel.Items.AddRange(Categories);

        btnRefresh.Text = "Yenile";
        btnRefresh.Location = new Point(500, 584);
        btnRefresh.Size = new Size(120, 36);
        btnRefresh.Click += (_, _) => LoadReports();

        btnUpdate.Text = "Güncelle";
        btnUpdate.Location = new Point(635, 584);
        btnUpdate.Size = new Size(120, 36);
        btnUpdate.Click += BtnUpdate_Click;

        btnDelete.Text = "Sil";
        btnDelete.Location = new Point(770, 584);
        btnDelete.Size = new Size(120, 36);
        btnDelete.Click += BtnDelete_Click;

        Controls.AddRange([title, grid, humanLabel, cmbHumanLabel, btnRefresh, btnUpdate, btnDelete]);
    }

    private void LoadReports()
    {
        try
        {
            var settings = Form4.LoadAppSettings();
            Form2.InitializeDatabase(settings.DatabasePath);

            using var connection = new SqliteConnection($"Data Source={settings.DatabasePath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                """
                SELECT
                    Id,
                    CreatedAt AS Tarih,
                    ReportText AS OlayMetni,
                    HumanLabel,
                    AiCategory,
                    Urgency AS Aciliyet,
                    Step1,
                    Step2,
                    Step3,
                    CASE
                        WHEN HumanLabel = AiCategory THEN 'Uyumlu'
                        ELSE 'Farklı'
                    END AS Karsilastirma
                FROM IncidentReports
                ORDER BY Id DESC;
                """;

            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            grid.DataSource = table;

            if (grid.Columns["OlayMetni"] is { } reportColumn)
            {
                reportColumn.Width = 300;
            }
        }
        catch
        {
            MessageBox.Show("Raporlar listelenirken hata oluştu!", "Veri Tabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (grid.CurrentRow == null || !grid.Columns.Contains("HumanLabel"))
        {
            return;
        }

        var humanLabel = Convert.ToString(grid.CurrentRow.Cells["HumanLabel"].Value) ?? "Diğer";
        cmbHumanLabel.SelectedItem = Categories.Contains(humanLabel) ? humanLabel : "Diğer";
    }

    private void BtnUpdate_Click(object? sender, EventArgs e)
    {
        try
        {
            var id = GetSelectedId();
            if (id == 0)
            {
                MessageBox.Show("Lütfen güncellenecek kaydı seçiniz.", "Seçim Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var settings = Form4.LoadAppSettings();
            using var connection = new SqliteConnection($"Data Source={settings.DatabasePath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE IncidentReports SET HumanLabel = $HumanLabel WHERE Id = $Id;";
            command.Parameters.AddWithValue("$HumanLabel", Convert.ToString(cmbHumanLabel.SelectedItem) ?? "Diğer");
            command.Parameters.AddWithValue("$Id", id);
            command.ExecuteNonQuery();

            LoadReports();
            MessageBox.Show("Kayıt başarıyla güncellendi.", "Güncelleme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch
        {
            MessageBox.Show("Kayıt güncellenirken hata oluştu!", "Güncelleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        try
        {
            var id = GetSelectedId();
            if (id == 0)
            {
                MessageBox.Show("Lütfen silinecek kaydı seçiniz.", "Seçim Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var answer = MessageBox.Show("Seçili raporu silmek istiyor musunuz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            var settings = Form4.LoadAppSettings();
            using var connection = new SqliteConnection($"Data Source={settings.DatabasePath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM IncidentReports WHERE Id = $Id;";
            command.Parameters.AddWithValue("$Id", id);
            command.ExecuteNonQuery();

            LoadReports();
        }
        catch
        {
            MessageBox.Show("Kayıt silinirken hata oluştu!", "Silme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private int GetSelectedId()
    {
        if (grid.CurrentRow == null || !grid.Columns.Contains("Id"))
        {
            return 0;
        }

        return Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
    }
}
