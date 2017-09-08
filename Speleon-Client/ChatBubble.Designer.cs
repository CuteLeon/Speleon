namespace Speleon_Client
{
    partial class ChatBubble
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.HeadPortraitLabel = new System.Windows.Forms.Label();
            this.ChatTimeLabel = new System.Windows.Forms.Label();
            this.MessageContextLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HeadPortraitLabel
            // 
            this.HeadPortraitLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.HeadPortraitLabel.Image = global::Speleon_Client.UnityResource.DefaultHeadProtrait;
            this.HeadPortraitLabel.Location = new System.Drawing.Point(6, 6);
            this.HeadPortraitLabel.Margin = new System.Windows.Forms.Padding(3);
            this.HeadPortraitLabel.MaximumSize = new System.Drawing.Size(56, 48);
            this.HeadPortraitLabel.MinimumSize = new System.Drawing.Size(56, 48);
            this.HeadPortraitLabel.Name = "HeadPortraitLabel";
            this.HeadPortraitLabel.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.HeadPortraitLabel.Size = new System.Drawing.Size(56, 48);
            this.HeadPortraitLabel.TabIndex = 1;
            // 
            // ChatTimeLabel
            // 
            this.ChatTimeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChatTimeLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChatTimeLabel.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.ChatTimeLabel.Location = new System.Drawing.Point(62, 6);
            this.ChatTimeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ChatTimeLabel.Name = "ChatTimeLabel";
            this.ChatTimeLabel.Size = new System.Drawing.Size(132, 28);
            this.ChatTimeLabel.TabIndex = 2;
            this.ChatTimeLabel.Text = "2017/08/30 18:56:03";
            this.ChatTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MessageContextLabel
            // 
            this.MessageContextLabel.AutoSize = true;
            this.MessageContextLabel.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.MessageContextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageContextLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MessageContextLabel.Location = new System.Drawing.Point(62, 34);
            this.MessageContextLabel.Margin = new System.Windows.Forms.Padding(0);
            this.MessageContextLabel.Name = "MessageContextLabel";
            this.MessageContextLabel.Padding = new System.Windows.Forms.Padding(5);
            this.MessageContextLabel.Size = new System.Drawing.Size(123, 30);
            this.MessageContextLabel.TabIndex = 3;
            this.MessageContextLabel.Text = "I\'m your Father.";
            // 
            // ChatBubble
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.MessageContextLabel);
            this.Controls.Add(this.ChatTimeLabel);
            this.Controls.Add(this.HeadPortraitLabel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(200, 58);
            this.Name = "ChatBubble";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(200, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HeadPortraitLabel;
        private System.Windows.Forms.Label ChatTimeLabel;
        private System.Windows.Forms.Label MessageContextLabel;
    }
}
