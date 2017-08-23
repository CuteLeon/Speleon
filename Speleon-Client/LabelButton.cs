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
    public partial class LabelButton : Label
    {
        public LabelButton()
        {
            InitializeComponent();

            this.MouseEnter += new EventHandler(delegate(object s,EventArgs e) {(s as LabelButton).Image = UnityResource.ResourceManager.GetObject((s as LabelButton).Tag as string + "_1") as Image;});
            this.MouseLeave += new EventHandler(delegate (object s, EventArgs e) { (s as LabelButton).Image = UnityResource.ResourceManager.GetObject((s as LabelButton).Tag as string + "_0") as Image; });
            this.MouseDown += new MouseEventHandler(delegate (object s, MouseEventArgs e) {(s as LabelButton).Image = UnityResource.ResourceManager.GetObject((s as LabelButton).Tag as string + "_2") as Image;});
            this.MouseUp += new MouseEventHandler(delegate (object s, MouseEventArgs e) { (s as LabelButton).Image = UnityResource.ResourceManager.GetObject((s as LabelButton).Tag as string + "_1") as Image; });
        }

    }
}
