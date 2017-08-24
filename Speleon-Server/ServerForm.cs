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
                Socket client = ServerSocket.Accept();
                byte[] a = new byte[1024];
                client.Receive(a);
                //删除结束字符
                string login = Encoding.ASCII.GetString(a).Trim('\0');
                MessageBox.Show(login);


                // .*? 任意匹配
                //(?<目标>.+?)  目标
                string TitlePattern = "HEY_CVER=(?<clientversion>.+?)_CMDTYPE=(?<cmdtype>.+?)_USERID=(?<userid>.+?)_PASSWORD=(?<password>.+?)\n";
                Regex ItemRegex = new Regex(TitlePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match ItemMatchResult = ItemRegex.Match(login);

                switch (ItemMatchResult.Groups["cmdtype"].Value)
                {
                    case "LOGIN":
                        {
                            object check = UnityDBControl.ExecuteScalar("select USERID from userbase where USERID='{0}' and password='{1}';",
                                ItemMatchResult.Groups["userid"].Value,
                                ItemMatchResult.Groups["password"].Value);
                            if (check == null)
                            {
                                UnityModule.DebugPrint("错误的密码验证！");
                            }
                            else
                            {
                                client.Send(Encoding.ASCII.GetBytes(check as string));
                            }
                            break;
                        }
                }

                
                
                client.Close();
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerSocket?.Close();
            ListenThread?.Abort();
        }
    }
}
