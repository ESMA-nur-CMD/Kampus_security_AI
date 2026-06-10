namespace KampusGuvenlikAI.WinForms
{
    partial class Form4
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtOpenWeatherMapApiKey = new System.Windows.Forms.TextBox();
            this.txtOpenAiApiKey = new System.Windows.Forms.TextBox();
            this.txtOpenAiModel = new System.Windows.Forms.TextBox();
            this.txtSqliteDbPath = new System.Windows.Forms.TextBox();
            this.chkUseFallback = new System.Windows.Forms.CheckBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnTestOpenWeatherMap = new System.Windows.Forms.Button();
            this.btnTestOpenAi = new System.Windows.Forms.Button();
            this.btnTestDatabase = new System.Windows.Forms.Button();
            this.btnBrowseDatabase = new System.Windows.Forms.Button();
            this.lblOpenWeatherMapApiKey = new System.Windows.Forms.Label();
            this.lblOpenAiApiKey = new System.Windows.Forms.Label();
            this.lblOpenAiModel = new System.Windows.Forms.Label();
            this.lblSqliteDbPath = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // Labels
            this.lblOpenWeatherMapApiKey.Text = "OpenWeatherMap API Key:";
            this.lblOpenWeatherMapApiKey.Location = new System.Drawing.Point(10, 10);
            this.lblOpenWeatherMapApiKey.AutoSize = true;

            this.lblOpenAiApiKey.Text = "OpenAI API Key:";
            this.lblOpenAiApiKey.Location = new System.Drawing.Point(10, 60);
            this.lblOpenAiApiKey.AutoSize = true;

            this.lblOpenAiModel.Text = "OpenAI Model:";
            this.lblOpenAiModel.Location = new System.Drawing.Point(10, 110);
            this.lblOpenAiModel.AutoSize = true;

            this.lblSqliteDbPath.Text = "SQLite DB Path:";
            this.lblSqliteDbPath.Location = new System.Drawing.Point(10, 160);
            this.lblSqliteDbPath.AutoSize = true;

            // TextBoxes
            this.txtOpenWeatherMapApiKey.Location = new System.Drawing.Point(10, 30);
            this.txtOpenWeatherMapApiKey.Size = new System.Drawing.Size(350, 25);
            this.txtOpenWeatherMapApiKey.PasswordChar = '*';

            this.txtOpenAiApiKey.Location = new System.Drawing.Point(10, 80);
            this.txtOpenAiApiKey.Size = new System.Drawing.Size(350, 25);
            this.txtOpenAiApiKey.PasswordChar = '*';

            this.txtOpenAiModel.Location = new System.Drawing.Point(10, 130);
            this.txtOpenAiModel.Size = new System.Drawing.Size(350, 25);

            this.txtSqliteDbPath.Location = new System.Drawing.Point(10, 180);
            this.txtSqliteDbPath.Size = new System.Drawing.Size(350, 25);

            // CheckBox
            this.chkUseFallback.Text = "API hatasında yerel kural tabanlı analiz kullan";
            this.chkUseFallback.Location = new System.Drawing.Point(10, 220);
            this.chkUseFallback.AutoSize = true;

            // Buttons
            this.btnTestOpenWeatherMap.Text = "OpenWeatherMap Test";
            this.btnTestOpenWeatherMap.Location = new System.Drawing.Point(370, 30);
            this.btnTestOpenWeatherMap.Size = new System.Drawing.Size(150, 30);
            this.btnTestOpenWeatherMap.Click += new System.EventHandler(this.BtnTestOpenWeatherMap_Click);

            this.btnTestOpenAi.Text = "OpenAI Test";
            this.btnTestOpenAi.Location = new System.Drawing.Point(370, 80);
            this.btnTestOpenAi.Size = new System.Drawing.Size(150, 30);
            this.btnTestOpenAi.Click += new System.EventHandler(this.BtnTestOpenAi_Click);

            this.btnTestDatabase.Text = "DB Test";
            this.btnTestDatabase.Location = new System.Drawing.Point(370, 180);
            this.btnTestDatabase.Size = new System.Drawing.Size(150, 30);
            this.btnTestDatabase.Click += new System.EventHandler(this.BtnTestDatabase_Click);

            this.btnBrowseDatabase.Text = "Gözat";
            this.btnBrowseDatabase.Location = new System.Drawing.Point(530, 180);
            this.btnBrowseDatabase.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseDatabase.Click += new System.EventHandler(this.BtnBrowseDatabase_Click);

            this.btnSaveSettings.Text = "Kaydet";
            this.btnSaveSettings.Location = new System.Drawing.Point(10, 260);
            this.btnSaveSettings.Size = new System.Drawing.Size(100, 40);
            this.btnSaveSettings.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);

            // Form4
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 320);
            this.Controls.Add(this.lblOpenWeatherMapApiKey);
            this.Controls.Add(this.txtOpenWeatherMapApiKey);
            this.Controls.Add(this.btnTestOpenWeatherMap);
            this.Controls.Add(this.lblOpenAiApiKey);
            this.Controls.Add(this.txtOpenAiApiKey);
            this.Controls.Add(this.btnTestOpenAi);
            this.Controls.Add(this.lblOpenAiModel);
            this.Controls.Add(this.txtOpenAiModel);
            this.Controls.Add(this.lblSqliteDbPath);
            this.Controls.Add(this.txtSqliteDbPath);
            this.Controls.Add(this.btnTestDatabase);
            this.Controls.Add(this.btnBrowseDatabase);
            this.Controls.Add(this.chkUseFallback);
            this.Controls.Add(this.btnSaveSettings);
            this.Name = "Form4";
            this.Text = "Sistem Ayarları";
            this.Load += new System.EventHandler(this.Form4_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtOpenWeatherMapApiKey;
        private System.Windows.Forms.TextBox txtOpenAiApiKey;
        private System.Windows.Forms.TextBox txtOpenAiModel;
        private System.Windows.Forms.TextBox txtSqliteDbPath;
        private System.Windows.Forms.CheckBox chkUseFallback;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnTestOpenWeatherMap;
        private System.Windows.Forms.Button btnTestOpenAi;
        private System.Windows.Forms.Button btnTestDatabase;
        private System.Windows.Forms.Button btnBrowseDatabase;
        private System.Windows.Forms.Label lblOpenWeatherMapApiKey;
        private System.Windows.Forms.Label lblOpenAiApiKey;
        private System.Windows.Forms.Label lblOpenAiModel;
        private System.Windows.Forms.Label lblSqliteDbPath;
    }
}
