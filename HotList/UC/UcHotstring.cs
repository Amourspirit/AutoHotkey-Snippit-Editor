using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;
using BigByteTechnologies.Library.Windows.CommandManagement;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    /// <summary>
    /// User Control for editing Hotstrings
    /// </summary>
    /// <remarks>
    /// If the <see cref="UcHotstring.HotStringValue.Options.ExcludeRules"/> has values then the 
    /// corresponding control is disabled.
    /// </remarks>
    public partial class UcHotstring : UserControl
    {
        #region Constructor
        public UcHotstring()
        {
            
            InitializeComponent();
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode == true)
            {
                return;
            }
            InitializeCommandManager();
            // for unknown reasons trying to hook handleCreated from user control is causing
            // Visual Studio 2015 to crash. It does not seem to matter if it is a child control
            // or the user control itself.
            // The solution is to use the designMode as show above to get around design time issue.
            // this.DesignMode does not work in a constructor

            txtHotstring.HandleCreated += (sender, ev) => { this.HotStringControlsCreated = true; };
            // txtHotstring.HandleCreated += HotStringControl_HandleCreated;
            //this.HandleCreated += HotStringControl_HandleCreated;
        }
        #endregion

        //private void HotStringControl_HandleCreated(Object sender, EventArgs e)
        //{
        //    HotStringControlsCreated = true;
        //}

        #region Fields / Members
        private bool IsLoaded = false;
        protected internal EventHandler ItemChanged;
        private bool ChangeEventEnabled = false;
        private BindingList<AhkKeyMapAhkMapValue> SendTypes;
        /// <summary>
        /// Gets if the Hotstring Controls have been created. Does not include Replacement Controls
        /// </summary>
        private bool HotStringControlsCreated = false;
        #endregion

        #region Properties
        /// <summary>
        /// Specifies if the Hotstring is new or existing
        /// </summary>
        public bool IsNew { get; set; }

        hotstring m_HotStringValue;
        /// <summary>
        /// Specifies the Hotstring used for the control instance
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public hotstring HotStringValue
        {
            get
            {
                //if (this.m_HotStringValue == null)
                //{
                //    this.m_HotStringValue = new hotstring();
                //    this.IsNew = true;
                //}
                return m_HotStringValue;
            }
            set
            {
                this.m_HotStringValue = value;
                if (this.m_HotStringValue == null)
                {
                    //this.m_HotStringValue = new hotstring();
                    this.IsNew = true;
                }
               
                this.ChangeEventEnabled = false;

                this.BindControls();
                this.ChangeEventEnabled = true;
            }
        }
        #endregion

        #region Commands
        #region Command Property
        bool? m_IsValid;
        /// <summary>
        /// Gets if the state of the controls pass validation
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (m_IsValid.HasValue == false)
                {
                   
                    m_IsValid = IsValidHsControls();
                }
                return m_IsValid.Value;
            }
        }
        #endregion
        #region Events
        private void CommandPrePorcess(object sender, CancelEventArgs e)
        {
            m_IsValid = null;
        }

       
        #endregion
        #region Command Management Init
        private void InitializeCommandManager()
        {
            cmdMgr.PreCommandProcess += CommandPrePorcess;
            cmdMgr.Commands.Add(new Command("ChkBoxesState", CmdNullExecuteHandler, new Command.UpdateHandler(CmdChkBoxesState)));
            cmdMgr.Commands.Add(new Command("SendTypeState", CmdNullExecuteHandler, new Command.UpdateHandler(CmdSendTypeState)));

        }
        #endregion

        #region Command Update Handlers
        private void CmdSendTypeState(Command cmd)
        {
            if (this.HotStringControlsCreated == false)
            {
                return;
            }
            if (this.m_HotStringValue == null)
            {
                cmd.Enabled = false;
                return;
            }
            bool b = false;
            switch (this.HotStringValue.type)
            {
               
                case hotstringType.Inline:
                    b = true;
                    break;
                default:
                    b = false;
                    break;
            }
            ddlSend.Enabled = b;
            lblSend.Enabled = b;
        }
        private void CmdChkBoxesState(Command cmd)
        {
            if (this.HotStringControlsCreated == false)
            {
                return;
            }
            if (this.m_HotStringValue == null)
            {
                cmd.Enabled = false;
                return;
            }
            if (this.Enabled == false)
            {
                return;
            }
            if(this.HotStringValue.Options.ExcludeRules.Count == 0)
            {
                return;
            }
            var rules = this.HotStringValue.Options.ExcludeRules;
            chkAutoBsOff.Enabled = !rules[HotStringOptionsEnum.AutomaticBackSpaceOff];
            lblAutoBsOff.Enabled = chkAutoBsOff.Enabled;

            chkCaseSensitive.Enabled = !rules[HotStringOptionsEnum.CaseSensitive];
            lblCaseSensitive.Enabled = chkCaseSensitive.Enabled;

            chkOmitEndChar.Enabled = !rules[HotStringOptionsEnum.OmitEndChar];
            lblOmitEndChar.Enabled = chkOmitEndChar.Enabled;

            chkResetReconizer.Enabled = !rules[HotStringOptionsEnum.ResetRecognizer];
            lblResetReconizer.Enabled = chkResetReconizer.Enabled;

            chkSendRaw.Enabled = !rules[HotStringOptionsEnum.SendRaw];
            lblSendRaw.Enabled = chkSendRaw.Enabled;

            chkTriggerInside.Enabled = !rules[HotStringOptionsEnum.TriggerInside];
            lblTriggerInside.Enabled = chkTriggerInside.Enabled;

        }
        #endregion

        #region Command Execute Handlers
        private void CmdNullExecuteHandler(Command cmd)
        {
            // do nothing
        }
        #endregion
        #endregion

        #region Load
        private void UcHotstring_Load(object sender, EventArgs e)
        {
            if (this.DesignMode == true)
            {
                return;
            }
            IsLoaded = true;
            this.ChangeEventEnabled = true;
            
        }
        #endregion

        #region Bind
        private void BindControls()
        {
            txtHotstring.DataBindings.Clear();
            if (this.m_HotStringValue == null)
            {
                this.chkAutoBsOff.Checked = false;
                this.chkCaseSensitive.Checked = false;
                this.chkOmitEndChar.Checked = false;
                this.chkResetReconizer.Checked = false;
                this.chkSendRaw.Checked = false;
                this.chkTriggerInside.Checked = false;
                txtHotstring.Text = string.Empty;
                return;
            }
            this.chkAutoBsOff.Checked = this.HotStringValue.Options[HotStringOptionsEnum.AutomaticBackSpaceOff];
            this.chkCaseSensitive.Checked = this.HotStringValue.Options[HotStringOptionsEnum.CaseSensitive];
            this.chkOmitEndChar.Checked = this.HotStringValue.Options[HotStringOptionsEnum.OmitEndChar];
            this.chkResetReconizer.Checked = this.HotStringValue.Options[HotStringOptionsEnum.ResetRecognizer];
            this.chkSendRaw.Checked = this.HotStringValue.Options[HotStringOptionsEnum.SendRaw];
            this.chkTriggerInside.Checked = this.HotStringValue.Options[HotStringOptionsEnum.TriggerInside];

            
            Binding b = new Binding("Text", HotStringValue, "trigger");
            this.txtHotstring.DataBindings.Add(b);
            //this.txtHotstring.Text = this.HotStringValue.trigger;

            BindSendType();



        }
        /// <summary>
        /// Binds the Send type Drop down list Filtering by Hotstring Type
        /// </summary>
        public void BindSendType()
        {
            if (m_HotStringValue == null)
            {
                return;
            }
            SelectedOptions<HotStringSendEnum> SendOptions = new SelectedOptions<HotStringSendEnum>();
            if (this.HotStringValue.type == hotstringType.Inline)
            {

                EnumRule<HotStringSendEnum> SendExclude = new EnumRule<HotStringSendEnum>();
                SendExclude.Add(HotStringSendEnum.None);

                SendTypes = HotstringSendMethodMap.Instance.ToBindingList(SendOptions, SendExclude);
            }
            else
            {
                SendTypes = HotstringSendMethodMap.Instance.ToBindingList(SendOptions);
            }
            ddlSend.DataSource = null;
            ddlSend.Items.Clear();

            ddlSend.DisplayMember = "DisplayValue";
            ddlSend.ValueMember = "Key";
            ddlSend.DataSource = SendTypes;


            if (ddlSend.Items.Count > 0)
            {
                ddlSend.SelectedIndex = 0;
                for (int i = 0; i < ddlSend.Items.Count; i++)
                {
                    AhkKeyMapAhkMapValue mv = (AhkKeyMapAhkMapValue)ddlSend.Items[i];
                    if (mv.Selected == true)
                    {
                        ddlSend.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Changed Events

        private void ControlItem_Changed(object sender, EventArgs e)
        {
            if (ChangeEventEnabled == false)
                return;
            if (ItemChanged != null)
                ItemChanged(this, e);
        }

        private void chkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;
            if (chkCaseSensitive.Checked == true)
            {
                this.HotStringValue.Options.Add(HotStringOptionsEnum.CaseSensitive);
            }
            else
            {
                this.HotStringValue.Options.Remove(HotStringOptionsEnum.CaseSensitive);
            }
            this.ControlItem_Changed(sender, e);
        }

        private void chkTriggerInside_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;
            if (chkTriggerInside.Checked == true)
            {
                this.HotStringValue.Options.Add(HotStringOptionsEnum.TriggerInside);
            }
            else
            {
                this.HotStringValue.Options.Remove(HotStringOptionsEnum.TriggerInside);
            }
            this.ControlItem_Changed(sender, e);
        }

        private void chkOmitEndChar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;
            if (chkOmitEndChar.Checked == true)
            {
                this.HotStringValue.Options.Add(HotStringOptionsEnum.OmitEndChar);
            }
            else
            {
                this.HotStringValue.Options.Remove(HotStringOptionsEnum.OmitEndChar);
            }
            this.ControlItem_Changed(sender, e);
        }

        private void chkAutoBsOff_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;
            if (chkAutoBsOff.Checked == true)
            {
                this.HotStringValue.Options.Add(HotStringOptionsEnum.AutomaticBackSpaceOff);
            }
            else
            {
                this.HotStringValue.Options.Remove(HotStringOptionsEnum.AutomaticBackSpaceOff);
            }
            this.ControlItem_Changed(sender, e);
        }

        private void chkSendRaw_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;

            if (chkSendRaw.Checked == true)
            {
                this.HotStringValue.Options.Add(HotStringOptionsEnum.SendRaw);
            }
            else
            {
                this.HotStringValue.Options.Remove(HotStringOptionsEnum.SendRaw);
            }
            this.ControlItem_Changed(sender, e);
        }

        private void chkResetReconizer_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;

            if (chkResetReconizer.Checked == true)
            {
                this.HotStringValue.Options.Add(HotStringOptionsEnum.ResetRecognizer);
            }
            else
            {
                this.HotStringValue.Options.Remove(HotStringOptionsEnum.ResetRecognizer);
            }
            this.ControlItem_Changed(sender, e);
        }

        private void ddlSend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsLoaded == false)
                return;
            if (this.m_HotStringValue == null)
                return;
            if (ddlSend.SelectedItem != null)
            {
                AhkKeyMapAhkMapValue Sendval = (AhkKeyMapAhkMapValue)ddlSend.SelectedItem;
                HotStringSendEnum currentSendEnum = (HotStringSendEnum)Enum.Parse(typeof(HotStringSendEnum), Sendval.Key);
                this.HotStringValue.Options.SendMethod = currentSendEnum;
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the controls for hotkeys without setting errors
        /// </summary>
        private bool IsValidHsControls()
        {
            bool retval = true;
            if (this.m_HotStringValue == null)
            {
                epHs.SetError(txtHotstring, null);
                return retval;
            }
            CancelEventArgs ce = new CancelEventArgs();
            txtHotstring_Validating(null, ce);
            retval &= !ce.Cancel;
            
            return retval;

        }

        private void txtHotstring_Validating(object sender, CancelEventArgs e)
        {
            
            if (this.HotStringControlsCreated == false)
            {
                e.Cancel = false;
                return;
            }
            if (this.m_HotStringValue == null)
            {
                epHs.SetError(txtHotstring, null);
                e.Cancel = false;
                return;
            }
            if (string.IsNullOrWhiteSpace(txtHotstring.Text))
            {
                e.Cancel = true;
                if (sender != null)
                    epHs.SetError(txtHotstring, string.Format(Properties.Resources.ValidationRequired, lblHotstring.Text));
                return;
            }

            //if (txtHotstring.Text.Length < 2)
            //{
            //    e.Cancel = true;
            //    if (sender != null)
            //        epHs.SetError(txtHotstring, string.Format(Properties.Resources.ValidationMinLength, lblHotstring.Text, 2));
            //    return;
            //}

            
            if (this.HotStringValue.Parent != null)
            {
                if (this.HotStringValue.Parent.Any(
                     x => string.Equals(x.name, txtHotstring.Text, StringComparison.CurrentCultureIgnoreCase) && !ReferenceEquals(x, this.HotStringValue)))
                {
                    e.Cancel = true;
                    if (sender != null)
                        epHs.SetError(txtHotstring, string.Format(Properties.Resources.ValidationDublicateNotAllowed, lblHotstring.Text));
                    return;
                }
            }

            if (e.Cancel == false)
            {
                if (sender != null)
                    epHs.SetError(txtHotstring, null);
                return;
            }
        }

        private void txtHotstring_Validated(object sender, EventArgs e)
        {
            if (this.IsLoaded == false)
                return;
           // this.HotStringValue.trigger = txtHotstring.Text;

            this.ControlItem_Changed(sender, e);
        }

        #endregion


    }
}
