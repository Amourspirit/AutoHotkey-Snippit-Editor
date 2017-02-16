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
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Extensions;
using System.Diagnostics;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    public partial class frmInputFixedList : Form
    {
        #region Constructor
        public frmInputFixedList()
        {
            InitializeComponent();
            InitializeCommandManager();
            this.hlpProvider.HelpNamespace = Properties.Resources.FileHelpName;

            ItemTypeBindingList = new BindingList<itemType>();
            ItemTypeBindingList.AddingNew += (s, e) =>
            {
                e.NewObject = new itemType { Parent = ItemTypeBindingList };
            };
            DelCol.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
        }
        #endregion
        
        #region Fields
        BindingList<itemType> ItemTypeBindingList;
        #endregion

        #region Properties
        public inputFixedList DialogData { get; set; }


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
            b &= dgList.NewRowIndex > 0;
            b &= this.UcPlaceholder.IsValid;
            b &= this.UcDialogTitle.IsValid;
            b &= this.UcDialogText.IsValid;
            if (b == false)
            {
                Cmd.Enabled = false;
                return;
            }
            if (dgList.CurrentRow != null)
            {
               b &= !this.dgList.IsCurrentRowDirty;
            }
            if (b == false)
            {
                Cmd.Enabled = false;
                return;
            }
            b &= !this.GridHasError();
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
                this.DialogData.placeholder = UcPlaceholder.FieldText;
                this.DialogData.dialogtext = UcDialogText.FieldText;
                this.DialogData.dialogtitle = UcDialogTitle.FieldText;
                List<itemType> items = new List<itemType>();

                foreach (itemType item in this.ItemTypeBindingList)
                {
                    // creating new and populating sets Parent Property to Null
                    itemType newItem = new itemType();
                    item.PoplulateOther(newItem, true);
                    //newItem.Populate(item);
                    //var newItem = item.Poplulate();
                    items.Add(newItem);
                }
                this.DialogData.listValues = items.ToArray();
                this.DialogResult = DialogResult.OK;
            }
        }
        private void CmdClose(Command Cmd)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

        #region Form Events
        private void frmInputFixedList_Load(object sender, EventArgs e)
        {
            if (this.DialogData == null)
            {
                this.DialogData = new inputFixedList();
            }

            ItemTypeBindingList.Clear();

            if (this.DialogData.listValues != null)
            {
                foreach (itemType itmType in this.DialogData.listValues)
                {
                    itemType newIt = ItemTypeBindingList.AddNew();
                    itmType.PoplulateOther(newIt);
                    newIt.Parent = ItemTypeBindingList;
                }

            }
            
            this.listValuesBindingSource.DataSource = ItemTypeBindingList;
            this.UcPlaceholder.FieldText = DialogData.placeholder;
            this.UcDialogTitle.FieldText = DialogData.dialogtitle;
            this.UcDialogText.FieldText = DialogData.dialogtext;
        }

        private void frmInputFixedList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion

        #region Methods
        private bool GridHasError()
        {
            bool hasErrorText = false;
            //replace this.dataGridView1 with the name of your datagridview control
            foreach (DataGridViewRow row in this.dgList.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ErrorText.Length > 0)
                    {
                        hasErrorText = true;
                        break;
                    }
                }
                if (hasErrorText)
                    break;
            }

            return hasErrorText;
        }
        #endregion

        #region dgList Events
        private void dgList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgList.NewRowIndex != e.RowIndex)
            {
                int delColIndex = dgList.ColumnCount - 1;
                if (e.ColumnIndex == delColIndex) // delete
                {
                    var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        this.ItemTypeBindingList.RemoveAt(e.RowIndex);

                    }
                }

            }
        }

        private void dgList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                      , MessageBoxButtons.YesNo
                      , MessageBoxIcon.Question
                      , MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void dgList_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // add delete image to new rows
            int colIndex = dgList.ColumnCount - 1;
            e.Row.Cells[colIndex].Value = Z.IconLibrary.Silk.Icon.Delete.GetImage();
        }
        #endregion
    }
}
