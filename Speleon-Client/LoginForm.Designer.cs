namespace Speleon_Client
{
    partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ActiveBGIBOX = new System.Windows.Forms.PictureBox();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.MinButton = new Speleon_Client.LabelButton();
            this.ClockButton = new Speleon_Client.LabelButton();
            this.labelButton1 = new Speleon_Client.LabelButton();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveBGIBOX)).BeginInit();
            this.SuspendLayout();
            // 
            // ActiveBGIBOX
            // 
            this.ActiveBGIBOX.BackColor = System.Drawing.Color.White;
            this.ActiveBGIBOX.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActiveBGIBOX.Image = global::Speleon_Client.UnityResource.LoginBGI;
            this.ActiveBGIBOX.Location = new System.Drawing.Point(0, 0);
            this.ActiveBGIBOX.Margin = new System.Windows.Forms.Padding(0);
            this.ActiveBGIBOX.Name = "ActiveBGIBOX";
            this.ActiveBGIBOX.Size = new System.Drawing.Size(500, 260);
            this.ActiveBGIBOX.TabIndex = 0;
            this.ActiveBGIBOX.TabStop = false;
            // 
            // TitleLabel
            // 
            this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.TitleLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TitleLabel.Location = new System.Drawing.Point(2, 2);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(108, 32);
            this.TitleLabel.TabIndex = 3;
            this.TitleLabel.Text = "Speleon";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MinButton
            // 
            this.MinButton.BackColor = System.Drawing.Color.Transparent;
            this.MinButton.Image = global::Speleon_Client.UnityResource.Min_0;
            this.MinButton.Location = new System.Drawing.Point(430, 0);
            this.MinButton.Name = "MinButton";
            this.MinButton.Size = new System.Drawing.Size(32, 21);
            this.MinButton.TabIndex = 2;
            this.MinButton.Tag = "Min";
            this.MinButton.Click += new System.EventHandler(this.MinButton_Click);
            // 
            // ClockButton
            // 
            this.ClockButton.BackColor = System.Drawing.Color.Transparent;
            this.ClockButton.Image = global::Speleon_Client.UnityResource.Close_0;
            this.ClockButton.Location = new System.Drawing.Point(461, 0);
            this.ClockButton.Name = "ClockButton";
            this.ClockButton.Size = new System.Drawing.Size(39, 21);
            this.ClockButton.TabIndex = 1;
            this.ClockButton.Tag = "Close";
            this.ClockButton.Click += new System.EventHandler(this.ClockButton_Click);
            // 
            // labelButton1
            // 
            this.labelButton1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelButton1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.labelButton1.Image = global::Speleon_Client.UnityResource.ColorfulButton_0;
            this.labelButton1.Location = new System.Drawing.Point(156, 262);
            this.labelButton1.Margin = new System.Windows.Forms.Padding(0);
            this.labelButton1.Name = "labelButton1";
            this.labelButton1.Size = new System.Drawing.Size(188, 64);
            this.labelButton1.TabIndex = 4;
            this.labelButton1.Tag = "ColorfulButton";
            this.labelButton1.Text = "Login On";
            this.labelButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 328);
            this.Controls.Add(this.labelButton1);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.MinButton);
            this.Controls.Add(this.ClockButton);
            this.Controls.Add(this.ActiveBGIBOX);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Speleon-Login";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ActiveBGIBOX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ActiveBGIBOX;
        private LabelButton ClockButton;
        private LabelButton MinButton;
        private System.Windows.Forms.Label TitleLabel;
        private LabelButton labelButton1;
    }
}

