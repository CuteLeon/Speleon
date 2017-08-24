using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        Thread ListenThread;
        Socket ServerSocket;
        DataBaseController UnityDBControl = new DataBaseController();

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
            }
            ListenThread = new Thread(new ThreadStart(ServerStartListen));
            ListenThread.Start();
        }

        private void ServerStartListen()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint point = new IPEndPoint(IPAddress.Any, 17417);
            ServerSocket.Bind(point);
            ServerSocket.Listen(10);

            while (true)
            {
                Socket ClientSocket = ServerSocket.Accept();
                byte[] ClientData = new byte[1024];
                ClientSocket.Receive(ClientData);
                //删除结束字符
                string ClientProtocol = Encoding.ASCII.GetString(ClientData).Trim('\0');

                string TitlePattern = ProtocolFormatter.GetProtocolPattern(ProtocolFormatter.CMDType.SignIn);
                Regex ItemRegex = new Regex(TitlePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match ItemMatchResult = ItemRegex.Match(ClientProtocol);
                string cmdType = ItemMatchResult.Groups["CMDTYPE"].Value.ToUpper();
                string UserID = ItemMatchResult.Groups["USERID"].Value;
                string Password = ItemMatchResult.Groups["PASSWORD"].Value;

                switch (cmdType)
                {
                    case "SIGNIN":
                        {
                            object USERID = UnityDBControl.ExecuteScalar("select USERID from userbase where USERID='{0}' and password='{1}';",UserID,Password);
                            if (USERID != null)
                            {
                                //用户登陆成功！
                                UnityModule.DebugPrint("用户登录成功！{0}",USERID as string);
                                ClientSocket.Send(Encoding.ASCII.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignInSuccessfully, USERID as string)));
                            }
                            else
                            {
                                //用户发生一次错误的密码验证！
                                UnityModule.DebugPrint("用户登录失败！{0}", UserID);
                                ClientSocket.Send(Encoding.ASCII.GetBytes(ProtocolFormatter.FormatProtocol(ProtocolFormatter.CMDType.SignInUnsuccessfully, UserID as string)));
                            }
                            break;
                        }
                }

                
                
                ClientSocket.Close();
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerSocket?.Close();
            ListenThread?.Abort();
        }
    }
}
