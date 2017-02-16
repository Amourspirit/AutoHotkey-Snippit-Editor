using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using BigByteTechnologies.Library.Windows.CommandManagement;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Tools;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor
{
    public partial class frmProfile : Form
    {
        #region Instance
        private static frmProfile _instance;
        
        public static frmProfile Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new frmProfile();
                }
                return _instance;
            }
        }

        public static bool IsInstance
        {
            get
            {
                return frmProfile._instance != null;
            }
        }
        
        #endregion

        #region Constructor

        public frmProfile()
        {
            InitializeComponent();
            InitializeCommandManager();
            hlpProviderProfile.HelpNamespace = Properties.Resources.FileHelpName;

            ErrorCounterWindows = new ErrorCounter();
            this.pf = new profile();
            pf.codeLanguage = new profileCodeLanguage();
            pf.codeLanguage.paths = new profileCodeLanguagePaths();
            
            windows = new BindingList<profileWindow>();
            this.IsNew = true;
           
            windows.AddingNew += (s, a) =>
            {
                profileWindow pfWind = new profileWindow();
                pfWind.Parent = windows;
                pfWind.Errors = ErrorCounterWindows;
                a.NewObject = pfWind;
            };
           
            this.DgWindowsAddImgColumns();
           
            profileWindow wp = windows.AddNew();
            wp.name = @"Notepad";
            wp.value = @"ahk_class Notepad";
            
            this.dgWindows.UserDeletingRow += dgWindows_UserDeletingRow;
            this.dgWindows.CellClick += dgWindows_CellClick;
            this.dgWindows.DefaultValuesNeeded += dgWindows_DefaultValuesNeeded;
            this.WindowSpyPath = WindowSpy.GetPath();
            if (!string.IsNullOrEmpty(this.WindowSpyPath))
            {
                this.WindowSpyFound = true;
            }
        }
        
        #endregion
              
        #region Fields
        private BindingList<profileWindow> windows;
        private profile pf;
        private BindingList<AhkKeyMapAhkMapValue> EndCharsBindingList;
        private bool WindowSpyFound = false;
        private string WindowSpyPath = string.Empty;
        ErrorCounter ErrorCounterWindows;

        /// <summary>
        /// Will contain which textbox is current Focused. Will be null if no textbox has focus
        /// </summary>
        /// <remarks>
        /// <see cref="tBox_Enter(object, EventArgs)"/> 
        /// <see cref="tBox_Leave(object, EventArgs)"/>
        /// </remarks>
        private TextBox FocusedTextbox = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets/Sets the main window
        /// </summary>
        public SnippitList MainWindow { get; set; }

        /// <summary>
        /// Specifies if this a new profile or an existing profile
        /// </summary>
        public bool IsNew { get; set; }
        #region IsValid Properties
        TriEnum m_IsClipboardText = TriEnum.NotSet;
        /// <summary>
        /// Gets if the Clipboard contains text
        /// </summary>
        /// <remarks>
        /// This property is reset on each CommandPrePorcess event
        /// </remarks>
        private bool IsClipboardText
        {
            get
            {
                if (m_IsClipboardText == TriEnum.NotSet)
                {
                    if (Clipboard.GetDataObject().GetDataPresent(typeof(string)) == true)
                    {
                        m_IsClipboardText = TriEnum.True;
                    }
                    else
                    {
                        m_IsClipboardText = TriEnum.False;
                    }
                }
                return m_IsClipboardText == TriEnum.True;
            }
        }
        #endregion
        #endregion

        #region Commands

        #region Command Events
        private void CommandPrePorcess(object sender, CancelEventArgs e)
        {
            // use pre process event to trigger setting of value only once per idle event
            m_IsClipboardText = TriEnum.NotSet;
           
        }
        #endregion

        #region Command Management Init
        private void InitializeCommandManager()
        {
            cmdMgr.PreCommandProcess += CommandPrePorcess;
            cmdMgr.Commands.Add(new Command("CmdClose", new Command.ExecuteHandler(CmdClose), new Command.UpdateHandler(CmdCloseHandler)));
            cmdMgr.Commands["CmdClose"].CommandInstances.AddRange(new object[] { mnuClose, tsBtnClose });

            cmdMgr.Commands.Add(new Command("CmdSave", new Command.ExecuteHandler(CmdSave), new Command.UpdateHandler(CmdSaveHandler)));
            cmdMgr.Commands["CmdSave"].CommandInstances.Add<ToolStripMenuItem>(mnuSave);
            cmdMgr.Commands["CmdSave"].CommandInstances.Add<ToolStripButton>(tsBtnSave);

            cmdMgr.Commands.Add(new Command("CmdWndowSpy", new Command.ExecuteHandler(CmdWindowSpy), new Command.UpdateHandler(CmdWindowSpyHandler)));
            cmdMgr.Commands["CmdWndowSpy"].CommandInstances.Add<ToolStripMenuItem>(mnuWindowSpy);
            cmdMgr.Commands["CmdWndowSpy"].CommandInstances.Add<ToolStripButton>(tsbWindowSpy);

            cmdMgr.Commands.Add(new Command("CmdWindowSpyFind", new Command.ExecuteHandler(CmdWindowSpyFind), new Command.UpdateHandler(CmdWindowSpyFindHandler)));
            cmdMgr.Commands["CmdWindowSpyFind"].CommandInstances.Add<ToolStripMenuItem>(mnuWindowSpyLocation);

            #region Cut / copy / Paste / Select / Delete

            cmdMgr.Commands.Add(new Command("CmdEditCopy", new Command.ExecuteHandler(CmdEditCopy), new Command.UpdateHandler(CmdEditCopyHandler)));
            cmdMgr.Commands["CmdEditCopy"].CommandInstances.Add(mnuEditCopy);
            cmdMgr.Commands["CmdEditCopy"].CommandInstances.Add(cMnuEditCopy);

            cmdMgr.Commands.Add(new Command("CmdMainCut", new Command.ExecuteHandler(CmdEditCut), new Command.UpdateHandler(CmdEditCutHandler)));
            cmdMgr.Commands["CmdMainCut"].CommandInstances.Add(mnuEditCut);
            cmdMgr.Commands["CmdMainCut"].CommandInstances.Add(cMnuEditCut);

            cmdMgr.Commands.Add(new Command("CmdMainPaste", new Command.ExecuteHandler(CmdEditPaste), new Command.UpdateHandler(CmdEditPasteHandler)));
            cmdMgr.Commands["CmdMainPaste"].CommandInstances.Add(mnuEditPaste);
            cmdMgr.Commands["CmdMainPaste"].CommandInstances.Add(cMnuEditPaste);

            cmdMgr.Commands.Add(new Command("CmdMainSelectAll", new Command.ExecuteHandler(CmdEditSelect), new Command.UpdateHandler(CmdEditSelectHandler)));
            cmdMgr.Commands["CmdMainSelectAll"].CommandInstances.Add(mnuEditSelectAll);
            cmdMgr.Commands["CmdMainSelectAll"].CommandInstances.Add(cMnuEditSelectAll);

            cmdMgr.Commands.Add(new Command("CmdMainDelete", new Command.ExecuteHandler(CmdEditDelete), new Command.UpdateHandler(CmdEditCopyHandler)));
            cmdMgr.Commands["CmdMainDelete"].CommandInstances.Add(mnuEditDelete);
            cmdMgr.Commands["CmdMainDelete"].CommandInstances.Add(cMnuEditDelete);

            cmdMgr.Commands.Add(new Command("CmdMainUndo", new Command.ExecuteHandler(CmdEditUndo), new Command.UpdateHandler(CmdEditUndoHandler)));
            cmdMgr.Commands["CmdMainUndo"].CommandInstances.Add(mnuEditUndo);
            cmdMgr.Commands["CmdMainUndo"].CommandInstances.Add(cMnuEditUndo);
            #endregion

        }
        #endregion

        #region Command Handlers

        #region Cut / Copy / Paste / Select / Handlers
        private void CmdEditUndoHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.Enabled
                && this.FocusedTextbox.CanUndo);
        }
        private void CmdEditCutHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.Enabled
                && this.FocusedTextbox.SelectionLength > 0);
        }

        private void CmdEditCopyHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.SelectionLength > 0);
        }

        private void CmdEditSelectHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null
                && this.FocusedTextbox.CanSelect);
        }

        private void CmdEditPasteHandler(Command Cmd)
        {
            Cmd.Enabled = (this.FocusedTextbox != null && this.IsClipboardText == true);
        }
        #endregion

        private void CmdWindowSpyFindHandler(Command Cmd)
        {
            Cmd.Enabled = true;

        }

        private void CmdWindowSpyHandler(Command Cmd)
        {
            Cmd.Visible = this.WindowSpyFound;
            Cmd.Enabled = true;
        }
        private void CmdSaveHandler(Command Cmd)
        {
            bool b = true;
            // if not a global profile then require at least one window
            if (chkGlobal.Checked == false)
            {
                b &= dgWindows.NewRowIndex > 0;
            }
            
            b &= this.ProfileName.IsValid;
            b &= this.ProfileVersion.IsValid;
            b &= this.ProfileDisplayName.IsValid;

            // calling validate children does not work out as it takes edit grid out of edit mode
            // Cmd.Enabled &= this.ValidateChildren(ValidationConstraints.Enabled);

            b &= !this.dgWindows.IsCurrentCellInEditMode;
            b &= !this.dgWindows.IsCurrentRowDirty;
            b &= this.ErrorCounterWindows.ErrorCount == 0;

            if (b == false)
            {
                Cmd.Enabled = b;
                return;
            }
            b &= !this.WindowsHasErrorText();
            Cmd.Enabled = b;
        }

       

        private void CmdCloseHandler(Command Cmd)
        {
            Cmd.Enabled = true;
        }
        #endregion

        #region Command Methods

        #region Cut / Copy / Paste / Select / Delete Handlers
        private void CmdEditCopy(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                Clipboard.SetText(this.FocusedTextbox.SelectedText);
            }
        }

        private void CmdEditCut(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                Clipboard.SetText(FocusedTextbox.SelectedText);

                FocusedTextbox.SelectedText = string.Empty;
            }
        }

        private void CmdEditPaste(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                string str = Clipboard.GetText();

                FocusedTextbox.Paste(str);
            }
        }

        private void CmdEditSelect(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                FocusedTextbox.SelectAll();
            }
        }

        private void CmdEditDelete(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                FocusedTextbox.SelectedText = "";
            }
        }

        private void CmdEditUndo(Command Cmd)
        {
            if (this.FocusedTextbox != null)
            {
                FocusedTextbox.Undo();
            }
        }

        #endregion
        private void CmdWindowSpyFind(Command Cmd)
        {
            OpenFileDialog Op = new OpenFileDialog();
            Op.CheckFileExists = true;
            Op.DefaultExt = "exe";
            Op.Filter = "EXE|*.exe";

            Op.Multiselect = false;
            Op.Title = Properties.Resources.FileFindWindwSpyTitle;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                
                WindowSpy.SetPath(Op.FileName);
                this.WindowSpyPath = Op.FileName;
                this.WindowSpyFound = true;
                
            }

        }
        private void CmdWindowSpy(Command Cmd)
        {
            Process.Start(this.WindowSpyPath);
        }

        private void CmdSave(Command Cmd)
        {
            if (this.ValidateChildren() == true)
            {
                profile p = this.ToProfile();
                if (this.IsNew == true)
                {
                    string sFileName = p.name.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase) == true ? p.name : p.name + ".xml";
                    if (Directory.Exists(AppCommon.Instance.PathProfiles) == false)
                    {
                        Directory.CreateDirectory(AppCommon.Instance.PathProfiles);
                    }
                    string sPath = Path.Combine(AppCommon.Instance.PathProfiles, sFileName);
                    p.File = sPath;
                    p.schemaVersion = AppCommon.Instance.MinAllowSchemaVersion;
                }
                XmlSerializer writer = new XmlSerializer(typeof(profile));
                FileStream file = File.Create(pf.File);
                writer.Serialize(file, p);
                file.Close();
                if (frmProfile.IsInstance == true)
                {
                    if (this.MainWindow != null)
                    {
                        this.MainWindow.Init();
                    }
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                }
              
            }
        }
        private void CmdClose(Command Cmd)
        {
            if (frmProfile.IsInstance == true)
            {
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            
        }
      
        #endregion

        #endregion

        #region Methods

        #region From Profile
        public void FromProfile(profile p)
        {
            if (p == null)
            {
                return;
            }
            this.IsNew = false;
            this.pf = p;
            this.ProfileName.FileName = p.name;
            this.ProfileVersion.Version = p.FullVersion.ToString();
            this.ProfileDisplayName.FieldText = p.codeLanguage.codeName;
            this.ProfileDisplayDescription.FieldText = p.codeLanguage.description;
            this.chkGlobal.Checked = p.globalProfile;
            this.WindowsClear();
            if (p.windows != null)
            {
                foreach (var w in p.windows)
                {
                    this.WindowsAdd(w.name, w.value);
                }
            }
        }
        #endregion

        #region To Profile
        public profile ToProfile()
        {
            pf.name = this.ProfileName.FileName;
            pf.version = this.ProfileVersion.Version;
            if (pf.codeLanguage == null)
            {
                pf.codeLanguage = new profileCodeLanguage();
            }
            pf.codeLanguage.codeName = this.ProfileName.FileName;
            pf.codeLanguage.codeName = this.ProfileDisplayName.FieldText;
            pf.codeLanguage.description = this.ProfileDisplayDescription.FieldText;
            pf.globalProfile = chkGlobal.Checked;

            List<profileWindow> pWindows = new List<profileWindow>();
            foreach (var w in this.windows)
            {
                profileWindow pw = new profileWindow();
                pw.name = w.name;
                pw.value = w.value;
                pWindows.Add(pw);

            }
            pf.windows = pWindows.ToArray();

            if (this.IsNew == true)
            {
                pf.codeLanguage.paths.mainData = pf.name;
                pf.codeLanguage.paths.plugin = pf.name;
                pf.codeLanguage.paths.snips = pf.name;
            }

            var ec = EndCharsSelectedOptions.FromList(this.EndCharsBindingList);
            pf.profileEndChars = ec.ToArray();

            return pf;
           
        }
        #endregion

        #region Windows
        private void dgWindows_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                       , MessageBoxButtons.YesNo
                       , MessageBoxIcon.Question
                       , MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
            profileWindow pWin;
            pWin = windows[e.Row.Index];
            // clear any errors the row has before deleting
            pWin.ClearErrors();
        }

        private void dgWindows_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgWindows.NewRowIndex != e.RowIndex)
            {
                int delColIndex = dgWindows.ColumnCount -1;
                if (e.ColumnIndex == delColIndex) // delete
                {
                    var result = MessageBox.Show(Properties.Resources.DeleteGeneralQuestion, Properties.Resources.DeleteConfirmTitle
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        this.windows.RemoveAt(e.RowIndex);

                    }
                }

            }
        }

        private void dgWindows_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            // add delete image to new rows
            int colIndex = dgWindows.ColumnCount -1;
            e.Row.Cells[colIndex].Value = Z.IconLibrary.Silk.Icon.Delete.GetImage();
        }

        private void DgWindowsAddImgColumns()
        {
            DataGridViewImageColumn del = new DataGridViewImageColumn();
            del.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            del.Width = 20;
            del.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgWindows.Columns.Add(del);
        }

        private bool WindowsHasErrorText()
        {
            bool hasErrorText = false;
            //replace this.dataGridView1 with the name of your datagridview control
            foreach (DataGridViewRow row in this.dgWindows.Rows)
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
        /// <summary>
        /// Clears the Windows List
        /// </summary>
        public void WindowsClear()
        {
            this.windows.Clear();
        }

        /// <summary>
        /// Adds a new Window to the windows list
        /// </summary>
        /// <param name="name">The Name of the window</param>
        /// <param name="window">The window to add</param>
        public void WindowsAdd(string name, string window)
        {
            profileWindow wp = windows.AddNew();
            wp.name = name;
            wp.value = window;
            //this.windows.Add(wp);
        }

        #endregion

        #endregion

        #region Form Events

        #region Form Load Events
        private void frmProfile_Load(object sender, EventArgs e)
        {
            windowsBindingSource.DataSource = windows;
            ProfileName.FieldEnabled = this.IsNew;
            if (this.IsNew)
            {
                // this app knows what the min version it works on for any
                // profile it creates so no reason to bother end user with min version.
                pf.minVersion = AppCommon.Instance.DefaultMinVersion.ToString();
            }

            
            if (pf.profileEndChars == null)
            {
                EndCharsBindingList = EndCharKeyMap.Instance.ToBindingList();
            }
            else
            {
               
                if (pf.profileEndChars == null || pf.profileEndChars.Length == 0) 
                {
                    EndCharsBindingList = EndCharKeyMap.Instance.ToBindingList();
                }
                else
                {
                    var ec = EndCharsSelectedOptions.FromArray(pf.profileEndChars);
                    EndCharsBindingList = EndCharKeyMap.Instance.ToBindingList(ec);
                }
               
            }
            bsEndChars.DataSource = this.EndCharsBindingList;
            
          
            ((ListBox)this.clbEndChars).DataSource = bsEndChars;
            ((ListBox)this.clbEndChars).DisplayMember = "DisplayValue";
            ((ListBox)this.clbEndChars).ValueMember = "Selected";

            for (int i = 0; i < clbEndChars.Items.Count; i++)
            {
                AhkKeyMapAhkMapValue mv = (AhkKeyMapAhkMapValue)clbEndChars.Items[i];
                clbEndChars.SetItemChecked(i, mv.Selected);
            }

            if (Properties.Settings.Default.ProWindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.ProLocation;
                Size = Properties.Settings.Default.ProSize;
            }
            else if (Properties.Settings.Default.ProWindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                Location = Properties.Settings.Default.ProLocation;
                Size = Properties.Settings.Default.ProSize;
            }
            else
            {
                Location = Properties.Settings.Default.ProLocation;
                Size = Properties.Settings.Default.ProSize;
            }
            mnuEditCut.Image = Z.IconLibrary.Silk.Icon.Cut.GetImage();
            mnuEditCopy.Image = Z.IconLibrary.Silk.Icon.PageCopy.GetImage();
            mnuEditPaste.Image = Z.IconLibrary.Silk.Icon.PagePaste.GetImage();
            mnuEditDelete.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            mnuEditUndo.Image = Z.IconLibrary.Silk.Icon.ArrowUndo.GetImage();

            cMnuEditCut.Image = Z.IconLibrary.Silk.Icon.Cut.GetImage();
            cMnuEditCopy.Image = Z.IconLibrary.Silk.Icon.PageCopy.GetImage();
            cMnuEditPaste.Image = Z.IconLibrary.Silk.Icon.PagePaste.GetImage();
            cMnuEditDelete.Image = Z.IconLibrary.Silk.Icon.Delete.GetImage();
            cMnuEditUndo.Image = Z.IconLibrary.Silk.Icon.ArrowUndo.GetImage();

            this.SetTextBoxGeneralEvents();
            this.AddTextBoxEditHandlers();
        }
        #endregion

        #region Form Cloading Events
        private void frmProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.ProLocation = RestoreBounds.Location;
                Properties.Settings.Default.ProWindowState = WindowState;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.ProLocation = Location;
                Properties.Settings.Default.ProSize = Size;
                Properties.Settings.Default.ProWindowState = WindowState;
            }
            else
            {
                Properties.Settings.Default.ProLocation = RestoreBounds.Location;
                Properties.Settings.Default.ProSize = RestoreBounds.Size;
                Properties.Settings.Default.ProWindowState = WindowState;
            }
        }

        private void frmProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
        #endregion

        #endregion

        #region End Chars Check
        private void clbEndChars_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                this.EndCharsBindingList[e.Index].Selected = true;
            }
            else
            {
                this.EndCharsBindingList[e.Index].Selected = false;
            }
        }
        #endregion

        #region Event related for Edit of textboxes

        #region Event General Event Handlers for Textboxes
        private void tBox_Enter(object sender, EventArgs e)
        {
            this.FocusedTextbox = (TextBox)sender;
        }

        private void tBox_Leave(object sender, EventArgs e)
        {
            this.FocusedTextbox = null;
        }
        #endregion

        #region Set User controls Enter and leave event handlers
        /// <summary>
        /// Capture Enter and leave for all textboxes that will be acted upon by the edit menus
        /// </summary>
        /// <remarks>
        /// All Edit menus are set to act upon <see cref="FocusedTextbox"/>.
        /// This method ensure that  <see cref="FocusedTextbox"/> will always be the current selected textbox.
        /// </remarks>
        private void SetTextBoxGeneralEvents()
        {
            ProfileName.TbField.Enter += tBox_Enter;
            ProfileName.TbField.Leave += tBox_Leave;

            ProfileVersion.TbField.Enter += tBox_Enter;
            ProfileVersion.TbField.Leave += tBox_Leave;

            ProfileDisplayName.TbField.Enter += tBox_Enter;
            ProfileDisplayName.TbField.Leave += tBox_Leave;

            ProfileDisplayDescription.TbField.Enter += tBox_Enter;
            ProfileDisplayDescription.TbField.Leave += tBox_Leave;
        }
        #endregion

        #region Add Event Handlers for edit to User control Textboxes
        /// <summary>
        /// Add Context Menu to textbox controls for usercontrols
        /// </summary>
        private void AddTextBoxEditHandlers()
        {
            ProfileName.TbField.ContextMenuStrip = cMnuEdit;
            ProfileVersion.TbField.ContextMenuStrip = cMnuEdit;
            ProfileDisplayName.TbField.ContextMenuStrip = cMnuEdit;
            ProfileDisplayDescription.TbField.ContextMenuStrip = cMnuEdit;
        }
        #endregion

        #region Popup Menu Event Handlers

        /// <summary>
        /// Sets focus on the current textbox the menu is being display for if the textbox does not already have focus.
        /// </summary>
        /// <param name="sender">ContextMenuStrip</param>
        /// <param name="e">CancelEventArgs</param>
        /// <remarks>
        /// If focus is not set on the textbox before the popup menu is displayed then the popup menu will be acting on any other textbox
        /// that may have focus.
        /// </remarks>
        private void cMnuEdit_Opening(object sender, CancelEventArgs e)
        {
            Control ctl = ((ContextMenuStrip)sender).SourceControl;
            if (ctl.Focused == false)
            {
                ctl.Focus();
            }
        }
        #endregion

        #endregion

        #region Disposing
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                cmdMgr.PreCommandProcess -= CommandPrePorcess;
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
