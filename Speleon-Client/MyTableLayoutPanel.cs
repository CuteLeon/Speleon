using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Client
{
    public partial class MyTableLayoutPanel : TableLayoutPanel
    {
        /// <summary>
        /// 此类用于扩展 Forms.MyFlowLayoutPanel 防止窗体改变大小时频繁闪烁
        /// </summary>
        public MyTableLayoutPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
