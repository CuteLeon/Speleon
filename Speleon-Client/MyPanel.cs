using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Speleon_Client
{
    class MyPanel:Panel
    {
        /// <summary>
        /// 此类用于扩展 Forms.Panel 防止窗体改变大小时频繁闪烁
        /// </summary>
        public MyPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
    }
}
