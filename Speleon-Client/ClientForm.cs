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
        /// 动态隐藏后执行的动作
        /// </summary>
        private enum HideTo
        {
            Min,
            Close
        }

        /// <summary>
        /// 全局TCPSocket
        /// </summary>
        Socket UnitySocket;

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
                HideMe(HideTo.Close);
            }
        }

        private void ClientForm_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Invalidate();

                UnitySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                UnitySocket.Connect(UnityModule.ServerIP, UnityModule.ServerPort);
                //UnitySocket.Send(Encoding.ASCII.GetBytes("123333333"));
            }
            catch (Exception ex)
            {
                new MyMessageBox("连接服务器遇到错误：{0}", ex.Message).ShowDialog(this);
            }
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
            if (new MyMessageBox("真的要退出 Speleon 吗？", MyMessageBox.IconType.Question).ShowDialog(this) == DialogResult.OK)
            {
                HideMe(HideTo.Close);
            }
            else
            {
                CloseButton.Image = UnityResource.Close_0;
                CloseButton.Invalidate();
            }
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
                else if (hideTo == HideTo.Close)
                {
                    UnityModule.DebugPrint("退出程序");
                    AllowToClose = true;
                    Application.Exit();
                }
            }));
        }

        #endregion

    }
}
