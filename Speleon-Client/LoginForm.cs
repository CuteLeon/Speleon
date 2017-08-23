using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
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
            InitializeComponent();

            this.Icon = UnityResource.Speleon;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            
            UnityModule.DebugPrint("窗体创建成功");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            TitleLabel.Image = UnityResource.Speleon.ToBitmap();
            TitleLabel.Parent = ActiveBGIBOX;
            MinButton.Parent = ActiveBGIBOX;
            ClockButton.Parent = ActiveBGIBOX;
            LoginAreaLabel.Parent = ActiveBGIBOX;

            this.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            ActiveBGIBOX.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            TitleLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            LoginAreaLabel.MouseDown+= new MouseEventHandler(UnityModule.MoveFormViaMouse);

            UnityModule.DebugPrint("窗体加载成功");
        }

        private void MinButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击最小化按钮");
            HideMe(HideTo.Min);
        }

        private void ClockButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击关闭按钮");
            HideMe(HideTo.Close);
        }

        private void HideMe(HideTo hideTo)
        {
            UnityModule.DebugPrint("开始动态隐藏窗体...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                while (this.Opacity>0)
                {
                    this.Opacity -= 0.1;
                    this.Top -= 2;
                    Thread.Sleep(15);
                }

                if (hideTo == HideTo.Min)
                {
                    UnityModule.DebugPrint("最小化窗体");
                    this.WindowState = FormWindowState.Minimized;
                    this.Opacity = 1.0;
                }
                else if (hideTo == HideTo.Close)
                {
                    UnityModule.DebugPrint("退出程序");
                    AllowToClose = true;
                    Application.Exit();
                }
            }));
        }

        bool AllowToClose=false;
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowToClose)
            {
                e.Cancel = true;
                HideMe(HideTo.Close);
            }
        }

        private void LoginOnButton_Click(object sender, EventArgs e)
        {
            Socket LoginSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LoginSocket.Connect("localhost", 17417);
            LoginSocket.Send(Encoding.ASCII.GetBytes(string.Format("{0}#{1}",UserIDTextBox.Text,PasswordTextBox.Text)));

            byte[] a = new byte[256];
            LoginSocket.Receive(a);
            MessageBox.Show(Encoding.ASCII.GetString(a));
            /*
            LoginSocket.Send(BitConverter.GetBytes(65535));
            while (true)
            {
                byte[] bits = new byte[256];
                int r = fs.Read(bits, 0, bits.Length);
                if (r <= 0) break;
                LoginSocket.Send(bits, r, SocketFlags.None);
            }
            */
            LoginSocket.Close();
        }
    }
}

