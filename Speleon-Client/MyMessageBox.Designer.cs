namespace Speleon_Client
{
    partial class MyMessageBox
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.IconLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Label();
            this.MessageBoxPanel = new Speleon_Client.MyPanel();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new Speleon_Client.LabelButton();
            this.MessageBoxPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TitleLabel.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.TitleLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.TitleLabel.Location = new System.Drawing.Point(1, 1);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.TitleLabel.Size = new System.Drawing.Size(398, 32);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Speleon";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MessageLabel
            // 
            this.MessageLabel.BackColor = System.Drawing.Color.White;
            this.MessageLabel.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MessageLabel.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.MessageLabel.Location = new System.Drawing.Point(85, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.MessageLabel.Size = new System.Drawing.Size(313, 104);
            this.MessageLabel.TabIndex = 1;
            this.MessageLabel.Text = "Speleon";
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IconLabel
            // 
            this.IconLabel.BackColor = System.Drawing.Color.White;
            this.IconLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IconLabel.Location = new System.Drawing.Point(0, 0);
            this.IconLabel.Name = "IconLabel";
            this.IconLabel.Size = new System.Drawing.Size(85, 114);
            this.IconLabel.TabIndex = 2;
            // 
            // OKButton
            // 
            this.OKButton.BackColor = System.Drawing.Color.White;
            this.OKButton.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OKButton.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.OKButton.Image = global::Speleon_Client.UnityResource.DefaultButton_0;
            this.OKButton.Location = new System.Drawing.Point(100, 101);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(108, 44);
            this.OKButton.TabIndex = 4;
            this.OKButton.Tag = "DefaultButton";
            this.OKButton.Text = "确定";
            this.OKButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.BackColor = System.Drawing.Color.White;
            this.CancelButton.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelButton.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.CancelButton.Image = global::Speleon_Client.UnityResource.DefaultButton_0;
            this.CancelButton.Location = new System.Drawing.Point(246, 101);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(108, 44);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Tag = "DefaultButton";
            this.CancelButton.Text = "取消";
            this.CancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MessageBoxPanel
            // 
            this.MessageBoxPanel.BackColor = System.Drawing.Color.White;
            this.MessageBoxPanel.Controls.Add(this.CancelButton);
            this.MessageBoxPanel.Controls.Add(this.OKButton);
            this.MessageBoxPanel.Controls.Add(this.InputTextBox);
            this.MessageBoxPanel.Controls.Add(this.IconLabel);
            this.MessageBoxPanel.Controls.Add(this.MessageLabel);
            this.MessageBoxPanel.Location = new System.Drawing.Point(1, 33);
            this.MessageBoxPanel.Name = "MessageBoxPanel";
            this.MessageBoxPanel.Size = new System.Drawing.Size(398, 146);
            this.MessageBoxPanel.TabIndex = 6;
            // 
            // InputTextBox
            // 
            this.InputTextBox.AllowDrop = true;
            this.InputTextBox.BackColor = System.Drawing.Color.Gainsboro;
            this.InputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InputTextBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InputTextBox.Location = new System.Drawing.Point(91, 75);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(296, 22);
            this.InputTextBox.TabIndex = 6;
            this.InputTextBox.Visible = false;
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CloseButton.Image = global::Speleon_Client.UnityResource.Close_0;
            this.CloseButton.Location = new System.Drawing.Point(360, 1);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(39, 21);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.Tag = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MyMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.MessageBoxPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MyMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "智能家居解决方案";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyMessageBox_FormClosing);
            this.Load += new System.EventHandler(this.MyMessageBox_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MyMessageBox_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyMessageBox_KeyDown);
            this.MessageBoxPanel.ResumeLayout(false);
            this.MessageBoxPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.Label IconLabel;
        private System.Windows.Forms.Label OKButton;
        private new System.Windows.Forms.Label CancelButton;
        private Speleon_Client.MyPanel MessageBoxPanel;
        private System.Windows.Forms.TextBox InputTextBox;
        private LabelButton CloseButton;
    }
}