using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Speleon_Client
{
    static class UnityModule
    {
        static public string USERID = "";

        static public string ServerIP = "localhost";
        //static public string ServerIP = "server.ngrok.cc";
        static public int ServerPort = 17417;

        //用于绘制窗体阴影
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);

        //用于鼠标拖动无边框窗体
        [DllImportAttribute("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")] public static extern bool ReleaseCapture();
        private const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        
        //鼠标改变窗体大小消息常量
        public const int WM_NCHITTEST = 0x0084;
        public const int HT_LEFT = 10;
        public const int HT_RIGHT = 11;
        public const int HT_TOP = 12;
        public const int HT_TOPLEFT = 13;
        public const int HT_TOPRIGHT = 14;
        public const int HT_BOTTOM = 15;
        public const int HT_BOTTOMLEFT = 16;
        public const int HT_BOTTOMRIGHT = 17;

        /// <summary>
        /// 为窗体绘制阴影
        /// </summary>
        /// <param name="HostForm"></param>
        static public void DrawWindowShadow(Form HostForm)
        {
            SetClassLong(HostForm.Handle, GCL_STYLE, GetClassLong(HostForm.Handle, GCL_STYLE) | CS_DropSHADOW);
        }

        /// <summary>
        /// 注册以帮助鼠标拖动无边框窗体
        /// </summary>
        static public void MoveFormViaMouse(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage((sender is Form ? (sender as Form).Handle : (sender as Control).FindForm().Handle), WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// 封装的函数以输出调试信息
        /// </summary>
        /// <param name="DebugMessage">调试信息</param>
        static public void DebugPrint(string DebugMessage)
        {
            Debug.Print(string.Format("客户端{0}：{1}    {2}", USERID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), DebugMessage));
        }

        /// <summary>
        /// 封装的函数以输出调试信息
        /// </summary>
        /// <param name="DebugMessage">调试信息</param>
        /// <param name="DebugValue">调试信息的值</param>
        static public void DebugPrint(string DebugMessage, params object[] DebugValue)
        {
            DebugPrint(string.Format(DebugMessage, DebugValue));
        }

    }
}
