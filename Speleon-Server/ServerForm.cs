using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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
        DataBaseController UnityDBControl = new DataBaseController();

        #endregion

        public ServerForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Icon = UnityResource.Speleon;
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            if (UnityDBControl.CreateConnection())
            {
                UnityModule.DebugPrint("数据库连接创建成功！");
            }
            else
            {
                MessageBox.Show("数据库连接创建失败！服务端无法启动！");
                Application.Exit();
            }
            if (!ServerStartListen())
            {
                MessageBox.Show("服务端 Socket 创建失败！服务端无法启动！");
                return;
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
            foreach (Thread clientThread in ReceiveThreadDictionary.Values)
            {
                clientThread.Abort();
            }
            foreach (Socket clientSocket in SocketsDictionary.Values)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ServerShutdown)));
                clientSocket.Close();
            }
            ServerSocket?.Close();
            ListenThread?.Abort();
            UnityDBControl.CloseConnection();
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
                string ClientMessage = "";
                try
                {
                    byte[] MessageBuffer = new byte[ClientSocket.ReceiveBufferSize - 1];
                    int MessageBufferSize = ClientSocket.Receive(MessageBuffer);
                    ClientMessage = Encoding.UTF8.GetString(MessageBuffer, 0, MessageBufferSize);
                }
                catch (ThreadAbortException) { return; }
                catch (Exception ex)
                {
                    UnityModule.DebugPrint("接收客户端消息时遇到错误：{0}", ex.Message);
                    //todo:断开与客户端的连接，需要将用户置为离线
                    if (USERID != "" && SocketsDictionary.ContainsKey(USERID))
                        SocketsDictionary.Remove(USERID);
                    if (ReceiveThreadDictionary.ContainsKey(ClientSocket?.RemoteEndPoint?.ToString()))
                        ReceiveThreadDictionary.Remove(ClientSocket?.RemoteEndPoint?.ToString());
                    UnityModule.DebugPrint("用户 {0} 下线，当前在线总数：{1}", USERID, SocketsDictionary.Count.ToString());

                    ClientSocket?.Close();
                    return;
                }

                try
                {
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
                                string Message = Encoding.UTF8.GetString(Convert.FromBase64String(MessageMatchResult.Groups["MESSAGE"].Value as string));
                                UnityModule.DebugPrint("消息来自:{0} 发送给:{1} 内容:{2}", USERID, ToID, Message);
                                //todo:转发消息

                                //判断对方是否连接
                                if (SocketsDictionary.ContainsKey(ToID))
                                {
                                    //todo:把消息储存进数据库
                                    SocketsDictionary[ToID].Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage, USERID, Message)));
                                }
                                else
                                {
                                    //todo:判断用户是否注册？=>把消息寄存在数据库，未发送标志记为真，等待用户上线发送给用户
                                }

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

                                object CheckUserID = UnityDBControl.ExecuteScalar("SELECT UserID FROM UserBase WHERE UserID='{0}' AND Password='{1}';", UserID, Password);
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
                                ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage,
                                    UnityModule.ServerNickName, "你好,\n欢迎登录 Speleon !")));

                                UnityModule.DebugPrint("用户 {0} 在 {1} 上线，当前在线总数：{2}", USERID, ClientSocket.RemoteEndPoint.ToString(), SocketsDictionary.Count.ToString());
                                break;
                            }
                        case "GETFRIENDSLIST":
                            {
                                OleDbDataReader FriendsListReader = UnityDBControl.ExecuteReader("SELECT UserID,NickName,Signature FROM UserBase WHERE UserID IN(SELECT Guest FROM FriendBase WHERE Host ='{0}')",USERID);
                                //查询为空即返回，注意不要return，仅break；
                                if (FriendsListReader == null || !FriendsListReader.HasRows) break;
                                while (FriendsListReader.Read())
                                {
                                    string FriendID = "";
                                    string NickName = "";
                                    string Signature = "";
                                    try
                                    {
                                        FriendID = FriendsListReader["UserID"] as string;
                                        NickName = FriendsListReader["NickName"] as string;
                                        Signature = FriendsListReader["Signature"] as string;
                                    }
                                    catch (Exception ex)
                                    {
                                        UnityModule.DebugPrint("读取用户{0}的好友{1}信息时遇到错误：{2}",USERID,FriendID,ex.Message);
                                    }
                                    ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol( ProtocolFormatter.CMDType.GetFriendsList,FriendID,NickName,Signature)));
                                }
                                break;
                            }
                        default:
                            {
                                UnityModule.DebugPrint("遇到未知消息：{0}", ClientMessage);
                                ClientSocket.Send(Encoding.UTF8.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.ChatMessage,
                                    "ServerSystem", "服务端收到你发来未知的CMDTYPE："+cmdType + "\n" + ClientMessage)));
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
}
