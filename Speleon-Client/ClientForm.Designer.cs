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
            this.FriendsFlowPanel = new Speleon_Client.MyFlowLayoutPanel();
            this.MainPanel = new Speleon_Client.MyPanel();
            this.ChatSendPanel = new Speleon_Client.MyPanel();
            this.ChatInputTextBox = new System.Windows.Forms.TextBox();
            this.ChatSendButton = new Speleon_Client.LabelButton();
            this.TitlePanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.ChatSendPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.Gainsboro;
            this.TitlePanel.Controls.Add(this.MinButton);
            this.TitlePanel.Controls.Add(this.MaxButton);
            this.TitlePanel.Controls.Add(this.RestoreButton);
            this.TitlePanel.Controls.Add(this.TitleLabel);
            this.TitlePanel.Controls.Add(this.CloseButton);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Size = new System.Drawing.Size(720, 65);
            this.TitlePanel.TabIndex = 0;
            // 
            // MinButton
            // 
            this.MinButton.BackColor = System.Drawing.Color.Transparent;
            this.MinButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MinButton.Image = global::Speleon_Client.UnityResource.Min_0;
            this.MinButton.Location = new System.Drawing.Point(585, 0);
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
            this.MaxButton.Location = new System.Drawing.Point(617, 0);
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
            this.RestoreButton.Location = new System.Drawing.Point(649, 0);
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
            this.TitleLabel.Size = new System.Drawing.Size(180, 30);
            this.TitleLabel.TabIndex = 6;
            this.TitleLabel.Text = "Speleon-Client";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseButton.Image = global::Speleon_Client.UnityResource.Close_0;
            this.CloseButton.Location = new System.Drawing.Point(681, 0);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.CloseButton.MaximumSize = new System.Drawing.Size(39, 21);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(39, 21);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Tag = "Close";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FriendsFlowPanel
            // 
            this.FriendsFlowPanel.AutoScroll = true;
            this.FriendsFlowPanel.BackColor = System.Drawing.Color.White;
            this.FriendsFlowPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.FriendsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FriendsFlowPanel.Location = new System.Drawing.Point(0, 65);
            this.FriendsFlowPanel.Name = "FriendsFlowPanel";
            this.FriendsFlowPanel.Size = new System.Drawing.Size(230, 435);
            this.FriendsFlowPanel.TabIndex = 12;
            this.FriendsFlowPanel.WrapContents = false;
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.ChatSendPanel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(230, 65);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(490, 435);
            this.MainPanel.TabIndex = 16;
            // 
            // ChatSendPanel
            // 
            this.ChatSendPanel.BackColor = System.Drawing.Color.White;
            this.ChatSendPanel.Controls.Add(this.ChatInputTextBox);
            this.ChatSendPanel.Controls.Add(this.ChatSendButton);
            this.ChatSendPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChatSendPanel.Location = new System.Drawing.Point(0, 356);
            this.ChatSendPanel.Name = "ChatSendPanel";
            this.ChatSendPanel.Size = new System.Drawing.Size(490, 79);
            this.ChatSendPanel.TabIndex = 15;
            // 
            // ChatInputTextBox
            // 
            this.ChatInputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChatInputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatInputTextBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChatInputTextBox.Location = new System.Drawing.Point(0, 0);
            this.ChatInputTextBox.MaxLength = 18000;
            this.ChatInputTextBox.Multiline = true;
            this.ChatInputTextBox.Name = "ChatInputTextBox";
            this.ChatInputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatInputTextBox.Size = new System.Drawing.Size(370, 79);
            this.ChatInputTextBox.TabIndex = 14;
            this.ChatInputTextBox.Text = "I am just a test message from  user-88888,now i am on my way to server.cant wait " +
    "anymore.let\'s go.\r\njust do IT.\r\nPain past is pleasure.\r\nnow ... now ... now ...";
            // 
            // ChatSendButton
            // 
            this.ChatSendButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ChatSendButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChatSendButton.ForeColor = System.Drawing.Color.Tomato;
            this.ChatSendButton.Image = global::Speleon_Client.UnityResource.DefaultButton_0;
            this.ChatSendButton.Location = new System.Drawing.Point(370, 0);
            this.ChatSendButton.Name = "ChatSendButton";
            this.ChatSendButton.Size = new System.Drawing.Size(120, 79);
            this.ChatSendButton.TabIndex = 13;
            this.ChatSendButton.Tag = "DefaultButton";
            this.ChatSendButton.Text = "发送";
            this.ChatSendButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ChatSendButton.Click += new System.EventHandler(this.ChatSendButton_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 500);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.FriendsFlowPanel);
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
            this.MainPanel.ResumeLayout(false);
            this.ChatSendPanel.ResumeLayout(false);
            this.ChatSendPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MyPanel TitlePanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label CloseButton;
        private LabelButton RestoreButton;
        private LabelButton MaxButton;
        private LabelButton MinButton;
        private MyFlowLayoutPanel FriendsFlowPanel;
        private MyPanel MainPanel;
        private MyPanel ChatSendPanel;
        private LabelButton ChatSendButton;
        private System.Windows.Forms.TextBox ChatInputTextBox;
    }
}