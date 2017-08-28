using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Client
{
    public partial class ClientForm : Form
    {
        /*注意！
         * 
         * 1>.双方Socket收发缓存区为64KB，这是包括按UTF8编码转换为BASE64编码(BASE64的字节数为原编码消息长度的5/4，
         * 即增加25%)后的消息加上自己规定的协议的最大长度，在客户端发送消息时需要限制用户文本框输入的字符数尽量不要超
         * 过16000个汉字，其实聊天也足够用了
         * 虽然TCP协议也会自己给消息截段，但是非头的段没有我们自己规定的协议头，会导致客户端无法判断消息的类型
         * 
         * 2>.消息内容内可能包含'\n'会与双方匹配消息内容的正则表达式冲突，所以需要先转换消息内容为BASE64编码，以屏蔽'\n'，
         * 或在发送消息的协议最后加上"'\n''\0'"，而正则表达式也以"'\n''\0'"结束即可
         */

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
                    
                    //发送获取好友列表请求
                    UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.GetFriendsList,Application.ProductVersion,UnityModule.USERID)));
                    UnityModule.DebugPrint("GETFRIENDSLIST 数据包发送成功！");
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
            UnityModule.DebugPrint("开始动态隐藏窗体...{0}", this.Name);
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
                if (UnitySocket?.Connected??false)
                {
                    //发送注销登录消息
                    UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol( ProtocolFormatter.CMDType.SignOut,Application.ProductVersion,UnityModule.USERID)));
                    UnitySocket.Close();
                }
            }catch{}
            try
            {
                ReceiveThread?.Abort();
            }
            catch {}
            this.Close();
            Application.Exit();
        }

        /// <summary>
        /// 接收来自服务器的消息
        /// </summary>
        private void ReceiveMessage()
        {
            while (true)
            {
                byte[] MessageBuffer = new byte[]{};
                int MessageBufferSize = 0;
                string ServerMessagePackage = "";

                try
                {
                    MessageBuffer = new byte[UnitySocket.ReceiveBufferSize - 1];
                    MessageBufferSize = UnitySocket.Receive(MessageBuffer);
                    ServerMessagePackage = Encoding.UTF8.GetString(MessageBuffer, 0, MessageBufferSize);
                }
                catch (ThreadAbortException) { return; }
                catch (Exception ex)
                {
                    UnityModule.DebugPrint("接收消息时遇到错误：{0}", ex.Message);
                    HideMe(HideTo.JusetClose);
                    this.loginForm.Show();
                    this.loginForm.ShowTips("与服务器连接中断，请检查网络连接。" + Convert.ToString(ex.HResult, 16));
                    return;
                }

                UnityModule.DebugPrint("ServerMessagePackage : {0}", ServerMessagePackage);

                /*
                 * 遇到严重的TCP粘包问题，服务端分多次发送的GETFRIENDSLIST协议，被一次发送给了客户端
                 * 导致客户端只能提取到第一条协议，因为每条协议以'\n'结尾，所以每次收到数据包后对其按'\n'分割
                 * 分割后判断不为空的段后加'\n'再按正常的协议包处理，为空不处理
                 * 以上，解决粘包问题
                 */
                string[] ServerMessages = ServerMessagePackage.Split('\n');
                foreach(string TempServerMessage in ServerMessages)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(TempServerMessage)) continue;

                        string ServerMessage = TempServerMessage + '\n';
                        UnityModule.DebugPrint("ServerMessage : {0}", ServerMessage);
                        string MessagePattern = ProtocolFormatter.GetCMDTypePattern();
                        Regex MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match MessageMatchResult = MessageRegex.Match(ServerMessage);
                        string cmdType = MessageMatchResult.Groups["CMDTYPE"].Value.ToUpper();
                        UnityModule.DebugPrint("收到 CMDTYPE : {0}", cmdType);

                        switch (cmdType)
                        {
                            case "CHATMESSAGE":
                                {
                                    string FromID=null,Message = null;
                                    DateTime ChatTime;
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.ChatMessage);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ServerMessage);
                                    FromID = MessageMatchResult.Groups["FROMID"].Value.ToString();
                                    ChatTime = DateTime.TryParse(MessageMatchResult.Groups["CHATTIME"].Value.ToString(),out ChatTime) ? ChatTime.ToLocalTime():DateTime.Now;
                                    Message =Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["MESSAGE"].Value.ToString()));

                                    this.Invoke(new Action(() =>
                                    {
                                        new MyMessageBox(Message, string.Format("{0} 来自 [{1}] 的消息：",ChatTime.ToString(), FromID),MyMessageBox.IconType.Info).Show(this);
                                    }));
                                    break;
                                }
                            case "GETFRIENDSLIST":
                                {
                                    string FriendID = null, NickName = null,Signature=null;
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.GetFriendsList);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ServerMessage);
                                    FriendID = MessageMatchResult.Groups["FRIENDID"].Value.ToString();
                                    NickName = Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["NICKNAME"].Value.ToString()));
                                    Signature = Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["SIGNATURE"].Value.ToString()));

                                    this.Invoke(new Action(() =>
                                    {
                                        UnityModule.DebugPrint("收到列表内好友信息：{1} ({0})：{2}", FriendID, NickName, Signature);
                                        FriendsFlowPanel.Controls.Add(new FriendItem(FriendID,NickName,Signature));
                                    }));
                                    break;
                                }
                            case "FRIENDSLISTCOMPLETE":
                                {
                                    UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol( ProtocolFormatter.CMDType.GetMessageNotSendYet,Application.ProductVersion,UnityModule.USERID)));
                                    break;
                                }
                            case "MESSAGENSYCOMPLETE":
                                {
                                    MessageBox.Show("暂存消息发送完毕");
                                    break;
                                }
                            case "ANOTHORSIGNIN":
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        HideMe(HideTo.JusetClose);
                                        this.loginForm.Show();
                                        this.loginForm.ShowTips("您的账号异地登陆，请注意密码安全！");
                                        UnitySocket.Close();
                                        ReceiveThread.Abort();
                                    }));

                                    //这里需要 return; 否则会进入 catch(){} 被当做异常处理
                                    return;
                                }
                            case "SERVERSHUTDOWN":
                                {
                                    this.Invoke(new Action(()=> {
                                        HideMe(HideTo.JusetClose);
                                        this.loginForm.Show();
                                        this.loginForm.ShowTips("远程服务器主动关闭，可能Leon关机去上课了...");
                                        UnitySocket.Close();
                                        ReceiveThread.Abort();
                                    }));
                                    return;
                                }
                            default:
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        new MyMessageBox("遇到未知的 CMDTYPE : " + cmdType, MyMessageBox.IconType.Info).Show(this);
                                    }));
                                    break;
                                }
                        }

                    }
                    catch (ThreadAbortException) { return; }
                    catch (Exception ex)
                    {
                        UnityModule.DebugPrint("处理消息时遇到错误：{0}", ex.Message);
                    }
                }

            }
        }

        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        private void ChatSendButton_Click(object sender, EventArgs e)
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

            UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage,Application.ProductVersion,"66666",ChatInputTextBox.Text)));
        }


    }
}
