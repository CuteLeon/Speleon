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
        
        #region 变量和枚举
        
        /// <summary>
        /// 登录验证 Socket
        /// </summary>
        Socket LoginSocket;

        /// <summary>
        /// 登录线程
        /// </summary>
        Thread LoginThread;
        
        /// <summary>
        /// 动态隐藏后执行的动作
        /// </summary>
        private enum HideTo
        {
            /// <summary>
            /// 最小化
            /// </summary>
            Min,
            /// <summary>
            /// 隐藏
            /// </summary>
            JustHide,
            /// <summary>
            /// 关闭
            /// </summary>
            Close
        }

        #endregion

        #region 标题栏按钮

        private void MinButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击最小化按钮");
            HideMe(HideTo.Min);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击关闭按钮");
            HideMe(HideTo.Close);
        }

        #endregion

        #region 控件事件与功能函数
        /// <summary>
        /// 动态隐藏方法
        /// </summary>
        /// <param name="hideTo">隐藏后的动作</param>
        private void HideMe(HideTo hideTo)
        {
            UnityModule.DebugPrint("开始动态隐藏窗体...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                int IniTop = this.Top;
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
                    this.Top = IniTop;
                }
                else if (hideTo == HideTo.JustHide)
                {
                    this.Hide();
                    this.Opacity = 1.0;
                    this.Top = IniTop;
                    SignInButton.Text = "Sign In";
                }
                else if (hideTo == HideTo.Close)
                {
                    UnityModule.DebugPrint("退出程序");
                    AllowToClose = true;
                    Application.Exit();
                }
            }));
        }

        /// <summary>
        /// 是否正在登录
        /// </summary>
        bool Logining = false;
        private void SignInButton_Click(object sender, EventArgs e)
        {
            if (!Logining)
            {
                //开始登录
                Logining = true;
                LoginThread = new Thread(Login);
                LoginThread.Start();
            }
            else
            {
                //取消登录
                Logining = false;
                LoginSocket?.Close();
                LoginSocket?.Dispose();
                LoginThread?.Abort();
                UserIDTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
                SignInButton.Text = "Sign In";
            }
        }

        private void UserIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限定用户账户输入框尽可接收数字和退格
            if (!char.IsNumber(e.KeyChar) && e.KeyChar!= (char)Keys.Back)
                e.Handled = true;
        }

        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            //更新界面
            this.Invoke(new Action(() => {
                UserIDTextBox.Enabled = false;
                PasswordTextBox.Enabled = false;
                SignInButton.Text = "Cancel";
                this.Invalidate();
            }));

            try
            {
                //创建TCP连接
                LoginSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                LoginSocket.Connect(UnityModule.ServerIP, UnityModule.ServerPort);
                //发送登录协议
                LoginSocket.Send(Encoding.ASCII.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignIn, Application.ProductVersion, UserIDTextBox.Text, PasswordTextBox.Text)));
                //接收登录验证结果
                byte[] SignResultBytes = new byte[1024];
                LoginSocket.Receive(SignResultBytes);
                string SignResult = Encoding.ASCII.GetString(SignResultBytes).Trim('\0');
                UnityModule.DebugPrint("接收到登录结果：{0}", SignResult);

                //判断登录验证结果
                if (SignResult.StartsWith(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignInSuccessfully)))
                {
                    //登录成功，切换界面
                    this.Invoke(new Action(() =>
                    {
                        SignInButton.Text = "Success.";
                        this.Invalidate();
                        UnityModule.USERID = UserIDTextBox.Text;
                        new ClientForm() { loginForm =this}.Show();
                        HideMe(HideTo.JustHide);
                    }));
                }
                else
                {
                    //登录失败
                    this.Invoke(new Action(() =>
                    {
                        new MyMessageBox("您的密码输入错误，请重试！", MyMessageBox.IconType.Warning).ShowDialog(this);
                        SignInButton.Text = "Sign In";
                    }));
                }
                LoginSocket?.Close();
                UnityModule.DebugPrint("验证登录TCP连接已经关闭 ...");

                UserIDTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
                this.Invalidate();

                Logining = false;
            }
            catch (ThreadAbortException)
            {
                //遇到线程中断异常时忽略，因为此异常由用户手动取消放弃登录任务。
                return; 
            }
            catch (Exception ex)
            {
                //登录过程中遇到错误
                Logining = false;
                UnityModule.DebugPrint("登录遇到错误！{0}", ex.Message);
                this.Invoke(new Action(() =>
                {
                    if (new MyMessageBox("登录遇到错误，是否重试？\n" + ex.Message, MyMessageBox.IconType.Question).ShowDialog(this) == DialogResult.OK)
                    {
                        this.Invoke(new Action(() =>
                        {
                            SignInButton_Click(null, null);
                        }));
                    }
                    else
                    {
                        UserIDTextBox.Enabled = true;
                        PasswordTextBox.Enabled = true;
                        SignInButton.Text = "Sign In";
                        this.Invalidate();
                    }
                }));
            }
        }

        #endregion

        #region 窗体事件

        public LoginForm()
        {
            //禁用多线程UI检查
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            //设置 图标 和 绘制方式(减少闪烁)
            this.Icon = UnityResource.Speleon;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            //为窗体增加阴影
            UnityModule.DrawWindowShadow(this);

            UnityModule.DebugPrint("窗体创建成功");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //设置标题图标
            TitleLabel.Image = UnityResource.Speleon.ToBitmap();
            //设置控件的容器关系
            TitleLabel.Parent = ActiveBGIBOX;
            MinButton.Parent = ActiveBGIBOX;
            CloseButton.Parent = ActiveBGIBOX;
            LoginAreaLabel.Parent = ActiveBGIBOX;

            //绑定鼠标拖动事件
            this.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            ActiveBGIBOX.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            TitleLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            LoginAreaLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);

            UnityModule.DebugPrint("窗体加载成功");
        }

        /// <summary>
        /// 是否放行关闭窗体消息
        /// </summary>
        bool AllowToClose = false;
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowToClose)
            {
                e.Cancel = true;
                HideMe(HideTo.Close);
            }
        }

        private void LoginForm_Paint(object sender, PaintEventArgs e)
        {
            using (Brush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.WhiteSmoke, Color.White, LinearGradientMode.ForwardDiagonal))
            {
                //绘制渐变
                e.Graphics.FillRectangle(linearGradientBrush, this.ClientRectangle);
            }
        }

        #endregion


    }
}

