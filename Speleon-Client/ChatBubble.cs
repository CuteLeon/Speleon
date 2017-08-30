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
    public partial class ChatBubble : UserControl
    {
        /// <summary>
        /// 发信人是不是自己
        /// </summary>
        public readonly bool IsMine;
        /// <summary>
        /// 消息ID
        /// </summary>
        public readonly string MessageID;
        /// <summary>
        /// 聊天时间
        /// </summary>
        public readonly string ChatTime;
        /// <summary>
        /// 发信人ID
        /// </summary>
        public readonly string SenderID;
        /// <summary>
        /// 消息内容
        /// </summary>
        public readonly string MessageContext;

        private int MaxWidth
        {
            get => this.MaximumSize.Width;
            set
            {
                this.MaximumSize = new Size(value, MaximumSize.Height);
                MessageContextLabel.MaximumSize =new Size( value-HeadPortraitLabel.Width-this.Padding.Left-this.Padding.Right,MessageContextLabel.MaximumSize.Height);
            }
        }

        /// <summary>
        /// 聊天气泡
        /// </summary>
        private ChatBubble()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造聊天气泡
        /// </summary>
        /// <param name="messageID">消息ID</param>
        /// <param name="chatTime">聊天时间</param>
        /// <param name="senderID">发信人ID</param>
        /// <param name="messageContext">消息内容</param>
        /// <param name="isMine">发信人是不是自己</param>
        public ChatBubble(string messageID,string chatTime,string senderID,string messageContext,int maxWidth,bool isMine)
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            
            MessageID = messageID;
            ChatTime = chatTime;
            SenderID = senderID;
            MessageContext = messageContext;
            IsMine = isMine;
            MaxWidth = maxWidth;

            ChatTimeLabel.Text = chatTime;
            MessageContextLabel.Text = messageContext;

            if (isMine)
            {
                this.Dock = DockStyle.Right;
                ChatTimeLabel.TextAlign = ContentAlignment.MiddleRight;
                HeadPortraitLabel.Dock = DockStyle.Right;
                MessageContextLabel.Dock = DockStyle.Right;
                this.MinimumSize =new Size(this.MinimumSize.Width, MessageContextLabel.Bottom + this.Padding.Bottom);
                HeadPortraitLabel.Padding = new Padding(8, 0, 0, 0);
            }
            else
            {
                this.Dock = DockStyle.Left;
                MessageContextLabel.BackColor = Color.Pink;
                HeadPortraitLabel.Padding = new Padding(0, 0, 8, 0);
            }


        }

    }
}
