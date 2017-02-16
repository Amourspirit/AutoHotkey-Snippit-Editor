using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigByteTechnologies.Library.Windows.CommandManagement;
using BigByteTechnologies.Windows.AHKSnipit.HotList.Validation;
using System.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Validation
{
    public partial class ValidatePluginForm : Form
    {
        #region Properties
        private XmlKindEnum m_ValidationType;
        public XmlKindEnum ValidationType
        {
            get
            {
                return this.m_ValidationType;
            }
            set
            {
                this.m_ValidationType = value;
                this.SetDisplayText();
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a new instance of the form
        /// </summary>
        public ValidatePluginForm()
        {
            InitializeComponent();
            InitializeCommandManager();
            this.hlpProvider.HelpNamespace = Properties.Resources.FileHelpName;
            this.ValidationType = XmlKindEnum.Plugin;
        }
        /// <summary>
        /// Constructs a new instance of the form
        /// </summary>
        public ValidatePluginForm(XmlKindEnum ValidationType) : this()
        {
            this.ValidationType = ValidationType;
            if (this.ValidationType == XmlKindEnum.SnippitInstl)
            {
                this.hlpProvider.SetHelpKeyword(this, @"23");
            }
            else
            {
                this.hlpProvider.SetHelpKeyword(this, @"24");
            }
        }
        #endregion

        #region Commands

        #region Command Management
        /// <summary>
        /// Initializes the Command Manager
        /// </summary>
        private void InitializeCommandManager()
        {
            cmdMgr.Commands.Add(new Command("CmdCopy", new Command.ExecuteHandler(CmdCopy), new Command.UpdateHandler(CmdCopyHandler)));
            cmdMgr.Commands["CmdCopy"].CommandInstances.Add(mnuCopy);

            cmdMgr.Commands.Add(new Command("CmdSelectAll", new Command.ExecuteHandler(CmdSelectAll), new Command.UpdateHandler(CmdSelectAllHandler)));
            cmdMgr.Commands["CmdSelectAll"].CommandInstances.Add(mnuSelectAll);

            cmdMgr.Commands.Add(new Command("CmdFile", new Command.ExecuteHandler(CmdFile), new Command.UpdateHandler(CmdFileHandler)));
            cmdMgr.Commands["CmdFile"].CommandInstances.AddRange(new object[] { mnuOpen, btnFile });

            cmdMgr.Commands.Add(new Command("CmdExit", new Command.ExecuteHandler(CmdExit), new Command.UpdateHandler(CmdExitHandler)));
            cmdMgr.Commands["CmdExit"].CommandInstances.Add(mnuExit);

            cmdMgr.Commands.Add(new Command("CmdValidate", new Command.ExecuteHandler(CmdValidate), new Command.UpdateHandler(CmdValidateHandler)));
            cmdMgr.Commands["CmdValidate"].CommandInstances.Add(mnuValidate);
        }

        #endregion

        #region Command Handlers
        /// <summary>
        /// Command handler to enable or disable commands for Validate plugin
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdValidate(Command)"/>
        private void CmdValidateHandler(Command cmd)
        {
            cmd.Enabled = txtFile.TextLength > 0;
            if (cmd.Enabled)
            {
                cmd.Enabled &= File.Exists(txtFile.Text);
            }
        }

        /// <summary>
        /// Command handler to enable or disable commands for the seleect results text
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdSelectAll(Command)"/>
        private void CmdSelectAllHandler(Command cmd)
        {
            cmd.Enabled = txtResult.TextLength > 0;
            cmd.Enabled &= txtResult.SelectionLength < txtResult.TextLength;
        }

        /// <summary>
        /// Command handler to enable or disable commands for the copy results
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdCopy(Command)"/>
        private void CmdCopyHandler(Command cmd)
        {
            cmd.Enabled = txtResult.SelectedText.Length > 0;
        }
        /// <summary>
        /// Command handler to enable or disable commands for File Open
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdFile(Command)"/>
        private void CmdFileHandler(Command cmd)
        {
            cmd.Enabled = true;
        }
        /// <summary>
        /// Command handler to enable or disable commands for the select all text
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdExit(Command)"/>
        private void CmdExitHandler(Command cmd)
        {
            cmd.Enabled = true;
        }
        #endregion

        #region Commnad Methods
        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdValidateHandler(Command)"/>
        private void CmdValidate(Command cmd)
        {
            this.ValidateFile();         
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdSelectAllHandler(Command)"/>
        private void CmdSelectAll(Command cmd)
        {
            txtResult.SelectAll();
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdCopyHandler(Command)"/>
        private void CmdCopy(Command cmd)
        {
            Clipboard.SetText(txtResult.SelectedText);
        }

        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdFileHandler(Command)"/>
        private void CmdFile(Command cmd)
        {
            
            string LastFolder = string.Empty;
            switch (this.ValidationType)
            {
                 case XmlKindEnum.SnippitInstl:
                    LastFolder = Properties.Settings.Default.ValLastSiLocation;
                    break;
                case XmlKindEnum.Profile:
                    LastFolder = Properties.Settings.Default.ValLastProLocation;
                    break;
                case XmlKindEnum.Plugin:
                    LastFolder = Properties.Settings.Default.ValLastPlgLocation;
                    break;
                default:
                    LastFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    break;
            }
            if (!Directory.Exists(LastFolder))
            {
                LastFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            ofdPlugin.RestoreDirectory = true;
            ofdPlugin.InitialDirectory = LastFolder;
           
            DialogResult result = ofdPlugin.ShowDialog();
            if (result == DialogResult.OK)
            {
                switch (this.ValidationType)
                {
                    case XmlKindEnum.SnippitInstl:
                        Properties.Settings.Default.ValLastSiLocation = Path.GetDirectoryName(ofdPlugin.FileName);
                        break;
                    case XmlKindEnum.Profile:
                        Properties.Settings.Default.ValLastProLocation = Path.GetDirectoryName(ofdPlugin.FileName);
                        break;
                    case XmlKindEnum.Plugin:
                        Properties.Settings.Default.ValLastPlgLocation = Path.GetDirectoryName(ofdPlugin.FileName);
                        break;
                    default:
                        break;
                }
                txtFile.Text = ofdPlugin.FileName;
                this.ValidateFile();
            }
        }
        /// <summary>
        /// Performs the command that takes action with when command is activated
        /// </summary>
        /// <param name="cmd">Instance of <see cref="Command"/> to manage the control state</param>
        /// <seealso cref="CmdExitHandler(Command)"/>
        private void CmdExit(Command cmd)
        {
            this.Close();
        }
        #endregion
        #endregion

        #region Validation

        private void ValidateFile()
        {
            switch (this.ValidationType)
            {
                case XmlKindEnum.SnippitInstl:
                    this.ValidateInstal();
                    break;
                case XmlKindEnum.Profile:
                    this.ValidatePropfile();
                    break;
                case XmlKindEnum.Plugin:
                    this.ValidatePlugin();
                    break;
                default:
                    break;
            }
        }

        private void ValidateInstal()
        {
            var result = ValidateXml.ValidateSnipitInstalFile(txtFile.Text);
            txtResult.Clear();
            if (result.HasErrors)
            {
                foreach (var strError in result.Errors)
                {
                    txtResult.AppendText(strError + Environment.NewLine);
                }
            }
            else
            {
                txtResult.Text = Properties.Resources.ValidateInstallSuccess;
            }
        }

        private void ValidatePlugin()
        {
            var result = ValidateXml.ValidatePluginFile(txtFile.Text);
            txtResult.Clear();
            if (result.HasErrors)
            {
                foreach (var strError in result.Errors)
                {
                    txtResult.AppendText(strError + Environment.NewLine);
                }
            }
            else
            {
                txtResult.Text = Properties.Resources.ValidatePluginSuccess;
            }
        }

        private void ValidatePropfile()
        {
            var result = ValidateXml.ValidateProfileFile(txtFile.Text);
            txtResult.Clear();
            if (result.HasErrors)
            {
                foreach (var strError in result.Errors)
                {
                    txtResult.AppendText(strError + Environment.NewLine);
                }
            }
            else
            {
                txtResult.Text = Properties.Resources.ValidatePluginSuccess;
            }
        }
        #endregion

        #region Display Methods
        private void SetDisplayText()
        {
            switch (this.ValidationType)
            {
                case XmlKindEnum.Plugin:
                    this.Text = Properties.Resources.ValidationDialogPluginTitle;
                    this.lblFile.Text = Properties.Resources.ValidatoinDialogPluginOpenText;
                    break;
                case XmlKindEnum.SnippitInstl:
                    this.Text = Properties.Resources.ValidationDialogInstallTitle;
                    this.lblFile.Text = Properties.Resources.ValidatoinDialogInstalOpenText;
                    break;
                case XmlKindEnum.Profile:
                    this.Text = Properties.Resources.ValidationDialogProfileTitle;
                    this.lblFile.Text = Properties.Resources.ValidationProfileOpenText;
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void ValidatePluginForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ValWindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.ValLocation;
                Size = Properties.Settings.Default.ValSize;
            }
            else if (Properties.Settings.Default.ValWindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                Location = Properties.Settings.Default.ValLocation;
                Size = Properties.Settings.Default.ValSize;
            }
            else
            {
                Location = Properties.Settings.Default.ValLocation;
                Size = Properties.Settings.Default.ValSize;
            }
        }

        private void ValidatePluginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.ValLocation = RestoreBounds.Location;
                Properties.Settings.Default.ValWindowState = WindowState;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.ValLocation = Location;
                Properties.Settings.Default.ValSize = Size;
                Properties.Settings.Default.SiWindowState = WindowState;
            }
            else
            {
                Properties.Settings.Default.ValLocation = RestoreBounds.Location;
                Properties.Settings.Default.ValSize = RestoreBounds.Size;
                Properties.Settings.Default.ValWindowState = WindowState;
            }
        }
    }
}
