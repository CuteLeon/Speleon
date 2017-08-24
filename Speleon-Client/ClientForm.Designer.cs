namespace Speleon_Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TitlePanel = new Speleon_Client.MyPanel();
            this.MinButton = new Speleon_Client.LabelButton();
            this.MaxButton = new Speleon_Client.LabelButton();
            this.RestoreButton = new Speleon_Client.LabelButton();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Label();
            this.TitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.LightGray;
            this.TitlePanel.Controls.Add(this.MinButton);
            this.TitlePanel.Controls.Add(this.MaxButton);
            this.TitlePanel.Controls.Add(this.RestoreButton);
            this.TitlePanel.Controls.Add(this.TitleLabel);
            this.TitlePanel.Controls.Add(this.CloseButton);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Size = new System.Drawing.Size(640, 85);
            this.TitlePanel.TabIndex = 0;
            // 
            // MinButton
            // 
            this.MinButton.BackColor = System.Drawing.Color.Transparent;
            this.MinButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MinButton.Image = global::Speleon_Client.UnityResource.Min_0;
            this.MinButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MinButton.Location = new System.Drawing.Point(505, 0);
            this.MinButton.Margin = new System.Windows.Forms.Padding(0);
            this.MinButton.Name = "MinButton";
            this.MinButton.Size = new System.Drawing.Size(32, 85);
            this.MinButton.TabIndex = 9;
            this.MinButton.Tag = "Min";
            this.MinButton.Click += new System.EventHandler(this.MinButton_Click);
            // 
            // MaxButton
            // 
            this.MaxButton.BackColor = System.Drawing.Color.Transparent;
            this.MaxButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MaxButton.Image = global::Speleon_Client.UnityResource.Max_0;
            this.MaxButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MaxButton.Location = new System.Drawing.Point(537, 0);
            this.MaxButton.Margin = new System.Windows.Forms.Padding(0);
            this.MaxButton.Name = "MaxButton";
            this.MaxButton.Size = new System.Drawing.Size(32, 85);
            this.MaxButton.TabIndex = 8;
            this.MaxButton.Tag = "Max";
            this.MaxButton.Click += new System.EventHandler(this.MaxButton_Click);
            // 
            // RestoreButton
            // 
            this.RestoreButton.BackColor = System.Drawing.Color.Transparent;
            this.RestoreButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.RestoreButton.Image = global::Speleon_Client.UnityResource.Restore_0;
            this.RestoreButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RestoreButton.Location = new System.Drawing.Point(569, 0);
            this.RestoreButton.Margin = new System.Windows.Forms.Padding(0);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(32, 85);
            this.RestoreButton.TabIndex = 7;
            this.RestoreButton.Tag = "Restore";
            this.RestoreButton.Visible = false;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // TitleLabel
            // 
            this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.TitleLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(113, 85);
            this.TitleLabel.TabIndex = 6;
            this.TitleLabel.Text = "Speleon";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseButton.Image = global::Speleon_Client.UnityResource.Close_0;
            this.CloseButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CloseButton.Location = new System.Drawing.Point(601, 0);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(39, 85);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Tag = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 500);
            this.Controls.Add(this.TitlePanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Speleon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.Shown += new System.EventHandler(this.ClientForm_Shown);
            this.Resize += new System.EventHandler(this.ClientForm_Resize);
            this.TitlePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyPanel TitlePanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label CloseButton;
        private LabelButton RestoreButton;
        private LabelButton MaxButton;
        private LabelButton MinButton;
    }
}