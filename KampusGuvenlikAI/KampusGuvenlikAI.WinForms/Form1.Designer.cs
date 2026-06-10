using System.Drawing;
using System.Windows.Forms;

namespace KampusGuvenlikAI.WinForms
{
    public partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // KRİTİK TANIMLAMALAR: Ana kodun erişebilmesi için public ve kesin isimlerle tanımlıyoruz
        public System.Windows.Forms.Button btnNewReport;
        public System.Windows.Forms.Button btnReports;
        public System.Windows.Forms.Button btnSettings;
        public System.Windows.Forms.Label titleLabel;
        public System.Windows.Forms.Label subtitleLabel;
        public System.Windows.Forms.Label lblWeather;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.lblWeather = new System.Windows.Forms.Label();
            this.btnNewReport = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.FromArgb(34, 59, 92);
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(50, 90);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(1329, 60);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Kampüs Güvenlik Yönetim Sistemi";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.subtitleLabel.ForeColor = System.Drawing.Color.White;
            this.subtitleLabel.Location = new System.Drawing.Point(50, 180);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(1329, 80);
            this.subtitleLabel.TabIndex = 1;
            this.subtitleLabel.Text = "Yapay Zeka Destekli Olay Analiz ve Sınıflandırma Modülü.\r\nSistem üzerinden kampüs içi güvenlik olaylarını bildirebilir, yapay zeka analiziyle aciliyet durumlarını raporlayabilirsiniz.";
            this.subtitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWeather
            // 
            this.lblWeather.BackColor = System.Drawing.Color.FromArgb(150, 34, 59, 92);
            this.lblWeather.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWeather.ForeColor = System.Drawing.Color.White;
            this.lblWeather.Location = new System.Drawing.Point(1080, 20);
            this.lblWeather.Name = "lblWeather";
            this.lblWeather.Padding = new System.Windows.Forms.Padding(5);
            this.lblWeather.Size = new System.Drawing.Size(250, 45);
            this.lblWeather.TabIndex = 5;
            this.lblWeather.Text = "☀️ Karaman: Yükleniyor...";
            this.lblWeather.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNewReport
            // 
            this.btnNewReport.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnNewReport.Location = new System.Drawing.Point(450, 470);
            this.btnNewReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNewReport.Name = "btnNewReport";
            this.btnNewReport.Size = new System.Drawing.Size(529, 97);
            this.btnNewReport.TabIndex = 2;
            this.btnNewReport.Text = "Yeni Olay Raporu Gir";
            this.btnNewReport.UseVisualStyleBackColor = false;
            // 
            // btnReports
            // 
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnReports.Location = new System.Windows.Forms.Point(450, 596);
            this.btnReports.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(529, 97);
            this.btnReports.TabIndex = 3;
            this.btnReports.Text = "Geçmiş Raporları Listele && Karşılaştır";
            this.btnReports.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnSettings.Location = new System.Drawing.Point(1350, 20);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(45, 45);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "⚙";
            this.btnSettings.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(16, 28, 48);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1429, 1050);

            // GARANTİLİ DÜZELTME: Butonların ve yazıların forma eklenmesini sağlayan kritik satırlar
            this.Controls.Add(this.lblWeather);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnNewReport);
            this.Controls.Add(this.subtitleLabel);
            this.Controls.Add(this.titleLabel);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ana Menü - Kampüs Güvenlik AI";
            this.ResumeLayout(false);
        }

        #endregion
    }
}