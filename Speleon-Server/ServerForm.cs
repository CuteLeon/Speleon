using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Server
{
    public partial class ServerForm : Form
    {
        #region 变量

        /// <summary>
        /// 最多允许连接的客户端数量
        /// </summary>
        private const byte MAX_CLIENT_COUNT = 10;
        /// <summary>
        /// 用户账号为KEY，Socket为Value的字典
        /// </summary>
        Dictionary<string, Socket> SocketsDictionary = new Dictionary<string, Socket>();
        /// <summary>
        /// Socket远程地址和端口为KEY，Thread为线程
        /// </summary>
        Dictionary<string, Thread> ReceiveThreadDictionary = new Dictionary<string, Thread>();
        /// <summary>
        /// 监听线程
        /// </summary>
        Thread ListenThread;
        /// <summary>
        /// 服务端监听Socket
        /// </summary>
        Socket ServerSocket;
        /// <summary>
        /// 数据库控制器
        /// </summary>
        DataBaseController UnityDBController = new DataBaseController();

        #endregion

        public ServerForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Icon = UnityResource.Speleon;
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            if (UnityDBController.CreateConnection())
            {
                UnityModule.DebugPrint("数据库连接创建成功！");
            }
            else
            {
                //其实不用数据库，也可以转发两个同时在线的客户端的消息，但是不连接数据库时无法验证客户端登录，所以不允许启动
                UnityModule.DebugPrint("数据库连接创建失败！服务端无法启动！");

                //todo:Exit();方法无法关闭进程，可能有释放操作未处理，暂且先强行杀掉进程
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
            if (!ServerStartListen())
            {
                UnityModule.DebugPrint("服务端 Socket 创建失败！服务端无法启动！");

                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }

            ListenThread = new Thread(new ThreadStart(ListenClientConnect));
            ListenThread.Start();
        }

        private bool ServerStartListen()
        {
            try
            {
                //初始化服务端Socket，绑定端口并监听
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EndPoint TCPIPAndPort = new IPEndPoint(IPAddress.Any, UnityModule.ServerPort);
                ServerSocket.Bind(TCPIPAndPort);
                ServerSocket.Listen(MAX_CLIENT_COUNT);
                return true;
            }
            catch (Exception ex)
            {
                UnityModule.DebugPrint("服务端启动监听遇到错误：{0}", ex.Message);
                return false;
            }

        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //将所有用户在线状态置为假
            UnityDBController.ExecuteNonQuery("UPDATE UserBase SET OnLine = NO WHERE OnLine = YES");
            //关闭数据库连接
            UnityDBController.CloseConnection();

            try
            {
                foreach (Thread clientThread in ReceiveThreadDictionary.Values)
                {
                    clientThread?.Abort();
                }
                foreach (Socket clientSocket in SocketsDictionary.Values)
                {
                    clientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ServerShutdown)));
                    clientSocket.Close();
                }
                ServerSocket?.Close();
                ListenThread?.Abort();
            }
            catch { }
            

            Application.Exit();
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void ListenClientConnect()
        {
            while (true)
            {
                try
                {
                    Socket ClientSocket = ServerSocket.Accept();
                    Thread ReceiveMessageThread = new Thread(new ThreadStart(delegate { ReceiveClientMessage(ClientSocket); }));
                    ReceiveMessageThread.Start();
                    ReceiveThreadDictionary.Add(ClientSocket.RemoteEndPoint.ToString(), ReceiveMessageThread);

                    UnityModule.DebugPrint("同意 {0} 的请求，已开始接收消息...", ClientSocket.RemoteEndPoint.ToString());
                }
                catch (Exception ex)
                {
                    UnityModule.DebugPrint("创建Socket连接失败：{0}", ex.Message);
                }
            }
        }

        private void ReceiveClientMessage(Socket ClientSocket)
        {
            string USERID = "";
            while (true)
            {
                string ClientMessagePackage = "";
                try
                {
                    byte[] MessageBuffer = new byte[ClientSocket.ReceiveBufferSize - 1];
                    int MessageBufferSize = ClientSocket.Receive(MessageBuffer);
                    ClientMessagePackage = Encoding.UTF8.GetString(MessageBuffer, 0, MessageBufferSize);
                }
                catch (ThreadAbortException) { return; }
                catch (Exception ex)
                {
                    UnityModule.DebugPrint("接收客户端消息时遇到错误：{0}", ex.Message);
                    UserSignOut(USERID, ref ClientSocket);
                    UnityModule.DebugPrint("用户 {0} 异常下线，当前在线总数：{1}", USERID, SocketsDictionary.Count.ToString());
                    return;
                }

                string[] ClientMessages = ClientMessagePackage.Split('\n');
                foreach (string TempClientMessage in ClientMessages)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(TempClientMessage)) continue;

                        string ClientMessage = TempClientMessage + '\n';
                        UnityModule.DebugPrint("收到客户端发送来的数据：{0}", ClientMessage);
                        string MessagePattern = ProtocolFormatter.GetCMDTypePattern();
                        Regex MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match MessageMatchResult = MessageRegex.Match(ClientMessage);
                        string cmdType = MessageMatchResult.Groups["CMDTYPE"].Value.ToUpper();
                        switch (cmdType)
                        {
                            case "CHATMESSAGE":
                                {
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.ChatMessage);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ClientMessage);
                                    string ToID = MessageMatchResult.Groups["TOID"].Value;
                                    string Message = MessageMatchResult.Groups["MESSAGE"].Value;

                                    UnityModule.DebugPrint("消息来自:{0} 发送给:{1} 内容:{2}", USERID, ToID, Encoding.UTF8.GetString(Convert.FromBase64String(Message)));
                                    
                                    //判断对方是否连接
                                    if (SocketsDictionary.ContainsKey(ToID))
                                    {
                                        string MessageID = null;
                                        UnityDBController.ExecuteNonQuery("INSERT INTO CHATBASE (FromID,ToID,ChatTime,Message,SentYet) VALUES ('{0}','{1}','{2}','{3}',YES)", USERID, ToID, DateTime.UtcNow, Message);
                                        MessageID = UnityDBController.ExecuteScalar("SELECT MAX(ID) FROM CHATBASE WHERE FROMID='{0}'\n",USERID).ToString();
                                        SocketsDictionary[ToID].Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage, USERID,DateTime.UtcNow.ToString(),MessageID, Message)));
                                    }
                                    else
                                    {
                                        UnityDBController.ExecuteNonQuery("INSERT INTO CHATBASE (FromID,ToID,ChatTime,Message,SentYet) VALUES ('{0}','{1}','{2}','{3}',NO)", USERID,ToID,DateTime.UtcNow,Message);
                                    }

                                    break;
                                }
                            case "GETCHATHISTORY":
                                {
                                    //TODO:获取历史聊天记录
                                    break;
                                }
                            case "SIGNIN":
                                {
                                    //用户登录消息
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.SignIn);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ClientMessage);
                                    string UserID = MessageMatchResult.Groups["USERID"].Value;
                                    string Password = MessageMatchResult.Groups["PASSWORD"].Value;

                                    object CheckUserID = UnityDBController.ExecuteScalar("SELECT UserID FROM UserBase WHERE UserID='{0}' AND Password='{1}';", UserID, Password);
                                    if (CheckUserID != null)
                                    {
                                        //用户登陆成功！
                                        UnityModule.DebugPrint("用户登录成功！{0}", CheckUserID as string);
                                        ClientSocket.Send(Encoding.ASCII.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignInSuccessfully, CheckUserID as string)));
                                    }
                                    else
                                    {
                                        //用户发生一次错误的密码验证！
                                        UnityModule.DebugPrint("用户登录失败！{0}", UserID);
                                        ClientSocket.Send(Encoding.ASCII.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignInUnsuccessfully, UserID as string)));
                                    }
                                    //验证成功后关闭Socket
                                    ClientSocket?.Close();
                                    //验证完成后需要结束接收消息线程
                                    return;
                                }
                            case "WHOAMI":
                                {
                                    MessagePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.WhoAmI);
                                    MessageRegex = new Regex(MessagePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                    MessageMatchResult = MessageRegex.Match(ClientMessage);
                                    USERID = MessageMatchResult.Groups["USERID"].Value;
                                    if (SocketsDictionary.ContainsKey(USERID))
                                    {
                                        UnityModule.DebugPrint("用户 {0} 已经在 {1} 登录，即将被顶下线...", USERID, SocketsDictionary[USERID].RemoteEndPoint.ToString());
                                        Socket TempSocket = SocketsDictionary[USERID];
                                        TempSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.AnothorSignIn, USERID)));
                                        Thread TempThread = ReceiveThreadDictionary[TempSocket.RemoteEndPoint.ToString()];
                                        ReceiveThreadDictionary.Remove(TempSocket.RemoteEndPoint.ToString());
                                        TempThread.Abort();

                                        SocketsDictionary.Remove(USERID);
                                        TempSocket?.Close();
                                    }

                                    //以USERID为KEY，记录Socket
                                    SocketsDictionary.Add(USERID, ClientSocket);
                                    
                                    //好友登录
                                    UserSignIn(USERID,ref ClientSocket);

                                    UnityModule.DebugPrint("用户 {0} 在 {1} 上线，当前在线总数：{2}", USERID, ClientSocket.RemoteEndPoint.ToString(), SocketsDictionary.Count.ToString());
                                    break;
                                }
                            case "GETFRIENDSLIST":
                                {
                                    OleDbDataReader FriendsListReader = UnityDBController.ExecuteReader("SELECT UserID,NickName,Signature,OnLine FROM UserBase WHERE UserID IN(SELECT Guest FROM FriendBase WHERE Host ='{0}')", USERID);
                                    if (FriendsListReader != null && FriendsListReader.HasRows)
                                    {
                                        while (FriendsListReader.Read())
                                        {
                                            string FriendID = null;
                                            string NickName = null;
                                            string Signature = null;
                                            bool OnLine = false;
                                            try
                                            {
                                                FriendID = FriendsListReader["UserID"] as string;//??""很重要，否则封装协议消息时会因为string类型变量为引用而出错
                                                NickName = FriendsListReader["NickName"] as string ?? "(无昵称)";
                                                Signature = FriendsListReader["Signature"] as string ?? "(无签名)";
                                                OnLine = (bool)FriendsListReader["OnLine"];
                                            }
                                            catch (Exception ex)
                                            {
                                                UnityModule.DebugPrint("读取用户{0}的好友{1}信息时遇到错误：{2}", USERID, FriendID, ex.Message);
                                            }
                                            ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.GetFriendsList, FriendID, NickName, Signature,OnLine.ToString().ToUpper())));
                                        }
                                    }

                                    FriendsListReader.Close();
                                    //发送[好友列表发送完成]消息
                                    ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol( ProtocolFormatter.CMDType.FriendsListComplete,USERID)));
                                    break;
                                }
                            case "GETMESSAGENOTSENDYET":
                                {
                                    OleDbDataReader MessageNRYReader = UnityDBController.ExecuteReader("SELECT * FROM ChatBase WHERE ToID ='{0}' AND SentYet = NO", USERID);
                                    if (MessageNRYReader != null && MessageNRYReader.HasRows)
                                    {
                                        List<string> MessageIDList = new List<string>();
                                        while (MessageNRYReader.Read())
                                        {
                                            string MessageID = null;
                                            string FromID = null;
                                            string ChatTime = null;
                                            string ChatMessage = null;
                                            try
                                            {
                                                MessageID = MessageNRYReader["ID"].ToString();
                                                FromID = MessageNRYReader["FromID"] as string;
                                                ChatTime = ((DateTime)MessageNRYReader["ChatTime"]).ToString();
                                                ChatMessage = MessageNRYReader["Message"] as string;
                                                MessageIDList.Add(MessageID);
                                            }
                                            catch (Exception ex)
                                            {
                                                UnityModule.DebugPrint("读取消息ID{0}时遇到错误：{2}", MessageID, ex.Message);
                                            }
                                            ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage, FromID, ChatTime,MessageID, ChatMessage)));
                                        }
                                        MessageNRYReader?.Close();

                                        foreach (string messageID in MessageIDList)
                                            UnityDBController.ExecuteNonQuery("UPDATE ChatBase SET SentYet = YES WHERE ID = {0}", messageID);
                                    }

                                    MessageNRYReader?.Close();
                                    //发送[暂存消息发送完成]消息
                                    ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.MessageNSYComplete, USERID)));
                                    break;
                                }
                            case "SIGNOUT":
                                {
                                    UserSignOut(USERID,ref ClientSocket);
                                    UnityModule.DebugPrint("用户 {0} 正常下线，当前在线总数：{1}", USERID, SocketsDictionary.Count.ToString());
                                    return;
                                }
                            default:
                                {
                                    UnityModule.DebugPrint("遇到未知消息：{0}", ClientMessage);
                                    ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage,
                                        UnityModule.ServerNickName,DateTime.UtcNow.ToString(),"0", Convert.ToBase64String(Encoding.UTF8.GetBytes("服务端收到你发来未知的CMDTYPE：" + cmdType + "    消息原文：" + ClientMessage)))));
                                    break;
                                }
                        }
                    }
                    catch (ThreadAbortException) { return; }
                    catch (Exception ex)
                    {
                        UnityModule.DebugPrint("分析并回应用户消息时发生错误：{0}", ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 好友下线，更新Socket字典、Thread字典、数据库
        /// </summary>
        private void UserSignOut(string USERID,ref Socket ClientSocket)
        {
            //关闭 Socket 和 Thread
            if (USERID != "" && SocketsDictionary.ContainsKey(USERID))
                SocketsDictionary.Remove(USERID);
            if (ReceiveThreadDictionary.ContainsKey(ClientSocket?.RemoteEndPoint?.ToString()))
                ReceiveThreadDictionary.Remove(ClientSocket?.RemoteEndPoint?.ToString());
            ClientSocket?.Close();
            
            //更新数据库在线状态
            UnityDBController.ExecuteNonQuery("UPDATE UserBase SET OnLine = NO WHERE UserID = '{0}'", USERID);

            //向好友群发下线信息
            OleDbDataReader FriendOnLineReader = UnityDBController.ExecuteReader("SELECT UserID FROM UserBase WHERE OnLine = YES AND UserID IN (SELECT Guest FROM FriendBase WHERE HOST = '{0}')", USERID);
            if (FriendOnLineReader != null)
            {
                if (FriendOnLineReader.HasRows)
                {
                    while (FriendOnLineReader.Read())
                    {
                        string FriendID = FriendOnLineReader["UserID"] as string;
                        if (SocketsDictionary.ContainsKey(FriendID))
                        {
                            SocketsDictionary[FriendID].Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.FriendSignOut, USERID)));
                        }
                    }
                }
                FriendOnLineReader.Close();
            }
            UnityModule.DebugPrint("向在线好友群发注销消息完成。USERID = {0}",USERID);
        }

        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="USERID"></param>
        /// <param name="ClientSocket"></param>
        private void UserSignIn(string USERID,ref Socket ClientSocket)
        {
            //发送问候语
            ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage,
                                        UnityModule.ServerNickName, DateTime.UtcNow.ToString(), "0", Convert.ToBase64String(Encoding.UTF8.GetBytes("你好,\n欢迎登录 Speleon !")))));

            //数据库更新用户上线状态
            UnityDBController.ExecuteNonQuery("UPDATE UserBase SET OnLine = YES WHERE UserID = '{0}'", USERID);

            //向好友群发上线信息
            OleDbDataReader FriendOnLineReader = UnityDBController.ExecuteReader("SELECT UserID FROM UserBase WHERE OnLine = YES AND UserID IN (SELECT Guest FROM FriendBase WHERE HOST = '{0}')",USERID);
            if (FriendOnLineReader != null)
            {
                if (FriendOnLineReader.HasRows)
                {
                    while (FriendOnLineReader.Read())
                    {
                        string FriendID = FriendOnLineReader["UserID"] as string;
                        if (SocketsDictionary.ContainsKey(FriendID))
                        {
                            SocketsDictionary[FriendID].Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol( ProtocolFormatter.CMDType.FriendSignIn,USERID)));
                        }
                    }
                }
                FriendOnLineReader.Close();
            }
            UnityModule.DebugPrint("向在线好友群发登录消息完成。USERID = {0}", USERID);
        }

        private void LogListBox_DoubleClick(object sender, EventArgs e)
        {
            if (LogListBox.SelectedIndex > -1)
                MessageBox.Show(LogListBox.Items[LogListBox.SelectedIndex].ToString());
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            string TargetUserID = "88888";
            ProtocolFormatter.CMDType ProtocolType = ProtocolFormatter.CMDType.ChatMessage;
            string[] DebugInfo = new string[] {"66666", DateTime.Now.ToString(), "1",Convert.ToBase64String(Encoding.UTF8.GetBytes("Test message"))};

            if(SocketsDictionary.ContainsKey(TargetUserID))
                SocketsDictionary[TargetUserID].Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolType, DebugInfo)));
        }
    }
}
