using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Speleon_Client
{
    class MyFlowLayoutPanel : FlowLayoutPanel
    {
        /// <summary>
        /// 此类用于扩展 Forms.MyFlowLayoutPanel 防止窗体改变大小时频繁闪烁
        /// </summary>
        public MyFlowLayoutPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
