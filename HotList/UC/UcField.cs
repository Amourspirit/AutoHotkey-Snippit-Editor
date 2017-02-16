using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    public partial class UcField : UserControl
    {
        #region Consrtuctor
        public UcField()
        {
            InitializeComponent();
            this.Label = @"Name";
            this.RegularExpression = string.Empty;
            this.MaxLength = 32767;
            this.ToolTip = @"Name";
            this.Required = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the edges of the container to which a control is bound and determines how a control is resized with its parent. 
        /// </summary>
        //[Category("Custom"), DefaultValue(AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right), Browsable(true)]
        //[EditorAttribute(typeof(System.Windows.Forms.Design.AnchorEditor), typeof(System.Drawing.Design.UITypeEditor))]
        //public AnchorStyles AnchorField
        //{
        //    get { return txtName.Anchor; }
        //    set { txtName.Anchor = value; }
        //}
        /// <summary>
        /// Controls if the text of the edit control can span more than one line
        /// </summary>
        [Category("Custom"), DefaultValue(false), Browsable(true)]
        public bool MultiLine
        {
            get { return TbField.Multiline; }
            set { TbField.Multiline = value; }
        }


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
        /// Gets/Sets the Text of the Field textbox
        /// </summary>
        [Category("Custom"), Browsable(true)]
        public string FieldText
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
        /// Gets/Sets the Text of the Label
        /// </summary>
        [Category("Custom"), DefaultValue("Name"), Browsable(true)]
        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        /// <summary>
        /// Gets/Sets the regular expression used to validate to textbox
        /// </summary>
        [Category("Custom"), Browsable(true)]
        public string RegularExpression { get; set; }

        /// <summary>
        /// Specifies the maximum number of characters that can be entered into the field textbox
        /// </summary>
        [Category("Custom"), DefaultValue(32767), Browsable(true)]
        public int MaxLength
        {
            get { return TbField.MaxLength; }
            set { TbField.MaxLength = value; }
        }

        /// <summary>
        /// Gets/Sets the Tooltip for the Textbox
        /// </summary>
        [Category("Custom"), DefaultValue("Name"), Browsable(true)]
        public string ToolTip
        {
            get
            {
                return this.Ttip.GetToolTip(TbField);
            }
            set
            {
                this.Ttip.SetToolTip(TbField, value);
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

        #region Event Handlers
        private void txtName_Validating(object sender, CancelEventArgs e)
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
            else
            {
                if (string.IsNullOrEmpty(TbField.Text))
                {
                    e.Cancel = true;
                    if (sender != null)
                        EpMain.SetError(TbField, string.Format(Properties.Resources.ValidationRequired, lblName.Text));
                                       
                    return;
                }
            }
            if (string.IsNullOrEmpty(this.RegularExpression) == false)
            {
                Regex rx = new Regex(
                  this.RegularExpression,
                    RegexOptions.Singleline
                    | RegexOptions.CultureInvariant
                    );
                bool IsMatch = rx.IsMatch(TbField.Text);
                if (IsMatch == true)
                {
                    e.Cancel = false;
                    if (sender != null)
                        EpMain.SetError(TbField, null);
                    return;
                }
                else
                {
                    e.Cancel = true;
                    if (sender != null)
                        EpMain.SetError(TbField, string.Format(Properties.Resources.ValidationFormat, lblName.Text));
                    return;
                }
            }
            else
            {
                e.Cancel = false;
                if (sender != null)
                    EpMain.SetError(TbField, null);

                return;
            }
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.KeyUpHandler != null)
                KeyUpHandler(sender, e);
        }
        
        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.KeyDonwHandler != null)
                KeyDonwHandler(sender, e);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.KeyPressHandler != null)
                KeyPressHandler(sender, e);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (this.TextChangedHandler != null)
                TextChangedHandler(sender, e);
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            this.m_Isvalid = true;
        }

        private void UcField_Load(object sender, EventArgs e)
        {
            ValidateField();
        }
        private void ValidateField()
        {
            // Set up the IsValid property for startup
            // Just in case the textbox has been assigned a startup value on a form
            // this will set IsValid to true if the textbox has been assigned valid data
            CancelEventArgs ce = new CancelEventArgs(false);
            txtName_Validating(null, ce);
            this.m_Isvalid = !ce.Cancel;
        }
        #endregion


    }
}
