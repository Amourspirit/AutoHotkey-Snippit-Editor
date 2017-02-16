using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    public partial class UcFileName : UserControl
    {
        public UcFileName()
        {
            InitializeComponent();
            this.Required = true;
            this.ToolTip = "Name";
            this.Label = "Name";
        }

        #region Properties
        private bool m_Isvalid = false;
        /// <summary>
        /// Gets if the current state is valid
        /// </summary>
        [Browsable(false)]
        public bool IsValid
        {
            get { return this.m_Isvalid; }
        }

        /// <summary>
        /// Gets or sets if FileName is required
        /// </summary>
        [Category("Custom"), DefaultValue(true), Browsable(true)]
        public bool Required { get; set; }

        /// <summary>
        /// Gets/Sets the Text of the Label
        /// </summary>
        [Category("Custom"), DefaultValue("Name"), Browsable(true)]
        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        /// <summary>
        /// Gets/Sets the Tooltip for the Textbox
        /// </summary>
        [Category("Custom"), DefaultValue("Name"), Browsable(true)]
        public string ToolTip
        {
            get
            {
                return this.TtMain.GetToolTip(TbField);
            }
            set
            {
                this.TtMain.SetToolTip(TbField, value);
            }
        }

        /// <summary>
        /// Gets/Sets the Text for the FileName Textbox
        /// </summary>
        [Category("Custom"), Browsable(true)]
        public string FileName
        {
            get { return TbField.Text; }
            set {
                if (TbField.Text != value)
                {
                    TbField.Text = value;
                    ValidateField();
                }
            }
        }

        /// <summary>
        /// Gets/Sets if the textbox is Enabled
        /// </summary>
        [Category("Custom"), DefaultValue(true), Browsable(true)]
        public bool FieldEnabled
        {
            get
            {
                return TbField.Enabled;
            }
            set
            {
                TbField.Enabled = value;
            }
        }
        #endregion

        #region Events
        public EventHandler<KeyEventArgs> KeyUpHandler;
        public EventHandler<KeyEventArgs> KeyDonwHandler;
        public EventHandler<KeyPressEventArgs> KeyPressHandler;
        public EventHandler TextChangedHandler;
        #endregion

        #region Event Handler
        private void txtFileName_Validating(object sender, CancelEventArgs e)
        {
            this.m_Isvalid = false;
            if (this.Required == false)
            {
                if (string.IsNullOrEmpty(TbField.Text))
                {
                    e.Cancel = false;
                    if (sender != null)
                        EpMain.SetError(TbField, null);
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(TbField.Text))
            {
                e.Cancel = true;
                //txtFileName.Focus();
                if (sender != null)
                    EpMain.SetError(TbField, string.Format(Properties.Resources.ValidationRequired, lblName.Text));
                return;
            }
            if (TbField.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                e.Cancel = true;
                //txtFileName.Focus();
                if (sender != null)
                    EpMain.SetError(TbField, string.Format(Properties.Resources.ValidationFormat, lblName.Text));
                return;
            }
            if (e.Cancel == false)
            {
                if (sender != null)
                    EpMain.SetError(TbField, null);
            }
        }

        private void txtFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.KeyDonwHandler != null)
                KeyDonwHandler(sender, e);
        }

        private void txtFileName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.KeyPressHandler != null)
                KeyPressHandler(sender, e);
        }

        private void txtFileName_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.KeyUpHandler != null)
                KeyUpHandler(sender, e);
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (this.TextChangedHandler != null)
                TextChangedHandler(sender, e);
        }

        private void txtFileName_Validated(object sender, EventArgs e)
        {
            this.m_Isvalid = true;
        }

        private void UcFileName_Load(object sender, EventArgs e)
        {
            ValidateField();
        }

        private void ValidateField()
        {
            // Set up the IsValid property for startup
            // Just in case the textbox has been assigned a startup value on a form
            // this will set IsValid to true if the textbox has been assigned valid data
            CancelEventArgs ce = new CancelEventArgs(false);
            txtFileName_Validating(null, ce);
            this.m_Isvalid = !ce.Cancel;
        }
        #endregion


    }
}
