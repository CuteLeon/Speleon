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
        /// 字典用于记录每个好友的聊天记录最早一条MessageID，再次拉取聊天记录时需要从此记录往前查找
        /// </summary>
        Dictionary<string, int> FriendsFirstMessageID=new Dictionary<string, int>();

        /// <summary>
        /// 全局TCPSocket
        /// </summary>
        Socket UnitySocket;

        /// <summary>
        /// 后台接收服务器消息线程
        /// </summary>
        Thread ReceiveThread = null;

        /// <summary>
        /// 聊天气泡自适应的最宽宽度
        /// </summary>
        private int BubbleMaxWidth = 0;

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

            //为 FriendItem 赋值 ParentPanel 属性，并绑定事件
            FriendItem.ParentPanel = FriendsFlowPanel;
            FriendItem.ActiveItemChanged += new EventHandler<FriendItem>(FriendItemActiveChanged);

            BubbleMaxWidth = MainPanel.Width - 60;

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
        /// FriendItem激活项改变，切换聊天记录控件
        /// </summary>
        /// <param name="OldActiveItem"></param>
        /// <param name="NewActiveItem"></param>
        private void FriendItemActiveChanged(object OldActiveItem,FriendItem NewActiveItem)
        {
            if ((FriendItem)OldActiveItem != null)
            {
                UnityModule.DebugPrint("旧的激活项：{0}",((FriendItem)OldActiveItem).FriendID);
                ((FriendItem)OldActiveItem).ChatDraft = ChatInputTextBox.Text;
                ((FriendItem)OldActiveItem).ChatBubblesPanel?.Hide();
            }
            if ((FriendItem)NewActiveItem != null)
            {
                UnityModule.DebugPrint("新的激活项：{0}", ((FriendItem)NewActiveItem).FriendID);
                if (!ChatSendPanel.Visible) ChatSendPanel.Show();

                ChatInputTextBox.Text = NewActiveItem.ChatDraft;
                if (NewActiveItem.ChatBubblesPanel != null)
                {
                    NewActiveItem.ChatBubblesPanel.Show();
                    NewActiveItem.ChatBubblesPanel.BringToFront();
                }
            }
            else
            {
                ChatSendPanel.Hide();
            }
        }


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
                    //初始化FriendItem静态数据，否则再次登录后会出现问题
                    FriendItem.ResetStaticData();
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
                #region 接收消息
                byte[] MessageBuffer = new byte[] { };
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
                #endregion

                /*
                 * 遇到严重的TCP粘包问题，服务端分多次发送的GETFRIENDSLIST协议，被一次发送给了客户端
                 * 导致客户端只能提取到第一条协议，因为每条协议以'\n'结尾，所以每次收到数据包后对其按'\n'分割
                 * 分割后判断不为空的段后加'\n'再按正常的协议包处理，为空不处理
                 * 以上，解决粘包问题
                 */
                string[] ServerMessages = ServerMessagePackage.Split('\n');
                foreach (string TempServerMessage in ServerMessages)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(TempServerMessage)) continue;

                        #region 读取消息协议类型
                        string ServerMessage = TempServerMessage + '\n';
                        UnityModule.DebugPrint("ServerMessage : {0}", ServerMessage);
                        string MessagePattern = ProtocolFormatter.GetCMDTypePattern();
                        Regex MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match MessageMatchResult = MessageRegex.Match(ServerMessage);
                        string cmdType = MessageMatchResult.Groups["CMDTYPE"].Value.ToUpper();
                        UnityModule.DebugPrint("收到 CMDTYPE : {0}", cmdType);
                        #endregion

                        switch (cmdType)
                        {
                            case "CHATMESSAGE":
                                {
                                    #region 聊天消息
                                    string FromID = null, Message = null;
                                    int MessageID;
                                    DateTime ChatTime;
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.ChatMessage);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ServerMessage);
                                    FromID = MessageMatchResult.Groups["FROMID"].Value;
                                    ChatTime = DateTime.TryParse(MessageMatchResult.Groups["CHATTIME"].Value.ToString(), out ChatTime) ? ChatTime.ToLocalTime() : DateTime.Now;
                                    MessageID = int.Parse(MessageMatchResult.Groups["MESSAGEID"].Value);
                                    Message = Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["MESSAGE"].Value));

                                    this.Invoke(new Action(() =>
                                    {
                                        FriendItem MessageFrom = FriendItem.GetFriendItemByFriendID(FromID);
                                        if (MessageFrom != null)
                                        {
                                            FriendsFlowPanel.Controls.SetChildIndex(MessageFrom, 0);

                                            if (FriendItem.ActiveFriend != MessageFrom) MessageFrom.MessageNRTCount += 1;

                                            MessageFrom.ChatBubblesPanel.Controls.Add(
                                                new ChatBubble
                                                (
                                                    MessageID.ToString(),
                                                    ChatTime.ToString(),
                                                    FromID,
                                                    Message,
                                                    BubbleMaxWidth,
                                                    false
                                                )
                                            );
                                        }
                                    }));
                                    break;
                                    #endregion
                                }
                            case "GETCHATHISTORY":
                                {
                                    #region 获取历史聊天记录
                                    //todo:显示历史聊天记录
                                    string FromID = "";
                                    int MessageID = 0;
                                    //更新本地第一条聊天记录MessageID
                                    if (FriendsFirstMessageID.ContainsKey(FromID))
                                    {
                                        //如果遇到更小的MessageID，只有
                                        if (MessageID < FriendsFirstMessageID[FromID]) FriendsFirstMessageID[FromID] = MessageID;
                                    }
                                    else
                                    {
                                        //估计运行不到这里
                                        FriendsFirstMessageID.Add(FromID, MessageID);
                                    }
                                    break;
                                    #endregion
                                }
                            case "GETFRIENDSLIST":
                                {
                                    #region 获取好友列表
                                    string FriendID = null, NickName = null, Signature = null; bool OnLine = false;
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.GetFriendsList);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ServerMessage);
                                    FriendID = MessageMatchResult.Groups["FRIENDID"].Value.ToString();
                                    NickName = Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["NICKNAME"].Value.ToString()));
                                    Signature = Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["SIGNATURE"].Value.ToString()));
                                    OnLine = Convert.ToBoolean(MessageMatchResult.Groups["ONLINE"].Value.ToString());

                                    this.Invoke(new Action(() =>
                                    {
                                        UnityModule.DebugPrint("收到好友信息：{1} ({0})：{2}", FriendID, NickName, Signature);

                                        if (FriendItem.FriendExisted(FriendID))
                                        {
                                            //TODO:如果 FriendID已经存在，且FriendItem不为null，仅更新FriendID的信息，此特性可以在服务端用于有用户更新了资料时，立即向好友客户端更新资料
                                            FriendItem.GetFriendItemByFriendID(FriendID)?.SetNickNameSignatureAndOnLine(NickName, Signature, OnLine);
                                        }
                                        else
                                        {
                                            //默认好友聊天历史记录最早一条MessageID=0
                                            if (!FriendsFirstMessageID.ContainsKey(FriendID)) FriendsFirstMessageID.Add(FriendID, 0);
                                            //新添加 FriendItem
                                            FriendItem NewFriendItem = new FriendItem(FriendID, NickName, Signature, OnLine)
                                            {
                                                RightToLeft = RightToLeft.No
                                            };
                                            NewFriendItem.FriendItemClick += new EventHandler(FriendItemClick);

                                            //创建好友聊天记录控件
                                            MyTableLayoutPanel NewChatBubblePanel = new MyTableLayoutPanel()
                                            {
                                                AutoScroll = true,
                                                Dock = DockStyle.Fill,
                                                BackColor = Color.White,
                                                Visible = false,
                                            };

                                            //绑定自动滚动到底部事件
                                            NewChatBubblePanel.ControlAdded += new ControlEventHandler(ChatBubblesPanel_ControlAdded);

                                            MainPanel.Controls.Add(NewChatBubblePanel);

                                            NewFriendItem.ChatBubblesPanel = NewChatBubblePanel;
                                            FriendsFlowPanel.Controls.Add(NewFriendItem);
                                        }
                                    }));
                                    break;
                                    #endregion
                                }
                            case "FRIENDSIGNIN":
                                {
                                    #region 好友登录
                                    string FriendID = null;
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.FriendSignIn);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ServerMessage);
                                    FriendID = MessageMatchResult.Groups["FRIENDID"].Value.ToString();

                                    this.Invoke(new Action(() =>
                                    {
                                        UnityModule.DebugPrint("收到好友登录消息：{0}", FriendID);
                                        if (FriendItem.FriendExisted(FriendID))
                                        {
                                            FriendItem JustSignIn = FriendItem.GetFriendItemByFriendID(FriendID);
                                            if (JustSignIn != null)
                                            {
                                                FriendsFlowPanel.Controls.SetChildIndex(JustSignIn, FriendItem.OnLineCount);
                                                JustSignIn.OnLine = true;
                                            }
                                        }
                                    }));

                                    break;
                                    #endregion
                                }
                            case "FRIENDSIGNOUT":
                                {
                                    #region 好友注销登录
                                    string FriendID = null;
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.FriendSignOut);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ServerMessage);
                                    FriendID = MessageMatchResult.Groups["FRIENDID"].Value.ToString();

                                    this.Invoke(new Action(() =>
                                    {
                                        UnityModule.DebugPrint("收到好友注销消息：{0}", FriendID);
                                        if (FriendItem.FriendExisted(FriendID))
                                        {
                                            FriendItem JustSignIn = FriendItem.GetFriendItemByFriendID(FriendID);
                                            if (JustSignIn != null)
                                            {
                                                JustSignIn.OnLine = false;
                                                FriendsFlowPanel.Controls.SetChildIndex(JustSignIn, FriendItem.OnLineCount);
                                            }
                                        }
                                    }));
                                    break;
                                    #endregion
                                }
                            case "FRIENDSLISTCOMPLETE":
                                {
                                    #region 好友列表获取完毕
                                    UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.GetMessageNotSendYet, Application.ProductVersion, UnityModule.USERID)));
                                    break;
                                    #endregion
                                }
                            case "MESSAGENSYCOMPLETE":
                                {
                                    #region 未读消息获取完毕
                                    //准备完毕，测试用
                                    UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.GetChatHistory, Application.ProductVersion, "66666", "0")));
                                    break;
                                    #endregion
                                }
                            case "ANOTHORSIGNIN":
                                {
                                    #region 异地登录
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
                                    #endregion
                                }
                            case "SERVERSHUTDOWN":
                                {
                                    #region 远程服务端关闭
                                    this.Invoke(new Action(() => {
                                        HideMe(HideTo.JusetClose);
                                        this.loginForm.Show();
                                        this.loginForm.ShowTips("远程服务器主动关闭，可能Leon关机去上课了...");
                                        UnitySocket.Close();
                                        ReceiveThread.Abort();
                                    }));
                                    return;
                                    #endregion
                                }
                            default:
                                {
                                    #region 未知的消息协议
                                    this.Invoke(new Action(() =>
                                    {
                                        new MyMessageBox("遇到未知的 CMDTYPE : " + cmdType, MyMessageBox.IconType.Info).Show(this);
                                    }));
                                    break;
                                    #endregion
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
            if (FriendItem.ActiveFriend == null) return;
            if (string.IsNullOrEmpty(ChatInputTextBox.Text)) return;
            FriendsFlowPanel.Controls.SetChildIndex(FriendItem.ActiveFriend,0);

            FriendItem.ActiveFriend.ChatBubblesPanel.Controls.Add(
                new ChatBubble
                (
                    "0",
                    DateTime.Now.ToString(),
                    UnityModule.USERID,
                    ChatInputTextBox.Text,
                    BubbleMaxWidth,
                    true
            ));

            UnitySocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage,Application.ProductVersion,FriendItem.ActiveFriend.FriendID,ChatInputTextBox.Text)));

            ChatInputTextBox.Text = "";
        }

        
        private void FriendItemClick(object sender,EventArgs e)
        {
            
        }

        private void ChatBubblesPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            ((MyTableLayoutPanel)sender).VerticalScroll.Value = ((MyTableLayoutPanel)sender).VerticalScroll.Maximum;
        }

        
        private void MainPanel_Resize(object sender, EventArgs e)
        {
            if (BubbleMaxWidth + 60 == MainPanel.Width) return;

            BubbleMaxWidth = MainPanel.Width - 60;
            foreach (FriendItem friendItem in FriendsFlowPanel.Controls)
            {
                if (friendItem is FriendItem)
                {
                    foreach (ChatBubble chatBubble in friendItem.ChatBubblesPanel.Controls)
                    {
                        if (chatBubble is ChatBubble)
                        {
                            chatBubble.MaxWidth = BubbleMaxWidth;
                        }
                    }
                }
            }
            System.Diagnostics.Debug.Print(BubbleMaxWidth.ToString());
        }

        private void ChatInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void ChatInputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (!e.Control)
                {
                    ChatSendButton_Click(null, null);
                }
            }
        }

        protected override void WndProc(ref Message Msg)
        {
            switch (Msg.Msg)
            {
                //鼠标拖动改变大小
                case UnityModule.WM_NCHITTEST:
                    {
                        // 获取鼠标位置
                        int nPosX = (Msg.LParam.ToInt32() & 65535);
                        int nPosY = (Msg.LParam.ToInt32() >> 16);

                        if (nPosX >= this.Right-6 && nPosY >= this.Bottom-6)
                        {
                            //右下角
                            Msg.Result = new IntPtr(UnityModule.HT_BOTTOMRIGHT);
                        }
                        else if (nPosX <= this.Left + 6 && nPosY <= this.Top + 6)
                        {
                            //左上角
                            Msg.Result = new IntPtr(UnityModule.HT_TOPLEFT);
                        }
                        else if (nPosX <= this.Left + 6 && nPosY >= this.Bottom - 6)
                        {
                            //左下角
                            Msg.Result = new IntPtr(UnityModule.HT_BOTTOMLEFT);
                        }
                        else if (nPosX >= this.Right - 6 && nPosY <= this.Top + 6)
                        {
                            //右上角
                            Msg.Result = new IntPtr(UnityModule.HT_TOPRIGHT);
                        }
                        else if (nPosX >= this.Right - 2)
                        {
                            //右边框
                            Msg.Result = new IntPtr(UnityModule.HT_RIGHT);
                        }
                        else if (nPosY >= this.Bottom - 2)
                        {
                            //底边框
                            Msg.Result = new IntPtr(UnityModule.HT_BOTTOM);
                        }
                        else if (nPosX <= this.Left + 2)
                        {
                            //左边框
                            Msg.Result = new IntPtr(UnityModule.HT_LEFT);
                        }
                        else if (nPosY <= this.Top + 2)
                        {
                            //上边框
                            Msg.Result = new IntPtr(UnityModule.HT_TOP);
                        }
                        else if (nPosY <= this.Left + 20)
                        {
                            //上方 2~20 像素作为标题栏拖动
                            Msg.Result = new IntPtr(UnityModule.HT_CAPTION);
                        }
                        break;
                    }
                default:
                    {
                        base.WndProc(ref Msg);
                        break;
                    }
            }
        }

        private void ClientForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Gainsboro,0,0,this.Width, TitlePanel.Height + 5);
            e.Graphics.FillRectangle(Brushes.WhiteSmoke, 0, FriendsFlowPanel.Top, 5, FriendsFlowPanel.Height + 5);
            e.Graphics.FillRectangle(Brushes.WhiteSmoke, 0, FriendsFlowPanel.Bottom, FriendsFlowPanel.Width+5, 5);
        }
    }
}
