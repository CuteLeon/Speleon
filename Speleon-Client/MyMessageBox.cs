using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speleon_Client
{
    public partial class MyMessageBox : Form
    {
        /// <summary>
        /// 图标类型枚举
        /// </summary>
        public enum IconType
        {
            /// <summary>
            /// 提示信息
            /// </summary>
            Info,
            /// <summary>
            /// 询问
            /// </summary>
            Question,
            /// <summary>
            /// 警告
            /// </summary>
            Warning,
            /// <summary>
            /// 错误
            /// </summary>
            Error
        }

        #region "构造函数"
        private MyMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 弹出提示窗口
        /// </summary>
        /// <param name="MessageText">消息文本</param>
        /// <param name="iconType">图表类型</param>
        public MyMessageBox(string MessageText, IconType iconType= IconType.Info) : this(MessageText, "Speleon：", iconType){ }

        /// <summary>
        /// 弹出提示窗口
        /// </summary>
        /// <param name="MessageText">消息文本</param>
        /// <param name="Title">消息标题</param>
        /// <param name="iconType">图表类型</param>
        public MyMessageBox(string MessageText,string Title,IconType iconType= IconType.Info)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            TitleLabel.Text = Title;
            MessageLabel.Text = MessageText;
            IconLabel.Image = UnityResource.ResourceManager.GetObject(iconType.ToString()) as Image;
            if (iconType == IconType.Question)
            {
                CancelButton.Left = (this.Width - CancelButton.Width * 2 - 20)/2;
                OKButton.Left = CancelButton.Right + 20;
            }
            else
            {
                CancelButton.Hide();
                OKButton.Left =(this.Width-OKButton.Width)/2;
            }

            //为窗体增加阴影
            UnityModule.DrawWindowShadow(this);
        }
        #endregion

        #region "按钮动态效果"
        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Label).Image = UnityResource.ResourceManager.GetObject((sender as Label).Tag + "_2") as Image;
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            (sender as Label).Image = UnityResource.ResourceManager.GetObject((sender as Label).Tag + "_1") as Image;
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            (sender as Label).Image = UnityResource.ResourceManager.GetObject((sender as Label).Tag + "_0") as Image;
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as Label).Image = UnityResource.ResourceManager.GetObject((sender as Label).Tag + "_1") as Image;
        }
        #endregion

        #region "按钮点击事件"
        private void CloseButton_Click(object sender, EventArgs e)
        {
            HideMe(DialogResult.Cancel);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            HideMe(DialogResult.OK);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            HideMe(DialogResult.Cancel);
        }
        #endregion

        #region "窗体事件"
        private void MyMessageBox_Load(object sender, EventArgs e)
        {
            this.Icon = UnityResource.Speleon;
            //注册鼠标拖动功能
            TitleLabel.MouseDown += new MouseEventHandler(UnityModule.MoveFormViaMouse);

            OKButton.MouseEnter += new EventHandler(Button_MouseEnter);
            OKButton.MouseLeave += new EventHandler(Button_MouseLeave);
            OKButton.MouseDown += new MouseEventHandler(Button_MouseDown);
            OKButton.MouseUp += new MouseEventHandler(Button_MouseUp);

            CancelButton.MouseEnter += new EventHandler(Button_MouseEnter);
            CancelButton.MouseLeave += new EventHandler(Button_MouseLeave);
            CancelButton.MouseDown += new MouseEventHandler(Button_MouseDown);
            CancelButton.MouseUp += new MouseEventHandler(Button_MouseUp);
        }

        private void MyMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                HideMe(DialogResult.Cancel);
            else if (e.KeyCode == Keys.Enter)
                HideMe(DialogResult.OK);
        }

        private void MyMessageBox_Paint(object sender, PaintEventArgs e)
        {
            using (Brush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(255, 71, 194, 255), Color.FromArgb(255, 16, 216, 110), LinearGradientMode.ForwardDiagonal))
            {
                //绘制渐变
                e.Graphics.FillRectangle(linearGradientBrush, this.ClientRectangle);
            }
        }
        #endregion

        #region "功能函数"
        /// <summary>
        /// 动态隐藏窗体并返回对话框DialogResult
        /// </summary>
        /// <param name="TargetDialogResult">对话框返回值</param>
        private void HideMe(DialogResult TargetDialogResult)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate{
                while (this.Opacity > 0)
                {
                    this.Opacity -= 0.1;
                    this.Top -= 1;
                    Thread.Sleep(10);
                }
                this.Invoke(new Action(()=> {
                    this.DialogResult = (DialogResult)TargetDialogResult;
                    //this.Close(); 方法使MyMessageBox()在.Show()方法时也可以被关闭
                    this.Close();
                }));
            }));
        }

        /// <summary>
        /// 弹出需要用户输入内容的 InputBox
        /// </summary>
        /// <param name="InputBoxTips">输入内容的提示信息</param>
        /// <param name="UserInput">用于接收数据的字符串对象（ref 传址）</param>
        /// <param name="DefaultString">默认字符串</param>
        /// <param name="MaxLength">最大文本长度</param>
        /// <param name="Owner">模态窗口的父窗口</param>
        /// <returns>输入框返回值（判断用户是否取消了输入）</returns>
        static public DialogResult ShowInputBox(string InputBoxTips,ref string UserInput,string DefaultString,int MaxLength,Form Owner=null)
        {
            MyMessageBox InputBoxForm = new MyMessageBox(InputBoxTips,"请输入信息：",IconType.Question);
            InputBoxForm.MessageLabel.Height = 70;
            InputBoxForm.InputTextBox.Show();
            InputBoxForm.InputTextBox.Text = DefaultString;
            InputBoxForm.InputTextBox.MaxLength = MaxLength;
            InputBoxForm.InputTextBox.KeyDown += new KeyEventHandler(delegate(object x,KeyEventArgs y) {
                if (y.KeyCode == Keys.Escape)
                    InputBoxForm.CancelButton_Click(InputBoxForm.CancelButton,new EventArgs());
                else if (y.KeyCode == Keys.Enter)
                    InputBoxForm.OKButton_Click(InputBoxForm.OKButton, new EventArgs());
            });
            DialogResult InputBoxDialogResult = InputBoxForm.ShowDialog(Owner);
            if (InputBoxDialogResult == DialogResult.OK)
            {
                UserInput = InputBoxForm.InputTextBox.Text;
            }
            return InputBoxDialogResult;
        }
        #endregion

        private void MyMessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                e.Cancel = true;
                HideMe(DialogResult.Cancel);
            }
        }
    }
}
