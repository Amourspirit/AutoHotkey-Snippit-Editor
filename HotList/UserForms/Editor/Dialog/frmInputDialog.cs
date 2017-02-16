using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin.Dialog;
using BigByteTechnologies.Library.Windows.CommandManagement;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    public partial class frmInputDialog : Form
    {
        #region Constructor
        public frmInputDialog()
        {
            InitializeComponent();
            InitializeCommandManager();
            this.hlpProvider.HelpNamespace = Properties.Resources.FileHelpName;
            
        }
        #endregion

   
        #region Properties
        public inputReplacement DialogData { get; set; }

        #endregion

        #region Commands

        #region Command Management Init
        private void InitializeCommandManager()
        {
            cmdMgr.Commands.Add(new Command("CmdClose", new Command.ExecuteHandler(CmdClose), new Command.UpdateHandler(CmdCloseHandler)));
            cmdMgr.Commands["CmdClose"].CommandInstances.AddRange(new object[] { mnuClose, tsBtnClose });

            cmdMgr.Commands.Add(new Command("CmdSave", new Command.ExecuteHandler(CmdSave), new Command.UpdateHandler(CmdSaveHandler)));
            cmdMgr.Commands["CmdSave"].CommandInstances.Add<ToolStripMenuItem>(mnuSave);
            cmdMgr.Commands["CmdSave"].CommandInstances.Add<ToolStripButton>(tsBtnSave);


        }
        #endregion

        #region Command Handlers

        private void CmdSaveHandler(Command Cmd)
        {
            bool b = true;
            b &= this.UcPlaceholder.IsValid;
            b &= this.UcDialogTitle.IsValid;
            b &= this.UcDialogText.IsValid;
            Cmd.Enabled = b;
        }

        private void CmdCloseHandler(Command Cmd)
        {
            Cmd.Enabled = true;
        }
        #endregion

        #region Commnad Methods
        private void CmdSave(Command Cmd)
        {
            if (this.ValidateChildren() == true)
            {
                this.DialogData.dialoginitialvalue = UcInitalValue.FieldText;
                this.DialogData.dialogtext = UcDialogText.FieldText;
                this.DialogData.dialogtitle = UcDialogTitle.FieldText;
                this.DialogData.placeholder = UcPlaceholder.FieldText;
                this.DialogResult = DialogResult.OK;
            }
        }
        private void CmdClose(Command Cmd)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

        #region Load Event
        private void frmInputFixedList_Load(object sender, EventArgs e)
        {
            if (this.DialogData == null)
            {
                this.DialogData = new inputReplacement();
            }
                                           
            this.UcPlaceholder.FieldText = DialogData.placeholder;
            this.UcDialogTitle.FieldText = DialogData.dialogtitle;
            this.UcInitalValue.FieldText = DialogData.dialoginitialvalue;
            this.UcDialogText.FieldText = DialogData.dialogtext;
        }



        #endregion

        private void ucField1_Load(object sender, EventArgs e)
        {

        }

        private void frmInputDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
