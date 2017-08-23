using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                byte[] a = new byte[256];
                client.Receive(a);
                string login = Encoding.ASCII.GetString(a);
                string[] b= login.Split('#');
                object check= UnityDBControl.ExecuteScalar("select USERID from userbase where USERID='{0}'", b[0]);
                if (check == null)
                {
                    MessageBox.Show("null");
                }
                else
                {
                    client.Send(Encoding.ASCII.GetBytes(check as string));
                }
                /*
                byte[] bitLen = new byte[8];
                client.Receive(bitLen, bitLen.Length, SocketFlags.None);
                long contentLen = BitConverter.ToInt64(bitLen, 0);
                byte[] bits = new byte[256];
                int r = client.Receive(bits, bits.Length, SocketFlags.None);
                */

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
