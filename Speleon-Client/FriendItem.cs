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

    //TODO:删除好友协议使用：移除方法：((IDisposable)sender).Dispose();
    public partial class FriendItem : UserControl, IDisposable
    {
        /// <summary>
        /// 点击 FriendItem
        /// </summary>
        public event EventHandler FriendItemClick;

        /// <summary>
        /// 被激活的Item改变时发生
        /// </summary>
        public static event EventHandler<FriendItem> ActiveItemChanged;

        /// <summary>
        /// 与FriendItem关联的聊天气泡容器
        /// </summary>
        public MyFlowLayoutPanel ChatBubblesPanel = null;

        /// <summary>
        /// 放置 FrinedItem 的流式布局容器
        /// </summary>
        public static MyFlowLayoutPanel ParentPanel = null;

        private static FriendItem activeFriend = null;
        /// <summary>
        /// 当前激活的 FriendItem
        /// </summary>
        public static FriendItem ActiveFriend {
            get => activeFriend;
            set{
                UnityModule.DebugPrint("好友列表激活项改变");
                if (activeFriend != null)
                {
                    activeFriend.NickNameLabel.ForeColor = Color.Black;
                    activeFriend.BackColor = Color.WhiteSmoke;
                    activeFriend.Invalidate();
                }
                if (value != null)
                {
                    value.NickNameLabel.ForeColor = Color.DeepSkyBlue;
                    value.BackColor = Color.LightGray;
                    value.Invalidate();

                }

                if (ActiveItemChanged != null)ActiveItemChanged(activeFriend??null,value??null);
                activeFriend = value;
            }
        }

        private bool onLine=false;
        /// <summary>
        /// 好友是否在线
        /// </summary>
        public bool OnLine
        {
            get => onLine;
            set{
                onLine = value;
                if (value)
                {
                    //在线状态
                    NickNameLabel.Font = new Font(NickNameLabel.Font, FontStyle.Regular);
                    HeadPortraitLabel.Enabled = true;
                }
                else
                {
                    //离线状态
                    NickNameLabel.Font = new Font(NickNameLabel.Font,FontStyle.Italic);
                    HeadPortraitLabel.Enabled = false;
                }
            }
        }

        /// <summary>
        /// FriendID和FriendItem字典
        /// </summary>
        private static Dictionary<string, FriendItem> FriendDictionary = new Dictionary<string, FriendItem>();

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

        private string signature = null;
        /// <summary>
        /// 读取或设置好友签名
        /// </summary>
        public string Signature
        {
            get => signature;
            set
            {
                signature = value;
                if (string.IsNullOrEmpty(chatDraft)) SignatureLabel.Text = value;
            }
        }

        private string chatDraft = null;
        /// <summary>
        /// 聊天草稿，切换聊天好友时，记录输入的草稿
        /// </summary>
        public string ChatDraft
        {
            get => chatDraft;
            set
            {
                if (chatDraft != value)
                {
                    chatDraft = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        SignatureLabel.ForeColor = Color.Tomato;
                        SignatureLabel.Font = new Font(SignatureLabel.Font.FontFamily, 10, FontStyle.Regular);
                        SignatureLabel.Text = "草稿：" + chatDraft;
                    }
                    else
                    {
                        SignatureLabel.ForeColor = Color.DimGray;
                        SignatureLabel.Font = new Font(SignatureLabel.Font.FontFamily, 9, FontStyle.Regular);
                        SignatureLabel.Text = Signature;
                    }
                }
            }
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
        public FriendItem(string FriendID, string nickName, string signature,bool isOnLine)
        {
            InitializeComponent();

            //维护FriendID和FriendItem字典
            if (!string.IsNullOrEmpty(FriendID))
            {
                _friendID = FriendID;
                NickName = nickName;
                Signature = signature;
                if(!FriendDictionary.ContainsKey(FriendID)) FriendDictionary.Add(FriendID, this);
                OnLine = isOnLine;

                MouseEnter += new System.EventHandler(Controls_MouseEnter);
                MouseLeave += new System.EventHandler(Controls_MouseLeave);
                Click += new System.EventHandler(FriendItem_Click);

                HeadPortraitLabel.MouseLeave += new System.EventHandler(Controls_MouseLeave);
                HeadPortraitLabel.Click += new System.EventHandler(FriendItem_Click);

                NickNameLabel.MouseLeave += new System.EventHandler(Controls_MouseLeave);
                NickNameLabel.Click += new System.EventHandler(FriendItem_Click);

                SignatureLabel.MouseLeave += new System.EventHandler(Controls_MouseLeave);
                SignatureLabel.Click += new System.EventHandler(FriendItem_Click);

                UnityModule.DebugPrint("以 FriendID 为KEY，将好友信息添加进FriendItem字典");
            }
            else
            {
                UnityModule.DebugPrint("FriendID为空，无法创建 FriendItem");
                throw new Exception("FriendID为空，无法创建 FriendItem！");
            }
        }

        /// <summary>
        /// 继承自 IDisposable 接口，释放托管资源并自动从Friend字典里移除此项（注意：必须使用 IDisposable 接口调用此方法）
        /// </summary>
        void IDisposable.Dispose()
        {
            try
            {
                if(FriendDictionary.ContainsKey(_friendID)) FriendDictionary.Remove(_friendID);
                if (ParentPanel != null)
                {
                    int ThisIndex = ParentPanel.Controls.GetChildIndex(this);
                    ParentPanel.Controls.Remove(this);
                    if (ParentPanel.Controls.Count > 0)
                    {
                        UnityModule.DebugPrint("FriendItem 移除成功，正在激活临近 FriendItem");
                        ActiveFriend = ParentPanel.Controls[Math.Min(ThisIndex, ParentPanel.Controls.Count - 1)] as FriendItem;
                    }
                }
                ChatBubblesPanel.Controls.Clear();
                ChatBubblesPanel.Dispose();
            }
            catch (Exception ex)
            {
                UnityModule.DebugPrint("FriendItem[{0}]释放托管内存前预处理时出错：{1}", _friendID, ex.Message);
            }

            base.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 判断指定FriendID是否已经存在
        /// </summary>
        /// <param name="FriendID">FriendID</param>
        /// <returns>FriendID是否已经存在</returns>
        static public bool FriendExisted(string FriendID)
        {
            if (FriendDictionary == null) return false;
            try
            {
                return FriendDictionary.ContainsKey(FriendID);
            }
            catch (Exception ex)
            {
                UnityModule.DebugPrint("查询 FriendID 是否已经存在时遇到错误：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据FriendID获得对应的FriendItem
        /// </summary>
        /// <param name="FriendID">FriendID</param>
        /// <returns>FriendID对应的FriendItem（不存在时返回null）</returns>
        static public FriendItem GetFriendItemByFriendID(string FriendID)
        {
            FriendItem TargetFriendItem;
            return (FriendDictionary.TryGetValue(FriendID, out TargetFriendItem) ? TargetFriendItem : null);
        }
        
        /// <summary>
        /// 修改好友信息
        /// </summary>
        /// <param name="nickName">好友昵称</param>
        /// <param name="signature">好友签名</param>
        public void SetNickNameSignatureAndOnLine(string nickName, string signature,bool onLine)
        {
            UnityModule.DebugPrint("修改好友信息：{0} / {1}", NickName, Signature);
            NickName = nickName;
            Signature = signature;
            OnLine = onLine;
        }

        /// <summary>
        /// 绘制图案
        /// </summary>
        private void FriendItem_Paint(object sender, PaintEventArgs e)
        {
            //todo:这里会引发 Program.Main()=>{Application.Run()}出错
            return;
            try
            {
                this.Invoke(new Action(() =>{
                    Application.DoEvents();
                    using (Graphics MyGraphics = e.Graphics)
                        MyGraphics.DrawLine(Pens.Gray, 0, 0, this.Width - 1, 0);
                }));
            }
            catch{ }
        }

        /// <summary>
        /// 鼠标进入方法
        /// </summary>
        private void Controls_MouseEnter(object sender, EventArgs e)
        {
            if (activeFriend != this) this.BackColor = Color.Gainsboro;
        }

        /// <summary>
        /// 鼠标离开方法
        /// </summary>
        private void Controls_MouseLeave(object sender, EventArgs e)
        {
            //鼠标在用户控件内部的控件间移动而不重复触发MouseLeave的关键！
            if (!this.RectangleToScreen(this.DisplayRectangle).Contains(MousePosition))
            {
                this.BackColor = activeFriend == this ? Color.LightGray : Color.WhiteSmoke;
            }
        }

        /// <summary>
        /// FriendItem 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FriendItem_Click(object sender, EventArgs e)
        {
            if (activeFriend != this) ActiveFriend = this;
            if (FriendItemClick != null) FriendItemClick(this, e);
        }

    }
}
