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
    public partial class FriendItem : UserControl
    {

        /// <summary>
        /// 放置 FrinedItem 的流式布局容器
        /// </summary>
        public static MyFlowLayoutPanel ParentPanel = null;

        /// <summary>
        /// 当前激活的 FriendItem
        /// </summary>
        public static FriendItem ActiveFrined = null;

        /// <summary>
        /// FriendID和FriendItem字典
        /// </summary>
        private static Dictionary<string, FriendItem> DADictionary = new Dictionary<string, FriendItem>();

        /// <summary>
        /// 好友ID (只读属性)
        /// </summary>
        private readonly string _friendID = "";
        /// <summary>
        /// 获取好友ID
        /// </summary>
        public string FriendID {
            get => _friendID;
        }

        /// <summary>
        /// 读取或设置好友昵称
        /// </summary>
        public string NickName {
            get => NickNameLabel.Text;
            set => NickNameLabel.Text = value;
        }

        /// <summary>
        /// 读取或设置好友签名
        /// </summary>
        public string Signature
        {
            get => SignaturLabel.Text;
            set => SignaturLabel.Text = value;
        }

        /// <summary>
        /// 好友条目
        /// </summary>
        private FriendItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FriendID">好友ID</param>
        /// <param name="NickName">昵称</param>
        /// <param name="Signature">签名</param>
        public FriendItem(string FriendID, string nickName, string signature)
        {
            InitializeComponent();

            _friendID = FriendID;
            NickName = nickName;
            Signature = signature;
        }



    }
}
