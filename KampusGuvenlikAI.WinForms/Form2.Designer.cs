namespace KampusGuvenlikAI.WinForms
{
    partial class Form2
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
            this.rtxtEventDescription = new System.Windows.Forms.RichTextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnApiTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtAiCategory = new System.Windows.Forms.TextBox();
            this.txtUrgency = new System.Windows.Forms.TextBox();
            this.rtxtFirstThreeSteps = new System.Windows.Forms.RichTextBox();
            this.lblEventDesc = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblAiCategory = new System.Windows.Forms.Label();
            this.lblUrgency = new System.Windows.Forms.Label();
            this.lblSteps = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // Labels
            this.lblEventDesc.Text = "Olay Açıklaması:";
            this.lblEventDesc.Location = new System.Drawing.Point(10, 10);
            this.lblEventDesc.AutoSize = true;

            this.lblCategory.Text = "Kategori:";
            this.lblCategory.Location = new System.Drawing.Point(10, 150);
            this.lblCategory.AutoSize = true;

            this.lblAiCategory.Text = "AI Kategorisi:";
            this.lblAiCategory.Location = new System.Drawing.Point(300, 150);
            this.lblAiCategory.AutoSize = true;

            this.lblUrgency.Text = "Aciliyet:";
            this.lblUrgency.Location = new System.Drawing.Point(10, 200);
            this.lblUrgency.AutoSize = true;

            this.lblSteps.Text = "İlk 3 Adım:";
            this.lblSteps.Location = new System.Drawing.Point(10, 250);
            this.lblSteps.AutoSize = true;

            // TextBox and RichTextBox Controls
            this.rtxtEventDescription.Location = new System.Drawing.Point(10, 30);
            this.rtxtEventDescription.Size = new System.Drawing.Size(500, 100);

            this.cmbCategory.Location = new System.Drawing.Point(10, 170);
            this.cmbCategory.Size = new System.Drawing.Size(150, 25);

            this.txtAiCategory.Location = new System.Drawing.Point(300, 170);
            this.txtAiCategory.Size = new System.Drawing.Size(150, 25);
            this.txtAiCategory.ReadOnly = true;

            this.txtUrgency.Location = new System.Drawing.Point(10, 220);
            this.txtUrgency.Size = new System.Drawing.Size(150, 25);
            this.txtUrgency.ReadOnly = true;

            this.rtxtFirstThreeSteps.Location = new System.Drawing.Point(10, 270);
            this.rtxtFirstThreeSteps.Size = new System.Drawing.Size(500, 80);
            this.rtxtFirstThreeSteps.ReadOnly = true;

            // Buttons
            this.btnAnalyze.Text = "Analiz Et";
            this.btnAnalyze.Location = new System.Drawing.Point(10, 360);
            this.btnAnalyze.Size = new System.Drawing.Size(100, 30);
            this.btnAnalyze.Click += new System.EventHandler(this.BtnAnalyze_Click);

            this.btnApiTest.Text = "API Test";
            this.btnApiTest.Location = new System.Drawing.Point(120, 360);
            this.btnApiTest.Size = new System.Drawing.Size(100, 30);
            this.btnApiTest.Click += new System.EventHandler(this.BtnApiTest_Click);

            this.btnSave.Text = "Kaydet";
            this.btnSave.Location = new System.Drawing.Point(230, 360);
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // Form2
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.lblEventDesc);
            this.Controls.Add(this.rtxtEventDescription);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblAiCategory);
            this.Controls.Add(this.txtAiCategory);
            this.Controls.Add(this.lblUrgency);
            this.Controls.Add(this.txtUrgency);
            this.Controls.Add(this.lblSteps);
            this.Controls.Add(this.rtxtFirstThreeSteps);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.btnApiTest);
            this.Controls.Add(this.btnSave);
            this.Name = "Form2";
            this.Text = "Yeni Rapor Girişi";
            this.Load += new System.EventHandler(this.Form2_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.RichTextBox rtxtEventDescription;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnApiTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtAiCategory;
        private System.Windows.Forms.TextBox txtUrgency;
        private System.Windows.Forms.RichTextBox rtxtFirstThreeSteps;
        private System.Windows.Forms.Label lblEventDesc;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblAiCategory;
        private System.Windows.Forms.Label lblUrgency;
        private System.Windows.Forms.Label lblSteps;
    }
}
