using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Client
{
    public partial class ClientForm : Form
    {

        Socket UnitySocket;

        public ClientForm()
        {
            this.Icon = UnityResource.Speleon;
            InitializeComponent();
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void labelButton1_Click(object sender, EventArgs e)
        {

        }

        private void ClientForm_Shown(object sender, EventArgs e)
        {
            try
            {
                UnitySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //UnitySocket.Connect("localhost", 17417);
            }
            catch(Exception ex)
            {
                new MyMessageBox("连接服务器遇到错误：{0}",ex.Message).ShowDialog(this);
            }
        }
    }
}
