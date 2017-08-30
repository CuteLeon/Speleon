namespace Speleon_Client
{
    partial class FriendItem
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
            this.SignatureLabel = new System.Windows.Forms.Label();
            this.NickNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HeadPortraitLabel
            // 
            this.HeadPortraitLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.HeadPortraitLabel.Image = global::Speleon_Client.UnityResource.DefaultHeadProtrait;
            this.HeadPortraitLabel.Location = new System.Drawing.Point(6, 6);
            this.HeadPortraitLabel.Margin = new System.Windows.Forms.Padding(0);
            this.HeadPortraitLabel.MaximumSize = new System.Drawing.Size(48, 48);
            this.HeadPortraitLabel.MinimumSize = new System.Drawing.Size(48, 48);
            this.HeadPortraitLabel.Name = "HeadPortraitLabel";
            this.HeadPortraitLabel.Size = new System.Drawing.Size(48, 48);
            this.HeadPortraitLabel.TabIndex = 0;
            // 
            // SignatureLabel
            // 
            this.SignatureLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SignatureLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SignatureLabel.ForeColor = System.Drawing.Color.DimGray;
            this.SignatureLabel.Location = new System.Drawing.Point(54, 30);
            this.SignatureLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SignatureLabel.Name = "SignatureLabel";
            this.SignatureLabel.Size = new System.Drawing.Size(152, 24);
            this.SignatureLabel.TabIndex = 2;
            this.SignatureLabel.Text = "Signature";
            this.SignatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NickNameLabel
            // 
            this.NickNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NickNameLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NickNameLabel.ForeColor = System.Drawing.Color.Black;
            this.NickNameLabel.Location = new System.Drawing.Point(54, 6);
            this.NickNameLabel.Margin = new System.Windows.Forms.Padding(0);
            this.NickNameLabel.Name = "NickNameLabel";
            this.NickNameLabel.Size = new System.Drawing.Size(152, 24);
            this.NickNameLabel.TabIndex = 4;
            this.NickNameLabel.Text = "NickeName";
            this.NickNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FriendItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.NickNameLabel);
            this.Controls.Add(this.SignatureLabel);
            this.Controls.Add(this.HeadPortraitLabel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FriendItem";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(212, 60);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FriendItem_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label HeadPortraitLabel;
        private System.Windows.Forms.Label SignatureLabel;
        private System.Windows.Forms.Label NickNameLabel;
    }
}
