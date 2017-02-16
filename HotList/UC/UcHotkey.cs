using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    /// <summary>
    /// User control for editing the values of a Hotkey
    /// </summary>
    public partial class UcHotkey : UserControl
    {
        private bool IsLoaded = false;
        public UcHotkey()
        {
            InitializeComponent();
            ddlKeys1.DataSource = Enum.GetNames(typeof(HotkeysEnum));
            ddlKeys2.DataSource = Enum.GetNames(typeof(HotkeysEnum));
        }

        protected internal EventHandler ItemChanged;
        private bool ChangeEventEnabled = false;
        HotkeyKeys m_HotKey;
        /// <summary>
        /// Specifies the Hotkey used for the control
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HotkeyKeys HotKey
        {
            get
            {
                return m_HotKey;
            }
            set
            {
                this.m_HotKey = value;
               
                this.ChangeEventEnabled = false;
                this.BindControls();
                this.ChangeEventEnabled = true;
            }
        }

        private void UcHotkey_Load(object sender, EventArgs e)
        {
            this.IsLoaded = true;
        }

        private void ddlKeys1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoaded == false)
            {
                return;
            }
            HotkeysEnum h;
            if (Enum.TryParse<HotkeysEnum>((string)ddlKeys1.SelectedValue, out h))
            {
                if (this.HotKey != null)
                {
                    this.HotKey.Key1 = h;
                }
            }
            this.ControlItem_Changed(sender, e);
        }

        private void ddlKeys2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoaded == false)
            {
                return;
            }
            HotkeysEnum h;
            if (Enum.TryParse<HotkeysEnum>((string)ddlKeys2.SelectedValue, out h))
            {
                if (this.HotKey != null)
                {
                    this.HotKey.Key2 = h;
                }
            }
            this.ControlItem_Changed(sender, e);
        }

       

        private void ControlItem_Changed(object sender, EventArgs e)
        {
            if (ChangeEventEnabled == false)
                return;
            if (ItemChanged != null)
                ItemChanged(this, e);
        }

      

        private void chkAlt_Validated(object sender, EventArgs e)
        {

        }

        private void BindControls()
        {
            
            if (this.HotKey == null)
            {
                this.HotKey = new HotkeyKeys();
            }
            chkAlt.Checked = this.HotKey.Alt;
            chkCtrl.Checked = this.HotKey.Ctrl;
            chkDisableNative.Checked = this.HotKey.NativeBlock;
            chkInstallHook.Checked = this.HotKey.InstallHook;
            chkLeftMod.Checked = this.HotKey.Left;
            chkRightMod.Checked = this.HotKey.Right;
            chkShift.Checked = this.HotKey.Shift;
            chkUp.Checked = this.HotKey.UP;
            chkWildcard.Checked = this.HotKey.WildCard;
            chkWin.Checked = this.HotKey.Win;
            ddlKeys1.SelectedItem = this.HotKey.Key1.ToString();
            ddlKeys2.SelectedItem = this.HotKey.Key2.ToString();
        }

        private void chkCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.Ctrl = chkCtrl.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkAlt_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.Alt = chkAlt.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkShift_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.Shift = chkShift.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkWin_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.Win = this.chkWin.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkLeftMod_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.Left = chkLeftMod.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkRightMod_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.Right = this.chkRightMod.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkWildcard_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.WildCard = chkWildcard.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkDisableNative_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.NativeBlock = chkDisableNative.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkInstallHook_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.InstallHook = chkInstallHook.Checked;
            this.ControlItem_Changed(sender, e);
        }

        private void chkUp_CheckedChanged(object sender, EventArgs e)
        {
            if (this.HotKey == null)
                return;
            this.HotKey.UP = chkUp.Checked;
            this.ControlItem_Changed(sender, e);
        }
    }
}
