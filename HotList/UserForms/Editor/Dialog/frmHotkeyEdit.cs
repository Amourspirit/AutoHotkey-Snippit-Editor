using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Library.Windows.CommandManagement;
using System.Drawing;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    public partial class frmHotkeyEdit : Form
    {
        public frmHotkeyEdit()
        {
            InitializeComponent();
            InitializeCommandManager();
            this.hlpProvider.HelpNamespace = Properties.Resources.FileHelpName;
        }

        public HotkeyKeys Hotkey { get; set; }


        #region Commands
        private void InitializeCommandManager()
        {
            cmdMgr.Commands.Add(new Command("CmdOk", new Command.ExecuteHandler(CmdOk), new Command.UpdateHandler(CmdOkHandler)));
            cmdMgr.Commands["CmdOk"].CommandInstances.Add(btnOk);

            cmdMgr.Commands.Add(new Command("CmdCancel", new Command.ExecuteHandler(CmdCancel), new Command.UpdateHandler(CmdCancelHandler)));
            cmdMgr.Commands["CmdCancel"].CommandInstances.Add(btnCancel);

        }

        private void CmdOkHandler(Command cmd)
        {
            bool b = true;
            b &= this.Hotkey != null;
            if (b == false)
            {
                cmd.Enabled = b;
                return;
            }

            //HotkeyKeys hks;
            //if (!HotkeyKeys.TryParse(this.UcHotkeyMain.HotKey.ToString(), out hks))
            //{
            //    cmd.Enabled = false;
            //    return;
            //}
            b &= this.UcHotkeyMain.HotKey.IsValid;
            
            cmd.Enabled = b;
        }
        private void CmdCancelHandler(Command cmd)
        {
            cmd.Enabled = true;
        }
        private void CmdOk(Command cmd)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void CmdCancel(Command cmd)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        private void frmHotkeyEdit_Load(object sender, System.EventArgs e)
        {
            btnOk.Image = Z.IconLibrary.Silk.Icon.Accept.GetImage();
            btnOk.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancel.Image = Z.IconLibrary.Silk.Icon.Cancel.GetImage();
            btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
            if (this.Hotkey == null)
            {
                this.Hotkey = new HotkeyKeys();
            }
            UcHotkeyMain.HotKey = this.Hotkey;
        }
    }
}
