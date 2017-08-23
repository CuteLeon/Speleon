using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Client
{
    public partial class LoginForm : Form
    {
        private enum HideTo
        {
            Min,
            Close
        }

        public LoginForm()
        {
            CheckForIllegalCrossThreadCalls = false;

            this.Icon = UnityResource.Speleon;
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            TitleLabel.Image = UnityResource.Speleon.ToBitmap();
            TitleLabel.Parent = ActiveBGIBOX;
            MinButton.Parent = ActiveBGIBOX;
            ClockButton.Parent = ActiveBGIBOX;
        }

        private void MinButton_Click(object sender, EventArgs e)
        {
            HideMe(HideTo.Min);
        }

        private void ClockButton_Click(object sender, EventArgs e)
        {
            HideMe(HideTo.Close);
        }

        private void HideMe(HideTo hideTo)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                while (this.Opacity>0)
                {
                    this.Opacity -= 0.1;
                    this.Top -= 1;
                    Thread.Sleep(20);
                }

                if (hideTo == HideTo.Min)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Opacity = 1.0;
                }
                else if (hideTo == HideTo.Close)
                    Application.Exit();
            }));
        }

    }
}

