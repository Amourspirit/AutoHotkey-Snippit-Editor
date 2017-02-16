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
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UC
{
    public partial class UcVersion : UserControl
    {
        #region Constructor
        public UcVersion()
        {
            InitializeComponent();
            this.TbField.Text = "1.0.0.0";
            this.Required = true;
            this.ToolTip = "Version";
            this.Label = "Version";
        }
        #endregion

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

        [Category("Custom"), DefaultValue("1.0.0.0"), Browsable(true)]
        public string Version
        {
            get { return TbField.Text; }
            set
            {
                if (TbField.Text != value)
                {
                    TbField.Text = value;
                    ValidateField();
                }
            }
        }

        [Category("Custom"), DefaultValue("Version"), Browsable(true)]
        public string Label
        {
            get { return lblVersion.Text; }
            set { lblVersion.Text = value; }
        }

        /// <summary>
        /// Gets or sets if Version is required
        /// </summary>
        [Category("Custom"), DefaultValue(true), Browsable(true)]
        public bool Required { get; set; }
        
        [Category("Custom"), DefaultValue("Version"), Browsable(true)]
        public string ToolTip
        {
            get
            {
                return this.ttVersion.GetToolTip(TbField);
            }
            set
            {
                this.ttVersion.SetToolTip(TbField, value);
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

        #region Event Handlers

        private void txtVersion_Validating(object sender, CancelEventArgs e)
        {
            this.m_Isvalid = false;
            if (this.Required == false)
            {
                if (string.IsNullOrEmpty(TbField.Text))
                {
                    e.Cancel = false;
                    if(sender != null)
                        errorProvider1.SetError(TbField, null);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(TbField.Text))
                {
                    e.Cancel = true;
                    if (sender != null)
                        errorProvider1.SetError(TbField, string.Format(Properties.Resources.ValidationRequired, lblVersion.Text));
                    return;
                }
            }
            bool IsMatch = RegularExpressions.VersionRegex.IsMatch(TbField.Text);
            if (IsMatch == true)
            {
                e.Cancel = false;
                if (sender != null)
                    errorProvider1.SetError(TbField, null);
                return;
            }
            else
            {
                e.Cancel = true;
                if (sender != null)
                    errorProvider1.SetError(TbField, string.Format(Properties.Resources.ValidationFormat, lblVersion.Text));
                return;
            }
           
        }

        private void txtVersion_Validated(object sender, EventArgs e)
        {
            this.m_Isvalid = true;
        }

        private void UcVersion_Load(object sender, EventArgs e)
        {
            ValidateField();
        }

        private void ValidateField()
        {
            // Set up the IsValid property for startup
            // Just in case the textbox has been assigned a startup value on a form
            // this will set IsValid to true if the textbox has been assigned valid data
            CancelEventArgs ce = new CancelEventArgs(false);
            txtVersion_Validating(null, ce);
            this.m_Isvalid = !ce.Cancel;
        }
        #endregion

       
       
    }
}
