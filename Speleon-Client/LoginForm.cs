using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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

            UnityModule.DrawWindowShadow(this);
            UnityModule.DebugPrint("窗体创建成功");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            TitleLabel.Image = UnityResource.Speleon.ToBitmap();
            TitleLabel.Parent = ActiveBGIBOX;
            MinButton.Parent = ActiveBGIBOX;
            CloseButton.Parent = ActiveBGIBOX;
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
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                LoginOnButton.Enabled = false;
                LoginOnButton.Text = "Signing ...";
                try
                {
                    Socket LoginSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    LoginSocket.Connect("localhost", 17417);
                    LoginSocket.Send(Encoding.ASCII.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignIn, Application.ProductVersion, UserIDTextBox.Text, PasswordTextBox.Text)));
                    byte[] SignResultBytes = new byte[1024];
                    LoginSocket.Receive(SignResultBytes);
                    string SignResult = Encoding.ASCII.GetString(SignResultBytes).Trim('\0');

                    UnityModule.DebugPrint("接收到登录结果：{0}", SignResult);

                    if (SignResult.StartsWith(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignInSuccessfully)))
                    {
                        this.Invoke(new Action(()=>{new MyMessageBox("登陆成功，欢迎使用！",MyMessageBox.IconType.Info).ShowDialog(this);}));
                    }
                    else
                    {
                        this.Invoke(new Action(() => { new MyMessageBox("您的密码输入错误，请重试！", MyMessageBox.IconType.Warning).ShowDialog(this); }));
                    }

                    LoginSocket.Close();
                    UnityModule.DebugPrint("验证登录TCP连接已经关闭 ...");
                }
                catch (Exception ex)
                {
                    UnityModule.DebugPrint("登录遇到错误！{0}",ex.Message);
                    this.Invoke(new Action(() =>
                    {
                        if (new MyMessageBox("登录遇到错误，是否重试？\n" + ex.Message, MyMessageBox.IconType.Question).ShowDialog(this) == DialogResult.OK)
                        {

                        }
                    }));
                }

                LoginOnButton.Text = "Sign In";
                LoginOnButton.Enabled = true;
            }));
        }

        private void UserIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限定用户账户输入框尽可接收数字和退格
            if (!char.IsNumber(e.KeyChar) && e.KeyChar!= (char)Keys.Back)
                e.Handled = true;
        }

        private void LoginForm_Paint(object sender, PaintEventArgs e)
        {
            using (Brush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.WhiteSmoke, Color.White, LinearGradientMode.ForwardDiagonal))
            {
                //绘制渐变
                e.Graphics.FillRectangle(linearGradientBrush, this.ClientRectangle);
            }
        }
    }
}

