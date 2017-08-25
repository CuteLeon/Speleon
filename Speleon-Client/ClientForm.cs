using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Client
{
    public partial class ClientForm : Form
    {
        #region 变量和枚举

        /// <summary>
        /// 父登录窗体
        /// </summary>
        public LoginForm loginForm;

        /// <summary>
        /// 动态隐藏后执行的动作
        /// </summary>
        private enum HideTo
        {
            Min,
            JusetClose,
            ExitApp
        }

        /// <summary>
        /// 全局TCPSocket
        /// </summary>
        Socket UnitySocket;

        /// <summary>
        /// 后台接收服务器消息线程
        /// </summary>
        Thread ReceiveThread = null;

        #endregion

        #region 窗体事件

        public ClientForm()
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
            UnityModule.DebugPrint("ClientForm 窗体创建成功");
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            //设置标题图标
            TitleLabel.Image = UnityResource.Speleon.ToBitmap();
            
            //绑定鼠标拖动事件
            this.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            TitleLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);
            TitlePanel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);

            //为关闭按钮设置动态效果
            CloseButton.MouseEnter += new EventHandler(delegate (object s, EventArgs ea) {CloseButton.Image = UnityResource.Close_1 as Image; });
            CloseButton.MouseLeave += new EventHandler(delegate (object s, EventArgs ea) { CloseButton.Image = UnityResource.Close_0 as Image; });
            CloseButton.MouseDown += new MouseEventHandler(delegate (object s, MouseEventArgs mea) { CloseButton.Image = UnityResource.Close_2; });

            UnityModule.DebugPrint("窗体加载成功");
        }

        /// <summary>
        /// 是否放行关闭窗体消息
        /// </summary>
        bool AllowToClose = false;
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowToClose)
            {
                e.Cancel = true;
                TryToClose();
            }
        }

        private void ClientForm_Shown(object sender, EventArgs e)
        {
            this.Invalidate();
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate{
                try
                {
                    UnitySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    UnitySocket.Connect(UnityModule.ServerIP, UnityModule.ServerPort);
                    ReceiveThread = new Thread(ReceiveMessage);
                    ReceiveThread.Start();
                    
                    //发送WHOAMI数据，告诉服务端自己的USERID
                    UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol( ProtocolFormatter.CMDType.WhoAmI,Application.ProductVersion,UnityModule.USERID)));
                    UnityModule.DebugPrint("WHOAMI 数据包发送成功！");
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(()=> {
                        new MyMessageBox("连接服务器遇到错误：{0}", ex.Message).ShowDialog(this);
                    }));
                }
            }));
        }

        private void ClientForm_Resize(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case FormWindowState.Normal:
                    {
                        MaxButton.Show();
                        RestoreButton.Hide();
                        break;
                    }
                case FormWindowState.Maximized:
                    {
                        MaxButton.Hide();
                        RestoreButton.Show();
                        break;
                    }
                case FormWindowState.Minimized:
                    {
                        break;
                    }
            }
        }

        #endregion

        #region 标题栏按钮事件

        private void CloseButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击关闭按钮");
            CloseButton.Image = UnityResource.Close_1;
            CloseButton.Invalidate();
            TryToClose();
            CloseButton.Image = UnityResource.Close_0;
            CloseButton.Invalidate();
        }

        private void MinButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击最小化按钮");
            HideMe(HideTo.Min);
        }

        private void MaxButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击最大化按钮");
            this.WindowState = FormWindowState.Maximized;
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            UnityModule.DebugPrint("点击还原按钮");
            this.WindowState = FormWindowState.Normal;
        }

        #endregion

        #region 功能函数

        /// <summary>
        /// 询问用户是否退出
        /// </summary>
        private void TryToClose()
        {
            if (new MyMessageBox("真的要退出 Speleon 吗？", MyMessageBox.IconType.Question).ShowDialog(this) == DialogResult.OK)
            {
                HideMe(HideTo.ExitApp);
            }
            else
            {
                AllowToClose = false;
            }
        }

        /// <summary>
        /// 动态隐藏方法
        /// </summary>
        /// <param name="hideTo">隐藏后的动作</param>
        private void HideMe(HideTo hideTo)
        {
            UnityModule.DebugPrint("开始动态隐藏窗体...");
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
                int IniTop = this.Top;
                while (this.Opacity > 0)
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
                else if (hideTo == HideTo.JusetClose)
                {
                    UnityModule.DebugPrint("关闭ClientForm");
                    AllowToClose = true;
                    this.Close();
                }
                else if (hideTo == HideTo.ExitApp)
                {
                    UnityModule.DebugPrint("退出程序");
                    AllowToClose = true;
                    ExitApplication();
                }
            }));
        }

        /// <summary>
        /// 退出客户端
        /// </summary>
        private void ExitApplication()
        {
            try
            {
                UnitySocket?.Close();
            }catch{}
            try
            {
                ReceiveThread?.Abort();
            }
            catch {}
            Application.Exit();
        }

        /// <summary>
        /// 接收来自服务器的消息
        /// </summary>
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] MessageBuffer = new byte[UnitySocket.ReceiveBufferSize - 1];
                    int MessageBufferSize = UnitySocket.Receive(MessageBuffer);
                    string Message = Encoding.UTF8.GetString(MessageBuffer, 0, MessageBufferSize);

                    UnityModule.DebugPrint("收到服务器的消息：{0}", Message);
                    this.Invoke(new Action(() =>
                    {
                        new MyMessageBox(Message, "收到服务器的消息：").ShowDialog(this);
                    }));
                }
                catch (ThreadAbortException) {return;}
                catch (Exception ex)
                {
                    //todo:客户端，连接断开，需要重新连接/登录
                    UnityModule.DebugPrint("接收消息时遇到错误：{0}", ex.Message);
                    HideMe(HideTo.JusetClose);
                    this.loginForm.Show();
                    //"您的账号在其他地方登录，请注意密码安全！"
                    return;
                }
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (UnitySocket == null)
            {
                //Socket为空，需要初始化
                //初始化失败时需要结束
            }
            if (!UnitySocket.Connected)
            {
                //Socket未连接，需要连接
                //连接失败时需要结束
            }

            UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.Message,Application.ProductVersion,"66666",textBox1.Text)));
        }

    }
}
