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
            this.LoginAreaLabel = new System.Windows.Forms.Label();
            this.UserIDTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.MinButton = new Speleon_Client.LabelButton();
            this.CloseButton = new Speleon_Client.LabelButton();
            this.SignInButton = new Speleon_Client.LabelButton();
            ((System.ComponentModel.ISupportInitialize)(this.ActiveBGIBOX)).BeginInit();
            this.SuspendLayout();
            // 
            // ActiveBGIBOX
            // 
            this.ActiveBGIBOX.BackColor = System.Drawing.Color.White;
            this.ActiveBGIBOX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
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
            // LoginAreaLabel
            // 
            this.LoginAreaLabel.BackColor = System.Drawing.Color.Transparent;
            this.LoginAreaLabel.Image = global::Speleon_Client.UnityResource.LoginArea;
            this.LoginAreaLabel.Location = new System.Drawing.Point(52, 64);
            this.LoginAreaLabel.Margin = new System.Windows.Forms.Padding(0);
            this.LoginAreaLabel.Name = "LoginAreaLabel";
            this.LoginAreaLabel.Size = new System.Drawing.Size(396, 158);
            this.LoginAreaLabel.TabIndex = 5;
            // 
            // UserIDTextBox
            // 
            this.UserIDTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.UserIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserIDTextBox.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserIDTextBox.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.UserIDTextBox.Location = new System.Drawing.Point(222, 107);
            this.UserIDTextBox.MaxLength = 10;
            this.UserIDTextBox.Name = "UserIDTextBox";
            this.UserIDTextBox.Size = new System.Drawing.Size(145, 20);
            this.UserIDTextBox.TabIndex = 6;
            this.UserIDTextBox.Text = "88888";
            this.UserIDTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserIDTextBox_KeyPress);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PasswordTextBox.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PasswordTextBox.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.PasswordTextBox.Location = new System.Drawing.Point(222, 148);
            this.PasswordTextBox.MaxLength = 16;
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '~';
            this.PasswordTextBox.Size = new System.Drawing.Size(145, 20);
            this.PasswordTextBox.TabIndex = 7;
            this.PasswordTextBox.Text = "234567";
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
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.Image = global::Speleon_Client.UnityResource.Close_0;
            this.CloseButton.Location = new System.Drawing.Point(461, 0);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(39, 21);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Tag = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SignInButton
            // 
            this.SignInButton.BackColor = System.Drawing.Color.Transparent;
            this.SignInButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SignInButton.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.SignInButton.Image = global::Speleon_Client.UnityResource.ColorfulButton_0;
            this.SignInButton.Location = new System.Drawing.Point(156, 256);
            this.SignInButton.Margin = new System.Windows.Forms.Padding(0);
            this.SignInButton.Name = "SignInButton";
            this.SignInButton.Size = new System.Drawing.Size(188, 72);
            this.SignInButton.TabIndex = 4;
            this.SignInButton.Tag = "ColorfulButton";
            this.SignInButton.Text = "Sign In";
            this.SignInButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 328);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UserIDTextBox);
            this.Controls.Add(this.LoginAreaLabel);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.MinButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ActiveBGIBOX);
            this.Controls.Add(this.SignInButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Speleon-Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LoginForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.ActiveBGIBOX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ActiveBGIBOX;
        private LabelButton CloseButton;
        private LabelButton MinButton;
        private System.Windows.Forms.Label TitleLabel;
        private LabelButton SignInButton;
        private System.Windows.Forms.Label LoginAreaLabel;
        private System.Windows.Forms.TextBox UserIDTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
    }
}

