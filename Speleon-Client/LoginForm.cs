﻿using System;
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
using System.Text.RegularExpressions;
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

        /// <summary>
        /// 关闭Tips事件句柄，使用唯一变量，防止动态显示和动态隐藏线程冲突
        /// </summary>
        private EventHandler CloseTipsEventHandler;

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
            UnityModule.DebugPrint("开始动态隐藏窗体...{0}",this.Name);
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
            if (string.IsNullOrEmpty(UserIDTextBox.Text))
            {
                ShowTips("请输入用户名！");
                UserIDTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                ShowTips("请输入密码！");
                PasswordTextBox.Focus();
                return;
            }

            if (!Logining)
            {
                //开始登录
                Logining = true;
                ShowTips("正在登录...");
                LoginThread = new Thread(Login);
                LoginThread.Start();
            }
            else
            {
                //取消登录
                LoginSocket?.Close();
                LoginSocket?.Dispose();
                LoginThread?.Abort();
                UserIDTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
                SignInButton.Text = "Sign In";
                ShowTips("已取消登录。");
                Logining = false;
            }
        }

        /// <summary>
        /// 限制用户账号输入框的输入内容
        /// </summary>
        private void UserIDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                //屏蔽非数字键和非退格键
                e.Handled = true;
                if (e.KeyChar == (char)Keys.Enter)
                {
                    //回车键触发登录
                    SignInButton_Click(null,null);
                }
            }
        }

        /// <summary>
        /// 在密码输入框使用回车键触发登录
        /// </summary>
        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SignInButton_Click(null, null);
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
                byte[] SignResultBytes = new byte[LoginSocket.ReceiveBufferSize-1];
                LoginSocket.Receive(SignResultBytes);
                string SignResult = Encoding.ASCII.GetString(SignResultBytes).Trim('\0');
                UnityModule.DebugPrint("接收到登录结果：{0}", SignResult);

                string MessagePattern = ProtocolFormatter.GetCMDTypePattern();
                Regex MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match MessageMatchResult = MessageRegex.Match(SignResult);
                string cmdType = MessageMatchResult.Groups["CMDTYPE"].Value.ToUpper();
                switch (cmdType)
                {
                    case "SIGNINSUCCESSFULLY":
                        {
                            //登录成功，切换界面
                            this.Invoke(new Action(() =>
                            {
                                ShowTips("登录成功！");
                                SignInButton.Text = "Success.";
                                this.Invalidate();
                                UnityModule.USERID = UserIDTextBox.Text;
                                new ClientForm() { loginForm = this }.Show();
                                HideMe(HideTo.JustHide);
                            }));
                            break;
                        }
                    case "SIGNINUNSUCCESSFULLY":
                        {
                            //登录失败
                            this.Invoke(new Action(() =>
                            {
                                ShowTips("您的密码输入错误，请重试！");
                                SignInButton.Text = "Sign In";
                                PasswordTextBox.Focus();
                            }));
                            break;
                        }
                }

                LoginSocket?.Close();
                UnityModule.DebugPrint("验证登录TCP连接已经关闭 ...");

                UserIDTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
                this.Invalidate();
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
                    UserIDTextBox.Enabled = true;
                    PasswordTextBox.Enabled = true;
                    SignInButton.Text = "Sign In";
                    this.Invalidate();
                    ShowTips("登录遇到错误，请检查网络连接。"+Convert.ToString(ex.HResult,16));
                }));
            }

            Logining = false;
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
            TipsPanel.Parent = ActiveBGIBOX;

            //绑定鼠标拖动事件
            this.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            ActiveBGIBOX.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            TitleLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            LoginAreaLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);

            //防止输入框选中
            UserIDTextBox.SelectionStart = UserIDTextBox.Text.Length;
            UserIDTextBox.SelectionLength = 0;
            PasswordTextBox.SelectionStart = PasswordTextBox.Text.Length;
            PasswordTextBox.SelectionLength = 0;
            
            //记录关闭Tips事件句柄
            CloseTipsEventHandler = new EventHandler(TipsClsoeButton_Click);

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

        #endregion

        #region Paint

        private void TipsPanel_Paint(object sender, PaintEventArgs e)
        {
            using (Brush linearGradientBrush = new LinearGradientBrush(TipsPanel.ClientRectangle, Color.Transparent, Color.FromArgb(255, 255, 255, 255), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(linearGradientBrush, TipsPanel.ClientRectangle);
            }
        }

        private void ControlPanel_Paint(object sender, PaintEventArgs e)
        {
            using (Brush linearGradientBrush = new LinearGradientBrush(ControlPanel.ClientRectangle, Color.White, Color.Gainsboro, LinearGradientMode.BackwardDiagonal))
            {
                e.Graphics.FillRectangle(linearGradientBrush, ControlPanel.ClientRectangle);
            }
        }

        #endregion

        #region 显示提示消息

        /// <summary>
        /// 动态显示Tips
        /// </summary>
        /// <param name="TipsMessage"></param>
        public void ShowTips(string TipsMessage)
        {
            TipsLabel.Text = TipsMessage;
            if (TipsPanel.Visible)
            {
                if (TipsPanel.Top == ActiveBGIBOX.Height - TipsPanel.Height)
                {
                    TipsPanel.Top = ActiveBGIBOX.Height;
                }
            }
            else
            {
                TipsPanel.Show();
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                while (TipsPanel.Top > ActiveBGIBOX.Height - TipsPanel.Height)
                {
                    TipsPanel.Top -= 1;
                    Thread.Sleep(5);
                }
                TipsClsoeButton.Click += CloseTipsEventHandler;
            }));
        }

        /// <summary>
        /// 点击关闭Tips
        /// </summary>
        private void TipsClsoeButton_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                while (TipsPanel.Top < ActiveBGIBOX.Height)
                {
                    TipsPanel.Top += 1;
                    Thread.Sleep(5);
                }
                TipsPanel.Hide();
                TipsClsoeButton.Click -= CloseTipsEventHandler;
            }));
        }

        #endregion

    }
}

