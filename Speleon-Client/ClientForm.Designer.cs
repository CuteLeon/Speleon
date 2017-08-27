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
            this.button1 = new System.Windows.Forms.Button();
            this.TitlePanel = new Speleon_Client.MyPanel();
            this.MinButton = new Speleon_Client.LabelButton();
            this.MaxButton = new Speleon_Client.LabelButton();
            this.RestoreButton = new Speleon_Client.LabelButton();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.FriendsFlowPanel = new Speleon_Client.MyFlowLayoutPanel();
            this.TitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(592, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 101);
            this.button1.TabIndex = 10;
            this.button1.Text = "发送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.TitlePanel.Controls.Add(this.MinButton);
            this.TitlePanel.Controls.Add(this.MaxButton);
            this.TitlePanel.Controls.Add(this.RestoreButton);
            this.TitlePanel.Controls.Add(this.TitleLabel);
            this.TitlePanel.Controls.Add(this.CloseButton);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Size = new System.Drawing.Size(749, 67);
            this.TitlePanel.TabIndex = 0;
            // 
            // MinButton
            // 
            this.MinButton.BackColor = System.Drawing.Color.Transparent;
            this.MinButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MinButton.Image = global::Speleon_Client.UnityResource.Min_0;
            this.MinButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MinButton.Location = new System.Drawing.Point(614, 0);
            this.MinButton.Margin = new System.Windows.Forms.Padding(0);
            this.MinButton.MaximumSize = new System.Drawing.Size(32, 21);
            this.MinButton.Name = "MinButton";
            this.MinButton.Size = new System.Drawing.Size(32, 21);
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
            this.MaxButton.Location = new System.Drawing.Point(646, 0);
            this.MaxButton.Margin = new System.Windows.Forms.Padding(0);
            this.MaxButton.MaximumSize = new System.Drawing.Size(32, 21);
            this.MaxButton.Name = "MaxButton";
            this.MaxButton.Size = new System.Drawing.Size(32, 21);
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
            this.RestoreButton.Location = new System.Drawing.Point(678, 0);
            this.RestoreButton.Margin = new System.Windows.Forms.Padding(0);
            this.RestoreButton.MaximumSize = new System.Drawing.Size(32, 21);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(32, 21);
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
            this.TitleLabel.ForeColor = System.Drawing.Color.Gray;
            this.TitleLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.MaximumSize = new System.Drawing.Size(180, 30);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(173, 30);
            this.TitleLabel.TabIndex = 6;
            this.TitleLabel.Text = "Speleon-Client";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseButton.Image = global::Speleon_Client.UnityResource.Close_0;
            this.CloseButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.CloseButton.Location = new System.Drawing.Point(710, 0);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.CloseButton.MaximumSize = new System.Drawing.Size(39, 21);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(39, 21);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Tag = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(344, 268);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(214, 73);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "Hello world .";
            // 
            // FriendsFlowPanel
            // 
            this.FriendsFlowPanel.AutoScroll = true;
            this.FriendsFlowPanel.BackColor = System.Drawing.Color.White;
            this.FriendsFlowPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.FriendsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FriendsFlowPanel.Location = new System.Drawing.Point(0, 67);
            this.FriendsFlowPanel.Name = "FriendsFlowPanel";
            this.FriendsFlowPanel.Size = new System.Drawing.Size(173, 413);
            this.FriendsFlowPanel.TabIndex = 12;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 480);
            this.Controls.Add(this.FriendsFlowPanel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
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
            this.PerformLayout();

        }

        #endregion

        private MyPanel TitlePanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label CloseButton;
        private LabelButton RestoreButton;
        private LabelButton MaxButton;
        private LabelButton MinButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private MyFlowLayoutPanel FriendsFlowPanel;
    }
}