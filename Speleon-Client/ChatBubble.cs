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
        public readonly string FromID;
        /// <summary>
        /// 消息内容
        /// </summary>
        public readonly string MessageContext;
        
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
        /// <param name="fromID">发信人ID</param>
        /// <param name="messageContext">消息内容</param>
        public ChatBubble(string messageID,string chatTime,string fromID,string messageContext)
        {
            MessageID = messageID;
            ChatTime = chatTime;
            FromID = fromID;
            MessageContext = messageContext;
        }



    }
}
