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
            this.labelButton1 = new Speleon_Client.LabelButton();
            this.SuspendLayout();
            // 
            // labelButton1
            // 
            this.labelButton1.Image = global::Speleon_Client.UnityResource.ColorfulButton_0;
            this.labelButton1.Location = new System.Drawing.Point(152, 93);
            this.labelButton1.Name = "labelButton1";
            this.labelButton1.Size = new System.Drawing.Size(233, 87);
            this.labelButton1.TabIndex = 0;
            this.labelButton1.Tag = "ColorfulButton";
            this.labelButton1.Text = "测试";
            this.labelButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelButton1.Click += new System.EventHandler(this.labelButton1_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 521);
            this.Controls.Add(this.labelButton1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Speleon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.Shown += new System.EventHandler(this.ClientForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private LabelButton labelButton1;
    }
}